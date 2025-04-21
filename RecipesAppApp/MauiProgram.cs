using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RecipesAppApp.ViewModels;
using RecipesAppApp.Views;
using RecipesAppApp.Services;
using CommunityToolkit.Maui;
using Syncfusion.Maui.Core.Hosting;

namespace RecipesAppApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
                builder.ConfigureSyncfusionCore();
                builder
                    .UseMauiApp<App>()
                    .RegisterDataServices()
                    .RegisterPages()
                    .RegisterViewModels();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        public static MauiAppBuilder RegisterPages(this MauiAppBuilder builder)
        {

            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<SignUpView>();
            builder.Services.AddTransient<LoginView>();
            builder.Services.AddTransient<CreateIngredientView>();
            builder.Services.AddTransient<CreateRecipeView>();
            builder.Services.AddTransient<EditProfileView>();
            builder.Services.AddTransient<HomePageView>();
            builder.Services.AddTransient<ProfileView>();
            builder.Services.AddTransient<RecipeDetailsView>();
            builder.Services.AddTransient<RecipesAproveView>();
            builder.Services.AddTransient<RemoveIngredientsView>();
            builder.Services.AddTransient<SettingsView>();
            builder.Services.AddTransient<AddPopup>();
            builder.Services.AddTransient<StorageView>();
            builder.Services.AddTransient<UsersListView>();

            return builder;
        }

        public static MauiAppBuilder RegisterDataServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<RecipesAppWebAPIProxy>();
            return builder;
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<CreateRecipeViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<RecipeDetailsViewModel>();
            builder.Services.AddTransient<RecipesAproveViewModel>();
            builder.Services.AddTransient<RemoveIngredientsViewModel>();
            builder.Services.AddTransient<SettingViewModel>();
            builder.Services.AddTransient<StorageViewModel>();
            builder.Services.AddTransient<UsersListViewModel>();
            builder.Services.AddTransient<ShellViewModel>();
            return builder;
        }
    }
}
