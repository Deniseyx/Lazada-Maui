using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class LoginPage : ContentPage
{
    private readonly AuthService _authService;

	public LoginPage()
	{
		InitializeComponent();
        _authService = App.Services.GetService<AuthService>()!;
	}

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Please enter email and password.", "OK");
            return;
        }

        var (success, error) = await _authService.LoginUser(email, password);

        if (success)
        {
            if (Application.Current != null)
            {
                if (AuthService.IsAdmin)
                {
                    Application.Current.MainPage = new AdminDashboardPage();
                }
                else
                {
                    Application.Current.MainPage = new AppShell();
                }
            }
        }
        else
        {
            await DisplayAlert("Login Failed", error, "OK");
        }
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }
}
