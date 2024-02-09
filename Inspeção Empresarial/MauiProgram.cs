using CommunityToolkit.Maui;
using Inspeção_Empresarial.Repositories;

namespace Inspeção_Empresarial
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();
            builder.Services.AddSingleton<IEstablishmentRepository, EstablishmentRepository>();
            builder.Services.AddSingleton<IResponsibilityRepository, ResponsibilityRepository>();
            builder.Services.AddSingleton<IProcessDescriptionRepository, ProcessDescriptionRepository>();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
