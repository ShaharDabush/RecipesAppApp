
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


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(Recipe), "Recipe")]
    public class RecipeDetailsViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        public ObservableCollection<Level> levels;
        public ObservableCollection<IngredientRecipe> ingredientRecipes;
        public ObservableCollection<Ingredient> ingredients;
        public ObservableCollection<IngredientsWithNameAndAmount> trueList;
        private Recipe recipe;
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
            this.TrueList = new ObservableCollection<IngredientsWithNameAndAmount>(TrueListA);
            OnPropertyChanged("Levels");
            OnPropertyChanged("Ingredients");
        } 
        public RecipeDetailsViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;

        }
    }
}
