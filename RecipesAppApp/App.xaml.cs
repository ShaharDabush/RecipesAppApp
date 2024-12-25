using RecipesAppApp.Models;
using RecipesAppApp.Views;
using RecipesAppApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace RecipesAppApp
{
    public partial class App : Application
    {
        //Use this class to store global application data that should be accessible throughout the entire app!

        //this is the current user that is logged in
        public User LoggedInUser { get; set; }

        //this is the Login page we have to create one here to not cause a loop couse login => shell == > login if we create a login on logout and not now
        public HomePageView HomePage;
        public LoginView Login;
        public SignUpView SignUp;
        public App(IServiceProvider serviceProvider)
        {
            LoggedInUser = null;
            InitializeComponent();

            MainPage = serviceProvider.GetService<AppShell>();

        }
    }
}
