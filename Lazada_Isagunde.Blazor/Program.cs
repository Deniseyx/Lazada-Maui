using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Lazada_Isagunde.Blazor;
using Blazored.LocalStorage;
using Lazada_Isagunde.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FirebaseService>();

var host = builder.Build();

var authService = host.Services.GetRequiredService<AuthService>();
await authService.InitializeAsync();

await host.RunAsync();
