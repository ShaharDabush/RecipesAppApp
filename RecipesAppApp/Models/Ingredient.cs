namespace RecipesAppApp.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public int KindId { get; set; }

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public bool Meat { get; set; }

        public bool Dairy { get; set; }

        public string Barkod { get; set; } = null!;

    }
}
