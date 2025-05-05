
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(Recipe), "Recipe")]
    public partial class RecipeDetailsViewModel : ViewModelBase
    {
        #region attributes and properties
        public ICommand RemoveIngredientsCommand => new Command(RemoveIngredients);
        private RecipesAppWebAPIProxy RecipesService;
        public event Action<List<string>> OpenPopup;
        public ObservableCollection<Level> levels;
        public ObservableCollection<IngredientRecipe> ingredientRecipes;
        public ObservableCollection<Ingredient> ingredients;
        public ObservableCollection<IngredientsWithNameAndAmount> trueList;
        public User loggedUser;
        private Recipe recipe;
        private List<int> ratings;
        private Rating userRating;
        private double recipeRating;
        private double? rate;
        private bool isRemoveIngredientVisible;

        public bool IsRemoveIngredientVisible
        {
            get { return isRemoveIngredientVisible; }
            set
            {
                this.isRemoveIngredientVisible = value;
                OnPropertyChanged();
            }
        }
        public List<int> Ratings
        {
            get { return ratings; }
            set
            {
                this.ratings = value;
                OnPropertyChanged();
            }
        }
        public double? Rate
        {
            get { return rate; }
            set
            {
                this.rate = value;
               OnPropertyChanged();
                if (Rate != null)
               SaveUserRating();
            }
        }
        
        public Rating UserRating
        {
            get { return userRating; }
            set
            {
                this.userRating = value;
                OnPropertyChanged();
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
        public double RecipeRating
        {
            get { return recipeRating; }
            set
            {
                this.recipeRating = value;
                OnPropertyChanged();
            }
        }
        public Recipe Recipe
        {
            get { return recipe; }
            set
            {
                this.recipe = value;
                OnPropertyChanged();
                MakeLists();
            }
        }
        public ObservableCollection<Level> Levels
        {
            get { return levels; }
            set
            {
                this.levels = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IngredientsWithNameAndAmount> TrueList
        {
            get { return trueList; }
            set
            {
                this.trueList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IngredientRecipe> IngredientRecipes
        {
            get { return ingredientRecipes; }
            set
            {
                this.ingredientRecipes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> Ingredients
        {
            get { return ingredients; }
            set
            {
                this.ingredients = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public async void MakeLists()
        {
            List<Ingredient> IngredientList = await RecipesService.GetIngredientsByRecipe(Recipe.Id);
            this.Ingredients = new ObservableCollection<Ingredient>(IngredientList);
            List<IngredientRecipe> IngredientRecipeList = await RecipesService.GetIngredientRecipesByRecipe(Recipe.Id);
            this.IngredientRecipes = new ObservableCollection<IngredientRecipe>(IngredientRecipeList);
            List<Level> Levellist = await RecipesService.GetLevelsByRecipe(Recipe.Id);
            this.Levels = new ObservableCollection<Level>(Levellist);
            List<IngredientsWithNameAndAmount> TrueListA = new List<IngredientsWithNameAndAmount>();
            for(int i = 0;i < IngredientList.Count; i++) 
            {
                TrueListA.Add(new IngredientsWithNameAndAmount(this.IngredientRecipes[i].IngredientId, this.IngredientRecipes[i].RecipeId, this.IngredientRecipes[i].Amount, this.IngredientRecipes[i].MeasureUnits, this.ingredients[i].IngredientName,false));
            }
            if(((App)Application.Current).LoggedInUser != null)
            {
                Storage s = await RecipesService.GetStoragesbyUser(((App)Application.Current).LoggedInUser.Id);
                List<Ingredient> UserIngredient = await RecipesService.GetIngredientsByStorage(s.Id);
                if(UserIngredient != null)
                {
                foreach(Ingredient i in UserIngredient)
                {
                    foreach(IngredientsWithNameAndAmount iwna in TrueListA)
                    {
                        if(i.Id == iwna.IngredientId)
                        {
                            iwna.IsChecked = true;
                        }
                    }
                }   
                }

            }
            RecipeRating = 0;
            RecipeRating = Recipe.Rating;
            this.TrueList = new ObservableCollection<IngredientsWithNameAndAmount>(TrueListA);
            OnPropertyChanged("Levels");
            OnPropertyChanged("Ingredients");
        } 
        public RecipeDetailsViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            if(((App)Application.Current).LoggedInUser != null)
            {
                IsRemoveIngredientVisible = true;
                LoggedUser = ((App)Application.Current).LoggedInUser;
            }
            else
            {
                IsRemoveIngredientVisible = false;
                LoggedUser = null;
            }
            Ratings = new List<int>();
            Ratings.Add(1);
            Ratings.Add(2);
            Ratings.Add(3);
            Ratings.Add(4);
            Ratings.Add(5);
            Ratings.Add(6);
            Ratings.Add(7);
            Ratings.Add(8);
            Ratings.Add(9);
            Ratings.Add(10);

        }
        public async void SaveUserRating()
        {

            if (((App)Application.Current).LoggedInUser != null)
            {
                UserRating = new Rating();
                UserRating.UserId = ((App)Application.Current).LoggedInUser.Id;
                UserRating.Rate = Rate.Value;
                UserRating.RecipeId = Recipe.Id;
                bool result = await RecipesService.SaveRating(UserRating);
                if(result != true)
                {
                    Rate = null;
                    OnPropertyChanged("Rate");
                    await Application.Current.MainPage.DisplayAlert("Rating", "Someting went wrong try again later!", "ok");
                }
                Double? NewRate = await RecipesService.GetRatingbyRecipe(Recipe.Id);
                RecipeRating = NewRate.Value;
                OnPropertyChanged("Recipe");
                OnPropertyChanged("RecipeRating");
            }
            else if (Rate != 0)
            {
                await Application.Current.MainPage.DisplayAlert("Rating", "You can not rate a recipe without an account Login Or Sigh Up and try again!", "ok");
            }
        } 

        public void RemoveIngredients()
        {
            if (OpenPopup != null)
            {
                List<string> l = new List<string>();
                OpenPopup(l);
            }
            MakeListsForRemoveIngredients();
        }
    }
}
