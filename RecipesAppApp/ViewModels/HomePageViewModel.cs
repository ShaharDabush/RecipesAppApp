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
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel.Design;
//using Windows.ApplicationModel.VoiceCommands;


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(IsNotLogged), "IsNotLogged")]
    public class HomePageViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private User loggedUser;
        private ObservableCollection<Recipe> recipes;
        private ObservableCollection<Recipe> yourRecipes;
        private ObservableCollection<Recipe> recipesYouCanMake;
        private ObservableCollection<Recipe> deserts;
        private ObservableCollection<Recipe> japaneseRecipes;
        private ObservableCollection<Recipe> frenchRecipes;
        private ObservableCollection<Recipe> italianRecipes;
        private ObservableCollection<Recipe> breakfastRecipes;
        private ObservableCollection<Recipe> lunchRecipes;
        private ObservableCollection<Recipe> dinnerRecipes;
        private ObservableCollection<TopTenList> mostPopularRecipes;
        private ObservableCollection<TopTenList> topRatedRecipes;
        private ObservableCollection<Recipe> kosherRecipes;
        private ObservableCollection<Recipe> searchedBarList;
        private ObservableCollection<UserAllergyWithIsChecked> allergiesList = new ObservableCollection<UserAllergyWithIsChecked>();
        private SignUpView signupView;
        private LoginView loginView;
        #region IsVisible properties
        private bool isKosherVisible;
        private bool inSearch;
        private bool notInSearch;
        private bool isAllergiesVisble;
        private bool isYourAllergiesVisble;
        private int flipPicker;
        public int FlipPicker
        {
            get { return flipPicker; }
            set
            {
                flipPicker = value;
                OnPropertyChanged("FlipPicker");
            }
        }

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

        public bool IsKosherVisible
        {
            get { return isKosherVisible; }
            set
            {
                isKosherVisible = value;
                OnPropertyChanged("IsKosherVisible");
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
                if (InSearch == true)
                {
                    IsKosherVisible = false;
                }
                else if (((App)Application.Current).LoggedInUser == null || (((App)Application.Current).LoggedInUser != null && ((App)Application.Current).LoggedInUser.IsKohser == false))
                {
                    IsKosherVisible = true;
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
            //get { return ((App)Application.Current).LoggedInUser != null && !InSearch; }
            get
            {
                return isLoggedSearch;
            }
            set
            {
                isLoggedSearch = value;
                OnPropertyChanged();
                isLoggedSearch = ((App)Application.Current).LoggedInUser != null && !InSearch;
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
        public ObservableCollection<Recipe> RecipesYouCanMake
        {
            get
            {
                return this.recipesYouCanMake;
            }
            set
            {
                this.recipesYouCanMake = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> Deserts
        {
            get
            {
                return this.deserts;
            }
            set
            {
                this.deserts = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> JapaneseRecipes
        {
            get
            {
                return this.japaneseRecipes;
            }
            set
            {
                this.japaneseRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> FrenchRecipes
        {
            get
            {
                return this.frenchRecipes;
            }
            set
            {
                this.frenchRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> ItalianRecipes
        {
            get
            {
                return this.italianRecipes;
            }
            set
            {
                this.italianRecipes = value;
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
        public ObservableCollection<TopTenList> TopRatedRecipes
        {
            get
            {
                return this.topRatedRecipes;
            }
            set
            {
                this.topRatedRecipes = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<Recipe> BreakfastRecipes
        {
            get
            {
                return this.breakfastRecipes;
            }
            set
            {
                this.breakfastRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> LunchRecipes
        {
            get
            {
                return this.lunchRecipes;
            }
            set
            {
                this.lunchRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> DinnerRecipes
        {
            get
            {
                return this.dinnerRecipes;
            }
            set
            {
                this.dinnerRecipes = value;
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
                if (IsYourAllergiesChecked == true)
                    AddYourAllergies();
                else
                    RemoveYourAllergies();

            }
        }

        #endregion
        private async Task MakeRecipesList()
        {
            List<Recipe> list = await RecipesService.GetAllRecipes();
            this.Recipes = new ObservableCollection<Recipe>(list);
            if (IsLoggedSearch != false)
            {
                List<Recipe> yourlist = this.Recipes.Where<Recipe>(r => r.MadeBy == ((App)Application.Current).LoggedInUser.Id).ToList();
                this.YourRecipes = new ObservableCollection<Recipe>(yourlist);
                if(((App)Application.Current).LoggedInUser.StorageId != null)
                {
                List<Ingredient> UserIngredient = await RecipesService.GetIngredientsByStorage(((App)Application.Current).LoggedInUser.StorageId.Value);
                List<Recipe> recipestoucanmakelist = this.Recipes.Where<Recipe>(r => r.MadeBy == ((App)Application.Current).LoggedInUser.Id).ToList();
                this.RecipesYouCanMake = new ObservableCollection<Recipe>(recipestoucanmakelist);
                }
            }
            else
            {
                this.YourRecipes = new();
            }
            List<Recipe> DesertList = this.Recipes.Where<Recipe>(r => r.Type == "Desert").ToList();
            this.Deserts = new ObservableCollection<Recipe>(DesertList);
            List<Recipe> JapaneseList = this.Recipes.Where<Recipe>(r => r.Type == "Japanese").ToList();
            this.JapaneseRecipes = new ObservableCollection<Recipe>(JapaneseList);
            List<Recipe> FrenchList = this.Recipes.Where<Recipe>(r => r.Type == "French").ToList();
            this.FrenchRecipes = new ObservableCollection<Recipe>(FrenchList);
            List<Recipe> ItalianList = this.Recipes.Where<Recipe>(r => r.Type == "Italian").ToList();
            this.ItalianRecipes = new ObservableCollection<Recipe>(ItalianList);
            List<Recipe> BreakfastList = this.Recipes.Where<Recipe>(r => r.TimeOfDay == "Morning").ToList();
            this.BreakfastRecipes = new ObservableCollection<Recipe>(BreakfastList);
            List<Recipe> LunchList = this.Recipes.Where<Recipe>(r => r.TimeOfDay == "Noon").ToList();
            this.LunchRecipes = new ObservableCollection<Recipe>(LunchList);
            List<Recipe> DinnerList = this.Recipes.Where<Recipe>(r => r.TimeOfDay == "Evening").ToList();
            this.DinnerRecipes = new ObservableCollection<Recipe>(DinnerList);
            List<Recipe> MostPopular = new(this.Recipes);
            MostPopular = MostPopular.Take(10).OrderByDescending(x => x.HowManyMadeIt).ToList();
            List<TopTenList> l1 = new List<TopTenList>();
            for (int i = 0; i < MostPopular.Count; i++)
            {
                TopTenList t = new TopTenList(MostPopular[i].Id, MostPopular[i].RecipesName, MostPopular[i].RecipeImage, i + 1);
                l1.Add(t);
            }
            this.MostPopularRecipes = new ObservableCollection<TopTenList>(l1);
            List<Recipe> MostRated = new(this.Recipes);
            MostRated = MostRated.Take(10).OrderByDescending(x => x.Rating).ToList();
            List<TopTenList> l2 = new List<TopTenList>();
            for (int i = 0; i < MostRated.Count; i++)
            {
                TopTenList t = new TopTenList(MostRated[i].Id, MostRated[i].RecipesName, MostRated[i].RecipeImage, i + 1);
                l2.Add(t);
            }
            this.TopRatedRecipes = new ObservableCollection<TopTenList>(l2);
            List<Recipe> KosherList = this.Recipes.Where<Recipe>(r => r.IsKosher == true).ToList();
            this.KosherRecipes = new ObservableCollection<Recipe>(KosherList);
            this.SearchedBarList = new ObservableCollection<Recipe>();
            this.InSearch = false;
            this.NotInSearch = true;
            if (AllergiesList.Count == 0)
            {
                List<Allergy> allAllergies = await RecipesService.GetAllAllergeis();
                foreach (Allergy a in allAllergies)
                {
                    UserAllergyWithIsChecked u1 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, false);
                    AllergiesList.Add(u1);
                }
            }

        }
        public HomePageViewModel(RecipesAppWebAPIProxy service, SignUpView signUp, LoginView login)
        {
            this.signupView = signUp;
            this.RecipesService = service;
            this.loginView = login;
            this.InSearch = false;
            this.NotInSearch = true;
            this.IsKosherVisible = true;
            this.IsAllergiesVisble = false;
            this.isYourAllergiesVisble = false;
            this.IsLoggedSearch = false;
            this.FlipPicker = 0;
            OnPropertyChanged("IsLoggedSearch");
            //LoggedUser = ((App)Application.Current).LoggedInUser;
            recipes = new ObservableCollection<Recipe>();
            this.LoginCommand = new Command(GoToLogin);
            this.SignUpCommand = new Command(GoToSignUp);
            MakeRecipesList();
            if(((App)Application.Current).LoggedInUser != null && ((App)Application.Current).LoggedInUser.IsKohser == true)
            {
                FilterKosherRecipes();
            }
            if (((App)Application.Current).LoggedInUser != null && ((App)Application.Current).LoggedInUser.Vegetarianism == "Vegetarian")
            {
                FilterVegetarianRecipes();
            }
        }


        private async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(signupView);

        }
        private async void GoToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(loginView);

        }

        public override async void Refresh()
        {
            this.IsLoggedSearch = false;
            await MakeRecipesList();
            if(((App)Application.Current).LoggedInUser.Vegetarianism == "Vegetarian")
            {
                FilterVegetarianRecipes();
            }
            if(((App)Application.Current).LoggedInUser.IsKohser == true)
            {
                FilterKosherRecipes();
            }
        }

        //on SortCommand change the list and leave only the users that contain the given string
        public void Sort()
        {
            if (string.IsNullOrEmpty(SearchedName))
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

        public async void FilterKosherRecipes()
        {
            #region Search List
            List<Recipe> sbl = new List<Recipe>(Recipes);
            foreach (Recipe r in Recipes)
            {
                if (r.IsKosher == false)
                {
                    sbl.Remove(r);
                }
            }
            Recipes = new ObservableCollection<Recipe>(sbl);
            OnPropertyChanged("Recipes");
            #endregion
            #region Deserts
            List<Recipe> D = new List<Recipe>(Deserts);
            foreach (Recipe r in Deserts)
            {
                if (r.IsKosher == false)
                {
                    D.Remove(r);
                }
            }
            Deserts = new ObservableCollection<Recipe>(D);
            OnPropertyChanged("Deserts");
            #endregion
            #region Japanese Recipes
            List<Recipe> jr = new List<Recipe>(JapaneseRecipes);
            foreach (Recipe r in JapaneseRecipes)
            {
                if (r.IsKosher == false)
                {
                    jr.Remove(r);
                }
            }
            JapaneseRecipes = new ObservableCollection<Recipe>(jr);
            OnPropertyChanged("JapaneseRecipes");
            #endregion
            #region French Recipes
            List<Recipe> fr = new List<Recipe>(FrenchRecipes);
            foreach (Recipe r in FrenchRecipes)
            {
                if (r.IsKosher == false)
                {
                    fr.Remove(r);
                }
            }
            FrenchRecipes = new ObservableCollection<Recipe>(fr);
            OnPropertyChanged("FrenchRecipes");
            #endregion
            #region Italian Recipes
            List<Recipe> ir = new List<Recipe>(ItalianRecipes);
            foreach (Recipe r in ItalianRecipes)
            {
                if (r.IsKosher == false)
                {
                    ir.Remove(r);
                }
            }
            ItalianRecipes = new ObservableCollection<Recipe>(ir);
            OnPropertyChanged("ItalianRecipes");
            #endregion
            #region Kosher Recipes
            List<Recipe> kr = new List<Recipe>(KosherRecipes);
            foreach (Recipe r in KosherRecipes)
            {
                if (r.IsKosher == false)
                {
                    kr.Remove(r);
                }
            }
            KosherRecipes = new ObservableCollection<Recipe>(kr);
            OnPropertyChanged("KosherRecipes");
            #endregion
            #region Breakfast Recipes
            List<Recipe> br = new List<Recipe>(BreakfastRecipes);
            foreach (Recipe r in BreakfastRecipes)
            {
                if (r.IsKosher == false)
                {
                    br.Remove(r);
                }
            }
            BreakfastRecipes = new ObservableCollection<Recipe>(br);
            OnPropertyChanged("BreakfastRecipes");
            #endregion
            #region Lunch Recipes
            List<Recipe> lr = new List<Recipe>(LunchRecipes);
            foreach (Recipe r in LunchRecipes)
            {
                if (r.IsKosher == false)
                {
                    lr.Remove(r);
                }
            }
            LunchRecipes = new ObservableCollection<Recipe>(lr);
            OnPropertyChanged("LunchRecipes");
            #endregion
            #region Dinner Recipes
            List<Recipe> dr = new List<Recipe>(DinnerRecipes);
            foreach (Recipe r in DinnerRecipes)
            {
                if (r.IsKosher == false)
                {
                    dr.Remove(r);
                }
            }
            DinnerRecipes = new ObservableCollection<Recipe>(dr);
            OnPropertyChanged("DinnerRecipes");
            #endregion
            IsKosherVisible = false;
        }
        public async void FilterVegetarianRecipes()
        {
            #region Search List
            List<Recipe> sbl = new List<Recipe>(Recipes);
            foreach (Recipe r in Recipes)
            {
                if (r.ContainsMeat == true)
                {
                    sbl.Remove(r);
                }
            }
            Recipes = new ObservableCollection<Recipe>(sbl);
            OnPropertyChanged("Recipes");
            #endregion
            #region Deserts
            List<Recipe> D = new List<Recipe>(Deserts);
            foreach (Recipe r in Deserts)
            {
                if (r.ContainsMeat == true)
                {
                    D.Remove(r);
                }
            }
            Deserts = new ObservableCollection<Recipe>(D);
            OnPropertyChanged("Deserts");
            #endregion
            #region Japanese Recipes
            List<Recipe> jr = new List<Recipe>(JapaneseRecipes);
            foreach (Recipe r in JapaneseRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    jr.Remove(r);
                }
            }
            JapaneseRecipes = new ObservableCollection<Recipe>(jr);
            OnPropertyChanged("JapaneseRecipes");
            #endregion
            #region French Recipes
            List<Recipe> fr = new List<Recipe>(FrenchRecipes);
            foreach (Recipe r in FrenchRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    fr.Remove(r);
                }
            }
            FrenchRecipes = new ObservableCollection<Recipe>(fr);
            OnPropertyChanged("FrenchRecipes");
            #endregion
            #region Italian Recipes
            List<Recipe> ir = new List<Recipe>(ItalianRecipes);
            foreach (Recipe r in ItalianRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    ir.Remove(r);
                }
            }
            ItalianRecipes = new ObservableCollection<Recipe>(ir);
            OnPropertyChanged("ItalianRecipes");
            #endregion
            #region Kosher Recipes
            List<Recipe> kr = new List<Recipe>(KosherRecipes);
            foreach (Recipe r in KosherRecipes)
            {
                if (r.ContainsMeat == false)
                {
                    kr.Remove(r);
                }
            }
            Recipes = new ObservableCollection<Recipe>(kr);
            OnPropertyChanged("KosherRecipes");
            #endregion
            #region Breakfast Recipes
            List<Recipe> br = new List<Recipe>(BreakfastRecipes);
            foreach (Recipe r in BreakfastRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    br.Remove(r);
                }
            }
            BreakfastRecipes = new ObservableCollection<Recipe>(br);
            OnPropertyChanged("BreakfastRecipes");
            #endregion
            #region Lunch Recipes
            List<Recipe> lr = new List<Recipe>(LunchRecipes);
            foreach (Recipe r in LunchRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    lr.Remove(r);
                }
            }
            LunchRecipes = new ObservableCollection<Recipe>(lr);
            OnPropertyChanged("LunchRecipes");
            #endregion
            #region Dinner Recipes
            List<Recipe> dr = new List<Recipe>(DinnerRecipes);
            foreach (Recipe r in DinnerRecipes)
            {
                if (r.ContainsMeat == true)
                {
                    dr.Remove(r);
                }
            }
            DinnerRecipes = new ObservableCollection<Recipe>(dr);
            OnPropertyChanged("DinnerRecipes");
            #endregion
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
                if (IsAllergiesVisble == false && ((App)Application.Current).LoggedInUser != null)
                {
                    LoggedUser = ((App)Application.Current).LoggedInUser;
                    IsYourAllergiesVisble = true;
                }
                else if (IsAllergiesVisble == false)
                {
                    IsYourAllergiesVisble = false;
                }
                FlipPicker += 180;
                FlipPicker = FlipPicker % 360;
            }

            public async void AddYourAllergies()
            {
                List<Allergy> UsersAllergy = await RecipesService.GetAllergiesByUser(LoggedUser.Id);
                List<UserAllergyWithIsChecked> aL = new(AllergiesList);
                foreach (UserAllergyWithIsChecked a in aL)
                {
                    foreach (Allergy au in UsersAllergy)
                    {
                        if (a.AllergyId == au.Id)
                        {
                            a.IsChecked = true;
                        }
                    }
                }
                AllergiesList = new(aL);
                FilterRecipes();
            }
            public async void RemoveYourAllergies()
            {
                List<Allergy> UsersAllergy = await RecipesService.GetAllergiesByUser(LoggedUser.Id);
                List<UserAllergyWithIsChecked> aL = new(AllergiesList);
                foreach (UserAllergyWithIsChecked a in aL)
                {
                    foreach (Allergy au in UsersAllergy)
                    {
                        if (a.AllergyId == au.Id)
                        {
                            a.IsChecked = false;
                        }
                    }
                }
                AllergiesList = new(aL);
            }
            public async void FilterRecipes()
            {
                await MakeRecipesList();
                foreach (UserAllergyWithIsChecked a in AllergiesList)
                {
                    if (a.IsChecked)
                    {
                        bool IsAllergy;
                        #region Search List
                        List<Recipe> sbl = new List<Recipe>();
                        foreach (Recipe r in Recipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                sbl.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                sbl.Add(r);
                            }
                        }
                        Recipes = new ObservableCollection<Recipe>(sbl);
                        OnPropertyChanged("Recipes");
                        #endregion
                        #region Deserts
                        List<Recipe> D = new List<Recipe>();
                        foreach (Recipe r in Deserts)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                D.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                D.Add(r);
                            }
                        }
                        Deserts = new ObservableCollection<Recipe>(D);
                        OnPropertyChanged("Deserts");
                        #endregion
                        #region Japanese Recipes
                        List<Recipe> jr = new List<Recipe>();
                        foreach (Recipe r in JapaneseRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                jr.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                jr.Add(r);
                            }
                        }
                        JapaneseRecipes = new ObservableCollection<Recipe>(jr);
                        OnPropertyChanged("JapaneseRecipes");
                        #endregion
                        #region French Recipes
                        List<Recipe> fr = new List<Recipe>();
                        foreach (Recipe r in FrenchRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                fr.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                fr.Add(r);
                            }
                        }
                        FrenchRecipes = new ObservableCollection<Recipe>(fr);
                        OnPropertyChanged("FrenchRecipes");
                        #endregion
                        #region Italian Recipes
                        List<Recipe> ir = new List<Recipe>();
                        foreach (Recipe r in ItalianRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                ir.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                ir.Add(r);
                            }
                        }
                        ItalianRecipes = new ObservableCollection<Recipe>(ir);
                        OnPropertyChanged("ItalianRecipes");
                        #endregion
                        #region Kosher Recipes
                        List<Recipe> kr = new List<Recipe>();
                        foreach (Recipe r in KosherRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                kr.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                kr.Add(r);
                            }
                        }
                        KosherRecipes = new ObservableCollection<Recipe>(kr);
                        OnPropertyChanged("KosherRecipes");
                        #endregion
                        #region Breakfast Recipes
                        List<Recipe> br = new List<Recipe>();
                        foreach (Recipe r in BreakfastRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                br.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                br.Add(r);
                            }
                        }
                        BreakfastRecipes = new ObservableCollection<Recipe>(br);
                        OnPropertyChanged("BreakfastRecipes");
                        #endregion
                        #region Lunch Recipes
                        List<Recipe> lr = new List<Recipe>();
                        foreach (Recipe r in LunchRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                lr.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                lr.Add(r);
                            }
                        }
                        LunchRecipes = new ObservableCollection<Recipe>(lr);
                        OnPropertyChanged("LunchRecipes");
                        #endregion
                        #region Dinner Recipes
                        List<Recipe> dr = new List<Recipe>();
                        foreach (Recipe r in DinnerRecipes)
                        {
                            IsAllergy = false;
                            if (r.Allergies.Count == 0)
                            {
                                dr.Add(r);
                            }
                            else
                            {
                                foreach (Allergy ar in r.Allergies)
                                {
                                    if (a.AllergyId == ar.Id)
                                    {
                                        IsAllergy = true;
                                    }
                                }
                            }
                            if (IsAllergy == false)
                            {
                                dr.Add(r);
                            }
                        }
                        DinnerRecipes = new ObservableCollection<Recipe>(dr);
                        OnPropertyChanged("DinnerRecipes");
                        #endregion
                    }
                }

            }

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
