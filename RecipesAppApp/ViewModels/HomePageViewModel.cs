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
using System.Runtime.CompilerServices;

namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(IsNotLogged),"IsNotLogged")]
    public class HomePageViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private ObservableCollection<Recipe> recipes;
        private ObservableCollection<Recipe> yourRecipes;
        private ObservableCollection<Recipe> recipesWithoutGloten;
        private ObservableCollection<Recipe> kosherRecipes;
        private string selectedNames;
        private ObservableCollection<Object> selectedRecipes;
        private Object selectedRecipe;
        private SignUpView signupView;
        private LoginView loginView;
        private bool isLogged;

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
        public ObservableCollection<Recipe> YourRecipes
        {
            get
            {
                return this.yourRecipes;
            }
            set
            {
                this.yourRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> RecipesWithoutGloten
        {
            get
            {
                return this.recipesWithoutGloten;
            }
            set
            {
                this.recipesWithoutGloten = value;
                OnPropertyChanged();
            }
        }

        public bool IsLogged
        {
            get { return ((App)Application.Current).LoggedInUser != null; }
        }
        #endregion
        private async void ReadRecipes()
        {
            
            List<Recipe> list = await RecipesService.GetAllRecipes();
            this.Recipes = new ObservableCollection<Recipe>(list);
        }

        private async void MakeRecipesList()
        {
            if(IsLogged != null)
            {
                List<Recipe> yourlist = this.Recipes.Where<Recipe>(r => r.MadeBy == ((App)Application.Current).LoggedInUser.Id).ToList();
                this.YourRecipes = new ObservableCollection<Recipe>(yourlist);
            }
            else
            {
                this.YourRecipes = new();
            }
            List<Recipe> NoGlotenList = this.Recipes.Where<Recipe>(r => r.IsGloten == "false").ToList();
            this.recipesWithoutGloten = new ObservableCollection<Recipe>(NoGlotenList);
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
