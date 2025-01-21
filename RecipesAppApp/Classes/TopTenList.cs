using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.Classes
{
    public class TopTenList
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string RecipeImage { get; set; }
        public int RecipePlace { get; set; }
        public TopTenList(int Id, string RecipeName, string RecipeImage, int RecipePlace)
        {
            this.Id = Id;
            this.RecipeName = RecipeName;
            this.RecipeImage = RecipeImage;
            this.RecipePlace = RecipePlace;
        }
    }
}
