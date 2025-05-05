using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using RecipesAppApp.Models;
using System.Windows.Input;

namespace RecipesAppApp.ViewModels
{
    public partial class RecipeDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<IngredientsWithNameAndAmount> userAndRecipeIngredients;
        private List<Ingredient> allUserIngredients;
        public ICommand RemoveIngredientsFromStorageCommand => new Command(RemoveIngredientsFromStorage);
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

        public async Task RemoveIngredientsFromStorage()
        {
            List<Ingredient> ingredientsToRemove = new List<Ingredient>();
            foreach (IngredientsWithNameAndAmount i in UserAndRecipeIngredients)
            {
                if (i.IsChecked)
                {
                    foreach(Ingredient ui in AllUserIngredients)
                    {
                        if(i.IngredientId == ui.Id)
                        {
                            ingredientsToRemove.Add(new Ingredient(ui));
                        }
                    }
                }
            }
            bool IsSuccessful = await RecipesService.RemoveStorageIngredient(ingredientsToRemove);
            if (IsSuccessful)
            {
                await Application.Current.MainPage.DisplayAlert("Remove Ingredient", "Ingredients were Removed", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Remove Ingredient", "Something went wrong, please try again later", "ok");
            }
        }

    }
}
