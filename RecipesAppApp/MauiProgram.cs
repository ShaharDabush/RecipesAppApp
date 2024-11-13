using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using RecipesAppApp.ViewModels;
using RecipesAppApp.Views;
using RecipesAppApp.Services;

namespace RecipesAppApp
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
            builder.Services.AddTransient<RecipeDetailsViews>();
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
            builder.Services.AddSingleton<CreateIngredientView>();
            builder.Services.AddSingleton<CreateRecipeView>();
            builder.Services.AddSingleton<EditProfileView>();
            builder.Services.AddSingleton<HomePageView>();
            builder.Services.AddSingleton<ProfileView>();
            builder.Services.AddSingleton<RecipeDetailsViews>();
            builder.Services.AddSingleton<RecipesAproveView>();
            builder.Services.AddSingleton<RemoveIngredientsView>();
            builder.Services.AddSingleton<SettingsView>();
            builder.Services.AddSingleton<StorageView>();
            builder.Services.AddSingleton<UsersListView>();
            return builder;
        }
    }
}
