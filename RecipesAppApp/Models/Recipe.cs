namespace RecipesAppApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string RecipesName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public string RecipeImage { get; set; } = null!;

        public int MadeBy { get; set; }

        public int Rating { get; set; }

        public string IsKosher { get; set; } = null!;

        public string IsGloten { get; set; } = null!;

        public int HowManyMadeIt { get; set; }

        public int ContainsMeat { get; set; }

        public int ContainsDairy { get; set; }

        public string TimeOfDay { get; set; } = null!;
    }
}
