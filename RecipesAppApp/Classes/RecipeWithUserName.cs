using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Services;

namespace RecipesAppApp.Classes
{
    public class RecipeWithUserName
    {
        public int Id { get; set; }

        public string RecipesName { get; set; } = null!;

        public string RecipeDescription { get; set; } = null!;

        public string RecipeImage { get; set; } = null!;

        public int MadeBy { get; set; }

        public int Rating { get; set; }

        public int HowManyMadeIt { get; set; }

        public string UserName { get; set; }
        public string RecipeImageURL
        {
            get
            {
                return RecipesAppWebAPIProxy.ImageBaseAddress + RecipeImage;
            }
        }

        public RecipeWithUserName() { }
        public RecipeWithUserName(int Id, string RecipesName, string RecipeDescription, string RecipeImage, int MadeBy, int Rating ,int HowManyMadeIt, string UserName)
        {
            this.Id = Id;
            this.RecipesName = RecipesName;
            this.RecipeDescription = RecipeDescription;
            this.RecipeImage = RecipeImage;
            this.MadeBy = MadeBy;
            this.Rating = Rating;
            this.HowManyMadeIt = HowManyMadeIt;
            this.UserName = UserName;
        }
    }
}
