using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MyCompany.BlazorUI.AcountService
{
	//الكلاس الأساسي في Blazor المسؤول عن معرفة "من هو المستخدم". بدون هذه الوراثة، لن يفهم المتصفح أي شيء عن تسجيل الدخول.
    //HttpClient: نحتاجه لنضع "التوكن" في رأس (Header) أي طلب يخرج من التطبيق للـ API.
    //ILocalStorageService: نحتاجه لنقرأ التوكن المحفوظ في المتصفح، حتى لا يضطر المستخدم لتسجيل الدخول في كل مرة يغلق فيها الصفحة.
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorageService;
		public CustomAuthStateProvider(HttpClient httpClient,ILocalStorageService localStorageService)
		{
			_httpClient = httpClient;
			_localStorageService = localStorageService;
		}

//ميثود GetAuthenticationStateAsync(البحث عن الهوية)
//هذه الميثود تعمل تلقائياً بمجرد فتح التطبيق أو عمل Refresh.

//var token = await _localStorageService.GetItemAsync<string>("authToken");: أول شيء نفعله هو سؤال المتصفح: "هل معك مفتاح (Token) لهذا المستخدم؟".

//if (string.IsNullOrEmpty(token)): لو لم نجد توكن، نرجع حالة "مجهول" (Anonymous)، وهذا ما يجعل الـ Navbar يظهر كلمة "Login".

//_httpClient.DefaultRequestHeaders.Authorization = ...: لو وجدنا توكن، نضعه فوراً في الـ HttpClient، لكي يذهب مع كل طلب API قادم كـ "إثبات هوية".

//var user = CreateClaimsPrincipal(token);: نرسل التوكن ليتم فكه وتحويله لشخص(User) معروف البيانات.
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			// get token from local storage
			var token = await _localStorageService.GetItemAsync<string>("authToken");
			if (string.IsNullOrEmpty(token))
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}
			var expiration = await _localStorageService.GetItemAsync<DateTime>("tokenExpiration");

			//  التوكن انتهى
			if (expiration < DateTime.UtcNow)
			{
				await _localStorageService.RemoveItemAsync("authToken");
				await _localStorageService.RemoveItemAsync("tokenExpiration");

				return new AuthenticationState(
					new ClaimsPrincipal(new ClaimsIdentity())
				);
			}
			// put table in headers
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var user = CreateClaimsPrincipal(token);
			// read claims from token
			return new AuthenticationState(user);
		}
// when user login
//NotifyUserLogin(string token) : عندما يسجل المستخدم دخوله بنجاح، نستخدم هذا السطر لإخبار كل أجزاء التطبيق فوراً: "يا جماعة، الحالة تغيرت، لدينا مستخدم جديد!".
		public void NotifyUserLogin(string token)
		{
			var user = CreateClaimsPrincipal(token);
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}
		private ClaimsPrincipal CreateClaimsPrincipal(string token)
		{
			var claims = ParseClaimsFromJwt(token); // ميثود فك التوكن
			var identity = new ClaimsIdentity(claims, "jwt");
			return new ClaimsPrincipal(identity);
		}
		//NotifyUserLogout() : تفعل العكس، تمسح حالة المستخدم وترجعها "مجهول"، مما يخفي الصفحات المحمية فوراً.
		public void NotifyUserLogout()
		{
			var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
		}
//		هنا "السحر" الحقيقي.التوكن عبارة عن 3 أجزاء مفصولة بنقطة. (Header.Payload.Signature).
//var payload = jwt.Split('.')[1];: نحن نحتاج الجزء الثاني(Index 1) لأنه هو الذي يحتوي على بيانات المستخدم(الاسم، الدور، الإيميل).
//ParseBase64WithoutPadding(payload) : التوكن يكون مضغوطاً بصيغة Base64، نحوله لمصفوفة بايتات(Bytes) لنتمكن من قراءته.
//JsonSerializer.Deserialize<Dictionary<...>>: نحول نص الـ JSON الموجود داخل التوكن إلى قاموس(Dictionary) من "مفتاح وقيمة" لسهولة التعامل معه.
//الجزء الخاص بالـ Roles(kvp.Key == ClaimTypes.Role): الـ JWT إذا كان المستخدم له أكثر من دور(مثل Admin و Manager)، يرسلها كمصفوفة[]. هذا الكود يتأكد من فك المصفوفة وإضافة كل دور كـ Claim منفصل، لكي تعمل الـ[Authorize(Roles = "Admin")] بشكل صحيح.
		private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			var claims = new List<Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			if (keyValuePairs != null)
			{
				foreach (var kvp in keyValuePairs)
				{
					// معالجة الأدوار (Roles) إذا كانت مصفوفة أو سلسلة واحدة
					if (kvp.Key == ClaimTypes.Role && kvp.Value.ToString().StartsWith("["))
					{
						var roles = JsonSerializer.Deserialize<string[]>(kvp.Value.ToString());
						foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));
					}
					else
					{
						claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
					}
				}

			}
			return claims;
		}
//لسبب: لغة الـ C# تتوقع أن يكون طول نص الـ Base64 دائماً من مضاعفات رقم 4. الـ JWT بطبيعته يحذف علامات التساوي (=) من الآخر ليكون أقصر.
//الحل: هذا الكود يعيد إضافة علامات = المفقودة(Padding) حسب طول النص، لكي لا يضرب الكود Exception أثناء التحويل.
		private byte[] ParseBase64WithoutPadding(string base64)
		{
			// بنشوف النص ناقصه كام حرف عشان يكمل لـ 4
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break; // ناقص حرفين، نزود ==
				case 3: base64 += "="; break;  // ناقص حرف، نزود =
			}
			// دلوقتي الـ C# هيقبله وهو مطمن
			return Convert.FromBase64String(base64);
		}

	}
}
