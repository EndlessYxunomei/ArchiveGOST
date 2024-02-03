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

            builder.Services.AddTransientWithShellRoute<InventoryListPage, OriginalListViewModel>(NavigationConstants.InventoryPage);
            builder.Services.AddTransientWithShellRoute<OriginalDetailPage, EditOriginalViewModel>(NavigationConstants.OriginalDetail);
            builder.Services.AddTransientWithShellRoute<CreateOriginalPage, CreateOriginalViewModel>(NavigationConstants.CreateOriginal);
            builder.Services.AddTransientWithShellRoute<CorrectionDetailPage, CorrectionDetailViewModel>(NavigationConstants.CorrectionDetail);
            builder.Services.AddTransientWithShellRoute<DocumentListPage, DocumentListViewModel>(NavigationConstants.DocumentList);
            builder.Services.AddTransientWithShellRoute<CompanyListPage, CompanyListViewModel>(NavigationConstants.CompanyList);
            builder.Services.AddTransientWithShellRoute<ApplicabilityListPage, ApplicabilityListViewModel>(NavigationConstants.ApplicabilityList);

            //НАДО РАЗОБРАТЬСЯ ГДЕ НАХОДИТСЯ НАША БАЗА
            //ПОКА РАБОТАЕТ ТОЛЬКО СЕРВЕРА

            //builder.Services.AddDbContext<ArchiveDbContext>(op =>
            //op.UseSqlServer(builder.Configuration.GetConnectionString("ArchiveLibrarySQLServer")));
            builder.Services.AddDbContext<ArchiveDbContext>(op =>
                op.UseSqlite(builder.Configuration.GetConnectionString("ArchiveLibrary")));

            builder.Services.AddTransient<IOriginalService, OriginalService>();
            builder.Services.AddTransient<IDocumentService, DocumentService>();
            builder.Services.AddTransient<ICompanyService, CompanyService>();
            builder.Services.AddTransient<IPersonService, PersonService>();
            builder.Services.AddTransient<IApplicabilityService, ApplicabilityService>();
            builder.Services.AddTransient<ICorrectionService, CorrectionService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            builder.Services.AddTransientPopup<ApplicabilityDetailPopup, ApplicabilityDetailViewModel>();
            builder.Services.AddTransientPopup<CompanyDetailPopup, CompanyDetailViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
