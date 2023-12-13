using CommunityToolkit.Maui;
using Inspeção_Empresarial.Repositories;
using Inspeção_Empresarial.ViewModels.Inspection;
using Inspeção_Empresarial.Views.Inspection;
using InspecaoEmpresarial.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inspeção_Empresarial
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();
            builder.Services.AddSingleton<IEstablishmentRepository, EstablishmentRepository>();
            builder.Services.AddSingleton<IProcessDescriptionRepository, ProcessDescriptionRepository>();
            builder.Services.AddSingleton<IResponsibilityRepository, ResponsibilityRepository>();

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
