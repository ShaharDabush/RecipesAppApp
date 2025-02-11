﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Models
{
    public class IngredientRecipe
    {
        public int IngredientId { get; set; }

        public int RecipeId { get; set; }

        public int Amount { get; set; }

        public string MeasureUnits { get; set; } = null!;
        public IngredientRecipe() { }
    }
}
