
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
        private ObservableCollection<Level> Levels;
        private ObservableCollection<LevelList> LevelList;
        private ObservableCollection<Ingredient> Ingredients;
        private Recipe recipe;
        public Recipe Recipe
        {
            get { return recipe; }
            set
            {
                this.recipe = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public async void MakeLists()
        {
            List<Ingredient> IngredientList = await RecipesService.GetIngredientsByRecipe(recipe.Id);
            this.Ingredients = new ObservableCollection<Ingredient>(IngredientList);
            List<Level> Levellist = await RecipesService.GetLevelsByRecipe(recipe.Id);
            this.Levels = new ObservableCollection<Level>(Levellist);
            List<LevelList> l = new List<LevelList>();
            for (int i = 0; i < Levels.Count; i++)
            {
                LevelList t = new LevelList(Levels[i].TextLevel, i + 1);
                l.Add(t);
            }
            this.LevelList = new ObservableCollection<LevelList>(l);
        } 
        public RecipeDetailsViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            MakeLists();
        }

        //private String name;
        //public String Name
        //{
        //    get { return name; }
        //    set
        //    {
        //        this.name = value;
        //        OnPropertyChanged();
        //    }
        //}
        public RecipeDetailsViewModel()
        {
            //Name = SelectedRecipe.RecipesName;
        }
    }
}
