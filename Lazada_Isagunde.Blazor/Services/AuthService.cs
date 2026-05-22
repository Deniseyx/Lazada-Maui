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

    public string? UserEmail => _authClient.User?.Info?.Email;
    public string? UserDisplayName => _authClient.User?.Info?.DisplayName;
    public bool IsLoggedIn => !string.IsNullOrEmpty(UserId);

    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();

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
        
        NotifyStateChanged();
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
        try
        {
            var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
            UserToken = await userCredential.User.GetIdTokenAsync();
            UserId = userCredential.User.Uid;

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
        try
        {
            if (_authClient.User != null)
            {
                _authClient.SignOut();
            }
        }
        catch (Exception ex)
        {
            // Log or ignore - the goal is to ensure local state is cleared regardless
            Console.WriteLine($"SignOut error: {ex.Message}");
        }

        UserToken = null;
        UserId = null;
        await _localStorage.RemoveItemAsync("firebase_user_token");
        await _localStorage.RemoveItemAsync("firebase_user_id");
        NotifyStateChanged();
    }

    private async Task SaveStateToLocalStorage()
    {
        await _localStorage.SetItemAsync("firebase_user_token", UserToken);
        await _localStorage.SetItemAsync("firebase_user_id", UserId);
        NotifyStateChanged();
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
