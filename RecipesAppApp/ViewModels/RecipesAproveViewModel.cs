using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using System.Windows.Input;
using RecipesAppApp.Classes;


namespace RecipesAppApp.ViewModels
{
    public class RecipesAproveViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private bool isRefreshing;
        public ICommand RefreshCommand => new Command(Refresh);
        private ObservableCollection<Recipe> recipeList;
        private ObservableCollection<User> userList;
        private ObservableCollection<RecipeWithUserName> searchedRecipes;
        private ObservableCollection<RecipeWithUserName> recipesForList;
        private string searchedName;

        public ObservableCollection<RecipeWithUserName> RecipesForList
        {
            get { return recipesForList; }
            set
            {
                this.recipesForList = value;
                OnPropertyChanged();
            }
        }
        public string SearchedName
        {
            get { return searchedName; }
            set
            {
                this.searchedName = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public ObservableCollection<RecipeWithUserName> SearchedRecipes
        {
            get { return searchedRecipes; }
            set
            {
                this.searchedRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set
            {
                this.userList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Recipe> RecipeList
        {
            get { return recipeList; }
            set
            {
                this.recipeList = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public RecipesAproveViewModel(RecipesAppWebAPIProxy service)
        {
            RecipesService = service;
            GetRecipes();
        }

        public async void GetRecipes()
        {
            List<Recipe> r = await RecipesService.GetAllRecipes();
            this.RecipeList = new ObservableCollection<Recipe>(r);
            List<User> u = await RecipesService.GetAllUsers();
            this.UserList = new ObservableCollection<User>(u);
            RecipesForList = new ObservableCollection<RecipeWithUserName>();
            for(int i = 0; i < RecipeList.Count; i++)
            {
                for(int j = 0; j < UserList.Count; j++)
                {
                    if (RecipeList[i].MadeBy == UserList[j].Id)
                    {
                        Recipe recipe = RecipeList[i];
                        User user = UserList[j];
                        RecipesForList.Add(new RecipeWithUserName(recipe.Id, recipe.RecipesName, recipe.RecipeDescription, recipe.RecipeImage, recipe.MadeBy, recipe.Rating, recipe.HowManyMadeIt, user.UserName));
                    }
                }
            }
            this.SearchedRecipes = new ObservableCollection<RecipeWithUserName>(RecipesForList);

        }
        //
        public void Sort()
        {
            if (string.IsNullOrEmpty(SearchedName))
            {
                ClearSort();
            }
           else 
           {
               List<RecipeWithUserName> temp = RecipesForList.Where(r => r.RecipesName.ToLower().Contains(SearchedName.ToLower())).ToList();
               this.SearchedRecipes.Clear();
               foreach (RecipeWithUserName r in temp)
               {
                   this.SearchedRecipes.Add(r);
               }
               //this.IsLogged = false;
           }
        }
        public void ClearSort()
        {
            if (!string.IsNullOrEmpty(SearchedName))
                this.SearchedName = null;
            GetRecipes();
            this.SearchedRecipes = RecipesForList;
        }

        public override void Refresh()
        {
            GetRecipes();
            OnPropertyChanged("SearchedRecipes");
        }
    }
}
