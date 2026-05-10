using Lazada_Isagunde.Pages;

namespace Lazada_Isagunde
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Profile", typeof(ProfilePage));
            Routing.RegisterRoute("Settings", typeof(SettingsPage));
            Routing.RegisterRoute("SellerCenter", typeof(SellerCenterPage));
            Routing.RegisterRoute("ContactUs", typeof(ContactUsPage));
        }
    }
}
