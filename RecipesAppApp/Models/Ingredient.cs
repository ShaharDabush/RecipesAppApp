using RecipesAppApp.Services;

namespace RecipesAppApp.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public bool Meat { get; set; }

        public bool Dairy { get; set; }

        public string Barcode { get; set; } = null!;
        public string IngredientImageURL
        {
            get
            {
                return RecipesAppWebAPIProxy.ImageBaseAddress + IngredientImage;
            }
        }

        public Ingredient() { }

        public Ingredient(Ingredient i)
        {
            Id = i.Id;
            IngredientName = i.IngredientName;
            IngredientImage = i.IngredientImage;
            IsKosher = i.IsKosher;
            IsGloten = i.IsGloten;
            Meat = i.Meat;
            Dairy = i.Dairy;
        }


    }
}
