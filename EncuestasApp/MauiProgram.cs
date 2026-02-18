using CommunityToolkit.Maui;
using EncuestaApp.Popups;
using EncuestaApp.Services;
using EncuestasApp;
using Microsoft.Extensions.Logging;

namespace EncuestaApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
                });

            builder.Services.AddSingleton<DatabaseService>(s =>
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "encuestas.db3");
                return new DatabaseService(dbPath);
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            _ = typeof(LocalidadPopup);
            return builder.Build();
        }
    }
}
