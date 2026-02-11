using Blazored.LocalStorage;
using Company.Shared;
using Company.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyCompany.BlazorUI;
using MyCompany.BlazorUI.AcountService;
using MyCompany.BlazorUI.Repository;
using MyCompany.BlazorUI.Repository.DepartmentRepository;
using MyCompany.BlazorUI.Repository.EmployeeRepository;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

///
builder.Services.AddScoped(sp=> new HttpClient { BaseAddress = new Uri("https://localhost:5184/") });

builder.Services.AddScoped<IRepository<Employee>,EmployeeRepository>();
builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IRepository<Department>,DepartmentRepository>();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IAuthService, AuthService>();

// 4. السطر ربط الـ Provider بتاعك بنظام Blazor
builder.Services.AddScoped<CustomAuthStateProvider>(); 
// نسجله لنفسه عشان نقدر نعمل Casting
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthStateProvider>());


await builder.Build().RunAsync();
