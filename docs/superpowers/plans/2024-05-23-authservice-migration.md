# AuthService Migration (WASM Adaptation) Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement a persistent, instance-based `AuthService` for Blazor WASM using `Blazored.LocalStorage`.

**Architecture:** Scoped service pattern with persistence in local storage to maintain session across refreshes.

**Tech Stack:** C#, Blazor WASM, Firebase Auth, Blazored.LocalStorage.

---

### Task 1: Implement AuthService.cs

**Files:**
- Overwrite: `Lazada_Isagunde.Blazor/Services/AuthService.cs`

- [ ] **Step 1: Write AuthService implementation**
The implementation should handle registration, login, logout, and token refresh, using `ILocalStorageService` for state persistence.

```csharp
using Firebase.Auth;
using Firebase.Auth.Providers;
using Blazored.LocalStorage;

namespace Lazada_Isagunde.Blazor.Services;

public class AuthService
{
    private const string WebApiKey = "AIzaSyCOWMhbhbuczuHYYx-GLlK7xCpO-qSs7QU";
    private const string AuthDomain = "isagunde-lazada.firebaseapp.com";

    private readonly FirebaseAuthClient _authClient;
    private readonly ILocalStorageService _localStorage;

    public string? UserToken { get; private set; }
    public string? UserId { get; private set; }
    public bool IsAdmin { get; private set; }

    public string? UserEmail => _authClient.User?.Info?.Email;
    public string? UserDisplayName => _authClient.User?.Info?.DisplayName;
    public bool IsLoggedIn => !string.IsNullOrEmpty(UserId);

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        var config = new FirebaseAuthConfig
        {
            ApiKey = WebApiKey,
            AuthDomain = AuthDomain,
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        };

        _authClient = new FirebaseAuthClient(config);
    }

    public async Task InitializeAsync()
    {
        UserToken = await _localStorage.GetItemAsync<string>("firebase_user_token");
        UserId = await _localStorage.GetItemAsync<string>("firebase_user_id");
        IsAdmin = await _localStorage.GetItemAsync<bool>("is_admin");

        if (_authClient.User != null)
        {
            try
            {
                UserToken = await _authClient.User.GetIdTokenAsync();
                await _localStorage.SetItemAsync("firebase_user_token", UserToken);
            }
            catch
            {
                await Logout();
            }
        }
    }

    public async Task<string> GetFreshTokenAsync()
    {
        if (_authClient.User != null)
        {
            UserToken = await _authClient.User.GetIdTokenAsync();
            await _localStorage.SetItemAsync("firebase_user_token", UserToken);
            return UserToken;
        }
        return UserToken ?? string.Empty;
    }

    public async Task<(bool Success, string ErrorMessage)> RegisterUser(string email, string password)
    {
        try
        {
            var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
            UserToken = await userCredential.User.GetIdTokenAsync();
            UserId = userCredential.User.Uid;
            IsAdmin = false;

            await SaveStateToLocalStorage();
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, GetFriendlyErrorMessage(ex.Message));
        }
    }

    public async Task<(bool Success, string ErrorMessage)> LoginUser(string email, string password)
    {
        if (email == "adminlogin" && password == "admin123")
        {
            IsAdmin = true;
            UserId = "ADMIN_ID";
            UserToken = "ADMIN_TOKEN";
            await SaveStateToLocalStorage();
            return (true, string.Empty);
        }

        try
        {
            var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            UserToken = await userCredential.User.GetIdTokenAsync();
            UserId = userCredential.User.Uid;
            IsAdmin = false;

            await SaveStateToLocalStorage();
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, GetFriendlyErrorMessage(ex.Message));
        }
    }

    public async Task<(bool Success, string ErrorMessage)> ResetPassword(string email)
    {
        try
        {
            await _authClient.ResetEmailPasswordAsync(email);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, GetFriendlyErrorMessage(ex.Message));
        }
    }

    public async Task Logout()
    {
        if (!IsAdmin)
        {
            _authClient.SignOut();
        }
        UserToken = null;
        UserId = null;
        IsAdmin = false;
        await _localStorage.RemoveItemAsync("firebase_user_token");
        await _localStorage.RemoveItemAsync("firebase_user_id");
        await _localStorage.RemoveItemAsync("is_admin");
    }

    private async Task SaveStateToLocalStorage()
    {
        await _localStorage.SetItemAsync("firebase_user_token", UserToken);
        await _localStorage.SetItemAsync("firebase_user_id", UserId);
        await _localStorage.SetItemAsync("is_admin", IsAdmin);
    }

    private string GetFriendlyErrorMessage(string technicalMessage)
    {
        if (string.IsNullOrEmpty(technicalMessage)) return "An unexpected error occurred.";
        if (technicalMessage.Contains("INVALID_LOGIN_CREDENTIALS") || technicalMessage.Contains("INVALID_PASSWORD"))
            return "Invalid email or password.";
        if (technicalMessage.Contains("EMAIL_EXISTS"))
            return "Email already exists.";
        if (technicalMessage.Contains("WEAK_PASSWORD"))
            return "Password should be at least 6 characters.";
        if (technicalMessage.Contains("INVALID_EMAIL") || technicalMessage.Contains("USER_NOT_FOUND"))
            return "No user found with this email address.";
        return technicalMessage;
    }
}
```

- [ ] **Step 2: Commit**
```bash
git add Lazada_Isagunde.Blazor/Services/AuthService.cs
git commit -m "feat: implement persistent AuthService for Blazor"
```

---

### Task 2: Register Service in Program.cs

**Files:**
- Modify: `Lazada_Isagunde.Blazor/Program.cs`

- [ ] **Step 1: Register BlazoredLocalStorage and AuthService**

```csharp
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Lazada_Isagunde.Blazor;
using Blazored.LocalStorage;
using Lazada_Isagunde.Blazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Add these lines
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
```

- [ ] **Step 2: Commit**
```bash
git add Lazada_Isagunde.Blazor/Program.cs
git commit -m "config: register AuthService and LocalStorage in Program.cs"
```

---

### Task 3: Verify Build

- [ ] **Step 1: Run dotnet build**
Run: `dotnet build Lazada_Isagunde.Blazor/Lazada_Isagunde.Blazor.csproj`
Expected: SUCCESS
