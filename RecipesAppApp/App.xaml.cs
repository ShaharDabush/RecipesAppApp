using RecipesAppApp.Models;
using RecipesAppApp.Views;
using RecipesAppApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using RecipesAppApp.Services;

namespace RecipesAppApp
{
    public partial class App : Application
    {
        //Use this class to store global application data that should be accessible throughout the entire app!

        //this is the current user that is logged in
        public User LoggedInUser { get; set; }
        public Storage UserStorage { get; set; }
        //this is the Login page we have to create one here to not cause a loop couse login => shell == > login if we create a login on logout and not now
        public HomePageView HomePage;
        public LoginView Login;
        public SignUpView SignUp;
        
        private RecipesAppWebAPIProxy RecipesService;
        public App(IServiceProvider serviceProvider, RecipesAppWebAPIProxy proxy)
        {
            RecipesService = proxy;
            LoggedInUser = null;
            InitializeComponent();

            MainPage = serviceProvider.GetService<AppShell>();

        }

        public async void GetUserStorage()
        {
            if (LoggedInUser.StorageId!=null) 
                UserStorage = await this.RecipesService.GetStoragesbyUser(LoggedInUser.StorageId.Value);
        }
    }
}
