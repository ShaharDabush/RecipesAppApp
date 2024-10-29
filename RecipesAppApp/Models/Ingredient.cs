namespace RecipesAppApp.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        public string IngredientName { get; set; } = null!;

        public string IngredientImage { get; set; } = null!;

        public int KindId { get; set; }

        public string MeatOrDariy { get; set; } = null!;

        public string IsKosher { get; set; } = null!;

        public string IsGloten { get; set; } = null!;

        public string Barkod { get; set; } = null!;

    }
}
