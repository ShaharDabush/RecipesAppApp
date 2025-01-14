﻿using System;
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
        private ObservableCollection<Recipe> recipesWithoutLactose;
        private ObservableCollection<Recipe> mostPopularRecipes;
        private ObservableCollection<Recipe> kosherRecipes;
        private ObservableCollection<Object> selectedRecipes;
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
        public bool IsLogged
        {
            get { return ((App)Application.Current).LoggedInUser != null; }
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
        public ObservableCollection<Recipe> RecipesWithoutLactose
        {
            get
            {
                return this.recipesWithoutLactose;
            }
            set
            {
                this.recipesWithoutLactose = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> MostPopularRecipes
        {
            get
            {
                return this.mostPopularRecipes;
            }
            set
            {
                this.mostPopularRecipes = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<Recipe> KosherRecipes
        {
            get
            {
                return this.kosherRecipes;
            }
            set
            {
                this.kosherRecipes = value;
                OnPropertyChanged();
            }

        }
        #endregion


        private async void MakeRecipesList()
        {
            List<Recipe> list = await RecipesService.GetAllRecipes();
            this.Recipes = new ObservableCollection<Recipe>(list);
            if (IsLogged != false)
            {
                List<Recipe> yourlist = this.Recipes.Where<Recipe>(r => r.MadeBy == ((App)Application.Current).LoggedInUser.Id).ToList();
                this.YourRecipes = new ObservableCollection<Recipe>(yourlist);
            }
            else
            {
                this.YourRecipes = new();
            }
            List<Recipe> NoGlotenList =  this.Recipes.Where<Recipe>(r => r.IsGloten == false).ToList();
            this.RecipesWithoutGloten =  new ObservableCollection<Recipe>(NoGlotenList);
            List<Recipe> NoLactoseList = this.Recipes.Where<Recipe>(r => r.ContainsDairy == false).ToList();
            this.RecipesWithoutLactose = new ObservableCollection<Recipe>(NoLactoseList);
            List<Recipe> MostPopular = new(this.Recipes);
            MostPopular = MostPopular.OrderByDescending(x => x.HowManyMadeIt).ToList();
            this.MostPopularRecipes = new ObservableCollection<Recipe>(MostPopular);
            List<Recipe> KosherList = this.Recipes.Where<Recipe>(r => r.IsKosher == true).ToList();
            this.KosherRecipes = new ObservableCollection<Recipe>(KosherList);
        }
        public HomePageViewModel(RecipesAppWebAPIProxy service, SignUpView signUp, LoginView login)
        {
            this.signupView = signUp;
            this.RecipesService = service;
            this.loginView = login;
            recipes = new ObservableCollection<Recipe>();
            SelectedRecipes = new ObservableCollection<Object>();
            this.LoginCommand = new Command(GoToLogin);
            this.SignUpCommand = new Command(GoToSignUp);
            MakeRecipesList();
            
            
        }

        private async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(signupView);

        }
        private async void GoToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(loginView);

        }
        
        public void Refresh2()
        {
            OnPropertyChanged("IsLogged");
        }
        #region Single Selection
        private Object selectedRecipe;
        public Object SelectedRecipe
        {
            get
            {
                return this.selectedRecipes;
            }
            set
            {
                this.selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public ICommand SingleSelectCommand => new Command(OnSingleSelectRecipe);

        async void OnSingleSelectRecipe()
        {
            if (SelectedRecipe != null)
            {
                var navParam = new Dictionary<string, object>()
                {
                    { "selectedRecipe",SelectedRecipe }
                };
                await Shell.Current.GoToAsync($"RecipeDetails", navParam);
                SelectedRecipe = null;
            }
        }


        #endregion
    }
}
