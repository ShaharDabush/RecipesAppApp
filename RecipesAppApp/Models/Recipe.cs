using RecipesAppApp.Services;

namespace RecipesAppApp.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        public string RecipesName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public string RecipeImage { get; set; } = null!;

        public string Kind { get; set; } = null!;

        public int MadeBy { get; set; }

        public int Rating { get; set; }

        public bool IsKosher { get; set; } 

        public bool IsGloten { get; set; } 

        public int HowManyMadeIt { get; set; }

        public bool ContainsMeat { get; set; }

        public bool ContainsDairy { get; set; }

        public string TimeOfDay { get; set; } = null!;

        public List<Allergy> Allergies { get; set; }

        public string RecipeImageURL
        {
            get
            {
                return RecipesAppWebAPIProxy.ImageBaseAddress + RecipeImage;
            }
        }

        public Recipe(Recipe r)
        {
            Id = r.Id;
            RecipesName = r.RecipesName;
            RecipeDescription = r.RecipeDescription;
            RecipeImage = r.RecipeImage;
            MadeBy = r.MadeBy;
            Rating = r.Rating;
            IsKosher = r.IsKosher;
            IsGloten = r.IsGloten;
            HowManyMadeIt = r.HowManyMadeIt;
            ContainsMeat = r.ContainsMeat;
            ContainsDairy = r.ContainsDairy;
            TimeOfDay = r.TimeOfDay;
            Allergies = r.Allergies;
        }
        public Recipe() { }
    }
}
