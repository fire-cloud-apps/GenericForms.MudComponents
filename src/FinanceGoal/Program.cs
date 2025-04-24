using FinanceGoal;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FinanceGoal.Helper;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<ISupabaseAuthService, SupabaseAuthService>(client =>
{
    client.BaseAddress = new Uri("https://cqfkvyppzrwjrikyaqsv.supabase.co/auth/v1/");
});
// register both the abstraction and the concrete provider:
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    sp => sp.GetRequiredService<CustomAuthStateProvider>());

// Database Entry
builder.Services.AddHttpClient<IGoalHttpUtility, GoalHttpUtility>(client =>
{
    client.BaseAddress = new Uri("https://cqfkvyppzrwjrikyaqsv.supabase.co/rest/v1/");
});

builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddMudServices();

// General HTTP Client Service
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();