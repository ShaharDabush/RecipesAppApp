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

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public int HowManyMadeIt { get; set; }

        public bool ContainsMeat { get; set; }

        public bool ContainsDairy { get; set; }

        public string TimeOfDay { get; set; } = null!;
    }
}
