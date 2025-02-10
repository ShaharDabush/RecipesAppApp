
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using System.Collections.ObjectModel;


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(Recipe), "Recipe")]
    public class RecipeDetailsViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        public ObservableCollection<Level> levels;
        public ObservableCollection<Ingredient> ingredients;
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
            List<Level> Levellist = await RecipesService.GetLevelsByRecipe(Recipe.Id);
            this.Levels = new ObservableCollection<Level>(Levellist);
            OnPropertyChanged("Levels");
            OnPropertyChanged("Ingredients");
        } 
        public RecipeDetailsViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;

        }
    }
}
