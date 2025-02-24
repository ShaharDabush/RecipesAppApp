using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RecipesAppApp.ViewModels;
using RecipesAppApp.Views;
using RecipesAppApp.Services;
using CommunityToolkit.Maui;

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
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }) 
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
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<CreateIngredientViewModel>();
            builder.Services.AddSingleton<CreateRecipeViewModel>();
            builder.Services.AddSingleton<EditProfileViewModel>();
            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<ProfileViewModel>();
            builder.Services.AddSingleton<RecipeDetailsViewModel>();
            builder.Services.AddSingleton<RecipesAproveViewModel>();
            builder.Services.AddSingleton<RemoveIngredientsViewModel>();
            builder.Services.AddSingleton<SettingViewModel>();
            builder.Services.AddSingleton<StorageViewModel>();
            builder.Services.AddSingleton<UsersListViewModel>();
            builder.Services.AddSingleton<ShellViewModel>();
            return builder;
        }
    }
}
