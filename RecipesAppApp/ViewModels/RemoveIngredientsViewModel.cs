using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using RecipesAppApp.Models;

namespace RecipesAppApp.ViewModels
{
    public partial class RecipeDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<IngredientsWithNameAndAmount> userAndRecipeIngredients;
        private List<Ingredient> allUserIngredients;
        public ObservableCollection<IngredientsWithNameAndAmount> UserAndRecipeIngredients
        {
            get { return userAndRecipeIngredients; }
            set
            {
                this.userAndRecipeIngredients = value;
                OnPropertyChanged();
            }
        }
        public List<Ingredient> AllUserIngredients
        {
            get { return allUserIngredients; }
            set
            {
                this.allUserIngredients = value;
                OnPropertyChanged();
            }
        }

        public async void MakeListsForRemoveIngredients()
        {
            List<IngredientsWithNameAndAmount> IngredientsListForRemoveIngredients = new List<IngredientsWithNameAndAmount>();
            AllUserIngredients = await RecipesService.GetIngredientsByStorage(LoggedUser.Id);
            foreach(Ingredient i in Ingredients)
            {
                foreach(Ingredient ingredient in AllUserIngredients)
                {
                    if(i.Id == ingredient.Id)
                    {
                        IngredientsWithNameAndAmount iwna = new IngredientsWithNameAndAmount(i.Id, recipe.Id,0,"",i.IngredientName,false);
                        IngredientsListForRemoveIngredients.Add(iwna);
                    }
                }
            }
            UserAndRecipeIngredients = new ObservableCollection<IngredientsWithNameAndAmount>(IngredientsListForRemoveIngredients);
        }

    }
}
