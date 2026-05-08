namespace Lazada_Isagunde.Pages;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

    private void OnSendLinkClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}
