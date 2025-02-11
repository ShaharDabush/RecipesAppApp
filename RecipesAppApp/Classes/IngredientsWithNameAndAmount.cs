using Android.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Classes
{
    public class IngredientsWithNameAndAmount
    {
        public int IngredientId { get; set; }

        public int RecipeId { get; set; }

        public int Amount { get; set; }

        public string MeasureUnits { get; set; } = null!;

        public string Name { get; set; } = null!;
        public IngredientsWithNameAndAmount() { }
        public IngredientsWithNameAndAmount(int IngredientId, int RecipeId, int Amount, string MeasureUnits, string Name) 
        {
            this.IngredientId = IngredientId;
            this.RecipeId = RecipeId;
            this.Amount = Amount;
            this.MeasureUnits = MeasureUnits;
            this.Name = Name;
        }
    }
}
