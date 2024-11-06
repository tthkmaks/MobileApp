using Microsoft.Extensions.Logging;

namespace MaxSiCalcilatoor
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
                    fonts.AddFont("Cairo-Light.ttf", "RegularFront");
                    fonts.AddFont("Cairo-ExtraLight.ttf", "LightFront");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
