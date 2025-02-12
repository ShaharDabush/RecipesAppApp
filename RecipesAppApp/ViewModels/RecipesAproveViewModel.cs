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
        private ObservableCollection<RecipeWithUserName> allRecipes;
        private ObservableCollection<RecipeWithUserName> searchedBarList;
        private string searchedName;

        public ObservableCollection<RecipeWithUserName> SearchedBarList
        {
            get { return searchedBarList; }
            set
            {
                this.searchedBarList = value;
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
        public ObservableCollection<RecipeWithUserName> AllRecipes
        {
            get { return allRecipes; }
            set
            {
                this.allRecipes = value;
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
            List<RecipeWithUserName> RecipesForList = new List<RecipeWithUserName>();
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
            this.AllRecipes = new ObservableCollection<RecipeWithUserName>(RecipesForList);

        }
        //
        public void Sort()
        {
            if (!string.IsNullOrEmpty(SearchedName))
            {
                List<RecipeWithUserName> temp = AllRecipes.Where(r => r.RecipesName.ToLower().Contains(SearchedName.ToLower())).ToList();
                this.AllRecipes.Clear();
                foreach (RecipeWithUserName r in temp)
                {
                    this.AllRecipes.Add(r);
                }
                //this.IsLogged = false;
            }
            else
            {
                GetRecipes();
            }
        }

        private async void Refresh()
        {
            GetRecipes();
        }
    }
}
