using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly AuthService _authService;

	public RegisterPage()
	{
		InitializeComponent();
        _authService = App.Services.GetService<AuthService>()!;
	}

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string name = NameEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
        {
            await DisplayAlert("Error", "Please fill all fields.", "OK");
            return;
        }

        var (success, error) = await _authService.RegisterUser(email, password);

        if (success)
        {
            // Save user profile with the name
            try
            {
                var firebaseService = App.Services.GetService<FirebaseService>();
                if (firebaseService != null && !string.IsNullOrEmpty(AuthService.UserId))
                {
                    await firebaseService.SaveUserProfileAsync(new Models.UserProfile
                    {
                        Id = AuthService.UserId,
                        FullName = name,
                        Email = email
                    });
                }
            }
            catch { /* Ignore profile save errors during registration */ }

            await DisplayAlert("Success", "Account created! Please login.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Registration Failed", error, "OK");
        }
    }
}
