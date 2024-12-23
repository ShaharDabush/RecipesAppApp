using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.Views;
using RecipesAppApp.Services;
using System.Collections.ObjectModel;
using RecipesAppApp.Models;

namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(IsNotLogged),"IsNotLogged")]
    public class HomePageViewModel : ViewModelBase
    {
        private RecipesAppWebAPIProxy RecipesService;
        private ObservableCollection<Recipe> recipes;
        private string selectedNames;
        private ObservableCollection<Object> selectedRecipes;
        private Object selectedRecipe;
        private SignUpView signupView;
        private LoginView loginView;
        public ICommand LoginCommand { get; set; }
        public Command SignUpCommand { protected set; get; }

        private bool isNotLogged;
        public bool IsNotLogged
        {
            get { return isNotLogged; }
            set
            {
                isNotLogged = value;
                OnPropertyChanged("IsNotLogged");
            }
        }
        public Object SelectedRecipe
        {
            get
            {
                return this.selectedRecipe;
            }
            set
            {
                this.selectedRecipe = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Object> SelectedRecipes
        {
            get
            {
                return this.selectedRecipes;
            }
            set
            {
                this.selectedRecipes = value;
                OnPropertyChanged();
            }
        }
        public string SelectedNames
        {
            get
            {
                return this.selectedNames;
            }
            set
            {
                this.selectedNames = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return this.recipes;
            }
            set
            {
                this.recipes = value;
                OnPropertyChanged();
            }
        }
        private async void ReadRecipes()
        {
            
            List<Recipe> list = await RecipesService.GetAllRecipes();
            this.Recipes = new ObservableCollection<Recipe>(list);
        }

        public HomePageViewModel(RecipesAppWebAPIProxy service, SignUpView signUp, LoginView login)
        {
            this.signupView = signUp;
            this.RecipesService = service;
            this.loginView = login;
            recipes = new ObservableCollection<Recipe>();
            SelectedNames = "none";
            SelectedRecipes = new ObservableCollection<Object>();
            this.LoginCommand = new Command(GoToLogin);
            this.SignUpCommand = new Command(GoToSignUp);
            ReadRecipes();
        }

        private async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(signupView);

        }
        private async void GoToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(loginView);

        }
        async void OnSingleSelectRecipe()
        {
            if (SelectedRecipe == null || !(SelectedRecipe is Recipe))
            {
                SelectedNames = "none";
            }
            else
                SelectedNames = ((Recipe)SelectedRecipe).RecipesName;
        }
        
    }
}
