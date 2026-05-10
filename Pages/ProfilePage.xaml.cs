using Lazada_Isagunde.Services;

using Lazada_Isagunde.Models;

namespace Lazada_Isagunde.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly AuthService _authService;
    private readonly FirebaseService _firebaseService;
    private UserProfile _currentProfile;

	public ProfilePage()
	{
		InitializeComponent();
        _authService = App.Services.GetService<AuthService>()!;
        _firebaseService = App.Services.GetService<FirebaseService>()!;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProfile();
    }

    private async Task LoadProfile()
    {
        var userId = AuthService.UserId;
        if (!string.IsNullOrEmpty(userId))
        {
            _currentProfile = await _firebaseService.GetUserProfileAsync(userId);
            
            // Fill entries
            FullNameEntry.Text = _currentProfile.FullName;
            PhoneNumberEntry.Text = _currentProfile.PhoneNumber;
            AddressEditor.Text = _currentProfile.ShippingAddress;

            // Update header
            HeaderNameLabel.Text = string.IsNullOrEmpty(_currentProfile.FullName) ? "No Name Set" : _currentProfile.FullName;
            HeaderPhoneLabel.Text = string.IsNullOrEmpty(_currentProfile.PhoneNumber) ? "No Phone Set" : _currentProfile.PhoneNumber;
            HeaderEmailLabel.Text = AuthService.UserEmail ?? "No Email";
        }
    }

    private async void OnSaveProfileClicked(object sender, EventArgs e)
    {
        if (_currentProfile == null) return;

        _currentProfile.FullName = FullNameEntry.Text;
        _currentProfile.PhoneNumber = PhoneNumberEntry.Text;
        _currentProfile.ShippingAddress = AddressEditor.Text;

        await _firebaseService.SaveUserProfileAsync(_currentProfile);
        
        // Update header immediately
        HeaderNameLabel.Text = _currentProfile.FullName;
        HeaderPhoneLabel.Text = _currentProfile.PhoneNumber;

        await DisplayAlert("Success", "Profile information saved!", "OK");
    }

    private async void OnSellerCenterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("SellerCenter");
    }

    private async void OnMyOrdersClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PurchaseHistoryPage());
    }

    private async void OnContactUsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ContactUs");
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        _authService.Logout();
        if (Application.Current != null)
        {
            Application.Current.MainPage = new NavigationPage(new LandingPage());
        }
    }
}
