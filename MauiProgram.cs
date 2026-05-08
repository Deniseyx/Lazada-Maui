using Microsoft.Extensions.Logging;
using Lazada_Isagunde.Services;

namespace Lazada_Isagunde
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<FirebaseService>();
            builder.Services.AddSingleton<ImageService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            App.Services = app.Services;
            return app;
        }
    }
}
