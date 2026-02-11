using Blazored.LocalStorage;
using Company.Shared.AuthModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace MyCompany.BlazorUI.AcountService
{
	public class AuthService : IAuthService
	{
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorageService;
		private readonly AuthenticationStateProvider _authenticationState;

		public AuthService(HttpClient httpClient,
			ILocalStorageService localStorageService,
			AuthenticationStateProvider authenticationStateProvider)
		{
			_httpClient = httpClient;
			_localStorageService = localStorageService;
			_authenticationState = authenticationStateProvider;
		}
		public async Task<GeneralResponse<AuthResponse>> Register(RegisterUser dto)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync("api/Account/register", dto);
				var result =  await response.Content.ReadFromJsonAsync<GeneralResponse<AuthResponse>>()!;
				if(result.Success && result.Data != null)
				{
					await _localStorageService.SetItemAsync("authToken", result.Data.Token);
					await _localStorageService.SetItemAsync("tokenExpiration", result.Data.Expiration);

					_httpClient.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", result.Data.Token);
					((CustomAuthStateProvider)_authenticationState).NotifyUserLogin(result.Data.Token);
				}
				return result ?? new GeneralResponse<AuthResponse>(false, "Registration failed");
			}
			catch(Exception ex)
			{
				return new GeneralResponse<AuthResponse>(false, $"An error occurred: {ex.Message}");
			}
		}
		public async Task<GeneralResponse<AuthResponse>> Login(LoginUser dto)
		{
			var response = await _httpClient.PostAsJsonAsync("api/Account/Login", dto);
			try
			{
				var result = await response.Content.ReadFromJsonAsync<GeneralResponse<AuthResponse>>()!;
				if (result.Success && result.Data != null)
				{
					await _localStorageService.SetItemAsync("authToken", result.Data.Token);
					await _localStorageService.SetItemAsync("tokenExpiration", result.Data.Expiration);

					_httpClient.DefaultRequestHeaders.Authorization =
						new AuthenticationHeaderValue("Bearer", result.Data.Token);
					((CustomAuthStateProvider)_authenticationState).NotifyUserLogin(result.Data.Token);
				}
				return result ?? new GeneralResponse<AuthResponse>(false, "Login failed");
			}
			catch (Exception ex)
			{
				return new GeneralResponse<AuthResponse>(false, $"An error occurred: {ex.Message}");
			}
		}
		public async Task Logout()
		{
			await _localStorageService.RemoveItemAsync("authToken");
			await _localStorageService.RemoveItemAsync("tokenExpiration");

			_httpClient.DefaultRequestHeaders.Authorization = null;
			((CustomAuthStateProvider)_authenticationState).NotifyUserLogout();
		}

	}
}
