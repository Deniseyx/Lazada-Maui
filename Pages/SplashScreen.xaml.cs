using Lazada_Isagunde.Services;

namespace Lazada_Isagunde.Pages;

public partial class SplashScreen : ContentPage
{
    private readonly AuthService _authService;

	public SplashScreen()
	{
		InitializeComponent();
        _authService = App.Services.GetService<AuthService>()!;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Initialize Auth session
        await _authService.InitializeAsync();

        // Wait a bit for the splash effect
        await Task.Delay(2000);
        
        if (Application.Current != null)
        {
            if (AuthService.IsLoggedIn)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new LandingPage());
            }
        }
    }
}
