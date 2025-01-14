
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;


namespace RecipesAppApp.ViewModels
{
    [QueryProperty(nameof(SelectedRecipe), "selectedRecipe")]
    public class RecipeDetailsViewModel : ViewModelBase
    {
        private Recipe selectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set
            {
                this.selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                this.name = value;
                OnPropertyChanged();
            }
        }
        public RecipeDetailsViewModel()
        {
            Name = SelectedRecipe.RecipesName;
        }
    }
}
