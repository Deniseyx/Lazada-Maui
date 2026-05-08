using System;
 
namespace Lazada_Isagunde
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; set; }

        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new Pages.SplashScreen());

            // Force mobile dimensions for desktop windows
            window.Width = 360;
            window.Height = 640;

            return window;
        }
    }
}