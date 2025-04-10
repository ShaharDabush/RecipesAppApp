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
using System.Xml.Linq;
using RecipesAppApp.Classes;
using Windows.ApplicationModel.VoiceCommands;


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(IsNotLogged),"IsNotLogged")]
    public class HomePageViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private User loggedUser;
        private ObservableCollection<Recipe> recipes;
        private ObservableCollection<Recipe> yourRecipes;
        private ObservableCollection<Recipe> recipesWithoutGloten;
        private ObservableCollection<Recipe> recipesWithoutLactose;
        private ObservableCollection<TopTenList> mostPopularRecipes;
        private ObservableCollection<Recipe> kosherRecipes;
        private ObservableCollection<Recipe> searchedBarList;
        private ObservableCollection<UserAllergyWithIsChecked> allergiesList = new ObservableCollection<UserAllergyWithIsChecked> ();
        private SignUpView signupView;
        private LoginView loginView;
        #region IsVisible properties
        private bool isGlutenVisible;
        private bool isLactoseVisible;
        private bool isKosherVisible;
        private bool isVegetarianVisible;
        private bool isVeganVisible;
        private bool inSearch;
        private bool notInSearch;
        private bool isAllergiesVisble;
        private bool isYourAllergiesVisble;
        public bool IsYourAllergiesVisble
        {
            get { return isYourAllergiesVisble; }
            set
            {
                isYourAllergiesVisble = value;
                OnPropertyChanged("IsYourAllergiesVisble");
            }
        }
        public bool IsAllergiesVisble
        {
            get { return isAllergiesVisble; }
            set
            {
                isAllergiesVisble = value;
                OnPropertyChanged("IsAllergiesVisble");
            }
        }
        public bool NotInSearch
        {
            get { return notInSearch; }
            set
            {
                notInSearch = value;
                OnPropertyChanged("NotInSearch");
            }
        }

        public bool IsGlutenVisible
        {
            get { return isGlutenVisible; }
            set
            {
                isGlutenVisible = value;
                OnPropertyChanged("IsGlutenVisible");
            }
        }

        public bool IsLactoseVisible
        {
            get { return isLactoseVisible; }
            set
            {
                isLactoseVisible = value;
                OnPropertyChanged("IsLactoseVisible");
            }
        }
        public bool IsKosherVisible
        {
            get { return isKosherVisible; }
            set
            {
                isKosherVisible = value;
                OnPropertyChanged("IsKosherVisible");
            }
        }
        public bool IsVegetarianVisible
        {
            get { return isVegetarianVisible; }
            set
            {
                isVegetarianVisible = value;
                OnPropertyChanged("IsVegetarianVisible");
            }
        }
        public bool IsVeganVisible
        {
            get { return isVeganVisible; }
            set
            {
                isVeganVisible = value;
                OnPropertyChanged("IsVeganVisible");
            }
        }
        public bool InSearch
        {
            get
            {
                return this.inSearch;
            }
            set
            {
                this.inSearch = value;
                OnPropertyChanged();
                if(InSearch == false)
                {
                    //SetAllergies();
                }
            }
        }
        #endregion
        private bool isLoggedSearch;
        private bool isYourAllergiesChecked;
        private String searchedName;

        public ICommand LoginCommand { get; set; }
        public ICommand ShowAllergiesCommand => new Command(ShowAllergies);
        public Command SignUpCommand { protected set; get; }
        public ICommand SortCommand => new Command(Sort);
        //on pressing the button to clear the sort
        public ICommand ClearSortCommand => new Command(ClearSort, () => !string.IsNullOrEmpty(SearchedName));

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
        public bool IsLoggedSearch
        {
            get { return ((App)Application.Current).LoggedInUser != null && !InSearch; }
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
        public ObservableCollection<TopTenList> MostPopularRecipes
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
        public ObservableCollection<Recipe> SearchedBarList
        {
            get
            {
                return this.searchedBarList;
            }
            set
            {
                this.searchedBarList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserAllergyWithIsChecked> AllergiesList
        {
            get
            {
                return this.allergiesList;
            }
            set
            {
                this.allergiesList = value;
                OnPropertyChanged();
            }
        }
        public String SearchedName
        {
            get
            {
                return this.searchedName;
            }
            set
            {
                this.searchedName = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public User LoggedUser
        {
            get
            {
                return loggedUser;
            }
            set
            {
                loggedUser = value;
                OnPropertyChanged();
                
            }
        }
        public bool IsYourAllergiesChecked
        {
            get { return isYourAllergiesChecked; }
            set
            {
                isYourAllergiesChecked = value;
                OnPropertyChanged("IsYourAllergiesChecked");
                AddYourAllergies();
            }
        }

        #endregion
        private async void MakeRecipesList()
        {
            List<Recipe> list = await RecipesService.GetAllRecipes();
            this.Recipes = new ObservableCollection<Recipe>(list);
            if (IsLoggedSearch != false)
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
            MostPopular = MostPopular.Take(10).OrderByDescending(x => x.HowManyMadeIt).ToList();
            List<TopTenList> l = new List<TopTenList>();
            for (int i = 0; i < MostPopular.Count; i++)
            {
                TopTenList t = new TopTenList(MostPopular[i].Id, MostPopular[i].RecipesName, MostPopular[i].RecipeImage, i+1);
                l.Add(t);
            }
            this.MostPopularRecipes = new ObservableCollection<TopTenList>(l);
            List<Recipe> KosherList = this.Recipes.Where<Recipe>(r => r.IsKosher == true).ToList();
            this.KosherRecipes = new ObservableCollection<Recipe>(KosherList);
            this.SearchedBarList = new ObservableCollection<Recipe>();
            this.InSearch = false;
            this.NotInSearch = true;
            List<Allergy> allAllergies = await RecipesService.GetAllAllergeis();
            foreach (Allergy a in allAllergies)
            {
                UserAllergyWithIsChecked u1 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, false);
                AllergiesList.Add(u1);
            }
        }
        public HomePageViewModel(RecipesAppWebAPIProxy service, SignUpView signUp, LoginView login)
        {
            this.signupView = signUp;
            this.RecipesService = service;
            this.loginView = login;
            this.InSearch = false;
            this.NotInSearch = true;
            this.IsAllergiesVisble = false;
            this.IsYourAllergiesChecked = false;
            //LoggedUser = ((App)Application.Current).LoggedInUser;
            recipes = new ObservableCollection<Recipe>();
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
            OnPropertyChanged("IsLoggedSearch");
        }

        //on SortCommand change the list and leave only the users that contain the given string
        public void Sort()
        {
            if(string.IsNullOrEmpty(SearchedName))
            {
                ClearSort();
            }
            else
            {
                List<Recipe> temp = Recipes.Where(r => r.RecipesName.ToLower().Contains(SearchedName.ToLower())).ToList();
                this.SearchedBarList.Clear();
                foreach (Recipe r in temp)
                {
                    this.SearchedBarList.Add(r);
                }
                InSearch = true;
                NotInSearch = false;
                //this.IsLogged = false;
            }
        }
        public void ClearSort()
        {
            if (!string.IsNullOrEmpty(SearchedName))
                this.SearchedName = null;
            this.SearchedBarList.Clear();
            InSearch = false;
            NotInSearch = true;
            //IsLogged = true;
        }


        #region Allergy
        public void ShowAllergies()
        {
            if (IsAllergiesVisble == false)
            {
                IsAllergiesVisble = true;
            }
            else 
            {
                IsAllergiesVisble = false;
            }
            if(IsAllergiesVisble == false && ((App)Application.Current).LoggedInUser != null)
            {
                loggedUser = ((App)Application.Current).LoggedInUser;
                IsYourAllergiesVisble = true ;
            }
            else if(IsYourAllergiesVisble == true)
            {
                IsAllergiesVisble = false;
            }
        }
        public void CheckAllergy()
        {

        }

        public async void AddYourAllergies()
        {
            List<Allergy> UsersAllergy = await RecipesService.GetAllergiesByUser(LoggedUser.Id);
            foreach (UserAllergyWithIsChecked a in AllergiesList)
            {
                foreach (Allergy au in UsersAllergy)
                {
                    if (a.AllergyId == au.Id)
                    {
                        a.IsChecked = true;
                    }
                }
            }
        }

        //public async void SetAllergies()
        //{
        //    List<Allergy> UsersAllergy = await RecipesService.GetAllergiesByUser(LoggedUser.Id);
        //    foreach (Allergy a in Allergies)
        //    {
        //        bool b = false;
        //        foreach (Allergy au in UsersAllergy)
        //        {
        //            if (a.Id == au.Id)
        //            {
        //                UserAllergyWithIsChecked u1 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, true);
        //                HasAllergy.Add(u1);
        //                b = true;
        //            }
        //        }
        //        UserAllergyWithIsChecked u2 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, false);
        //        if (!b)
        //            HasAllergy.Add(u2);
        //    }
        //}
        #endregion

        #region Single Selection

        private Recipe selectedRecipe;
        public Recipe SelectedRecipe
        {
            get
            {
                return this.selectedRecipe;
            }
            set
            {
                if (this.selectedRecipe != value)
                {
                    this.selectedRecipe = value;
                    
                    if (SelectedRecipe != null)
                        OnSingleSelectRecipe();
                    OnPropertyChanged();
                }
                
            }
        }
        private TopTenList selectedTopTenRecipe;
        public TopTenList SelectedTopTenRecipe
        {
            get
            {
                return this.selectedTopTenRecipe;
            }
            set
            {
                if (this.selectedTopTenRecipe != value)
                {
                    this.selectedTopTenRecipe = value;
                    
                    if (selectedTopTenRecipe != null)
                        OnSingleSelectTopTenRecipe();
                    OnPropertyChanged();
                }
                
            }
        }

        async void OnSingleSelectRecipe()
        {
                var navParam = new Dictionary<string, object>()
                {
                    { "Recipe",SelectedRecipe }
                };
                await Shell.Current.GoToAsync("RecipeDetails", navParam);
                SelectedRecipe = null;
        }
        async void OnSingleSelectTopTenRecipe()
        {
            Recipe newRecipe = Recipes.Where(r => r.Id == SelectedTopTenRecipe.Id).FirstOrDefault();
            var navParam = new Dictionary<string, object>()
                {
                    { "Recipe",newRecipe }
                };
            await Shell.Current.GoToAsync("RecipeDetails", navParam);
            SelectedTopTenRecipe = null;
        }


        #endregion
    }
}
