using ArchiveGHOST.Client.Popups;
using ArchiveGHOST.Client.Services;
using ArchiveGOST_DbLibrary;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer;
using VMLayer;
using VMLayer.Navigation;

namespace ArchiveGHOST.Client
{
    public static class MauiProgram
    {
        //private static IConfigurationRoot _configuration;
        
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsRegular");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiCommunityToolkit();

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            builder.Services.AddTransientWithShellRoute<InventoryListPage, OriginalListViewModel>("InventoryPage");
            //builder.Services.AddTransient<InventoryListPage>();
            //builder.Services.AddTransient<OriginalListViewModel>();

            builder.Services.AddTransientWithShellRoute<OriginalDetailPage, EditOriginalViewModel>("OriginalDetail");
            //builder.Services.AddTransient<OriginalDetailPage>();
            //builder.Services.AddTransient<OriginalDetailViewModel>();

            builder.Services.AddTransientWithShellRoute<CreateOriginalPage, CreateOriginalViewModel>("CreateOriginal");
            //builder.Services.AddTransient<CreateOriginalPage>();
            //builder.Services.AddTransient<CreateOriginalViewModel>();


            //НАДО РАЗОБРАТЬСЯ НАХОДИТСЯ НАША БАЗА
            //ПОКА РАБОТАЕТ ТОЛЬКО СЕРВЕРА

            builder.Services.AddDbContext<ArchiveDbContext>(op =>
            op.UseSqlServer(builder.Configuration.GetConnectionString("ArchiveLibrarySQLServer")));
            //op.UseSqlServer("Data Source=localhost;Initial Catalog=ArchiveGostDb;Trusted_Connection=True;Encrypt = True;Trust Server Certificate=True"));

            builder.Services.AddTransient<IOriginalService, OriginalService>();
            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IApplicabilityService, ApplicabilityService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            builder.Services.AddTransientPopup<ApplicabilityDetailPopup, ApplicabilityDetailViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
