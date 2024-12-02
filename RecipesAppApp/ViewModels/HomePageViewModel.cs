using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.Views;
using RecipesAppApp.Services;

namespace RecipesAppApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private RecipesAppWebAPIProxy RecipesService;
        private SignUpView signupView;
        private LoginView loginView;
        public ICommand LoginCommand { get; set; }
        public Command SignUpCommand { protected set; get; }
        public HomePageViewModel(RecipesAppWebAPIProxy service, SignUpView signUp, LoginView login)
        {
            this.signupView = signUp;
            this.RecipesService = service;
            this.loginView = login;
            this.LoginCommand = new Command(GoToLogin);
            this.SignUpCommand = new Command(GoToSignUp);
        }

        private async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(signupView);

        }
        private async void GoToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(loginView);

        }

    }
}
