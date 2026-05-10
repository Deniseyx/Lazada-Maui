using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly AuthService _authService;

	public ForgotPasswordPage()
	{
		InitializeComponent();
        _authService = App.Services.GetService<AuthService>()!;
	}

    private async void OnSendLinkClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("Error", "Please enter your email address.", "OK");
            return;
        }

        var (success, error) = await _authService.ResetPassword(email);

        if (success)
        {
            await DisplayAlert("Success", "A password reset link has been sent to your email.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", error, "OK");
        }
    }
}
