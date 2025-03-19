using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Models
{
    public class SaveRecipeInfo
    {
        public Recipe RecipeInfo { get; set; }
        public List<Level> LevelsInfo { get; set; }
        public List<IngredientRecipe> IngredientsInfo { get; set; }
    }
}
