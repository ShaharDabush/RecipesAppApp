using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using RecipesAppApp.ViewModels;
using RecipesAppApp.Services;
using RecipesAppApp.Views;
using System.Collections.ObjectModel;

namespace RecipesAppApp.ViewModels
{
    public class StorageViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private User loggedUser;
        private ObservableCollection<Ingredient> ingredientsList;
        private ObservableCollection<Ingredient> ingredientsListForShow;
        private Storage storage;
        private string searchedIngredient;

        public string SearchedIngredient
        {
            get
            {
                return searchedIngredient;
            }
            set
            {
                this.searchedIngredient = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public ObservableCollection<Ingredient> IngredientsList
        {
            get
            {
                return ingredientsList;
            }
            set
            {
                this.ingredientsList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> IngredientsListForShow
        {
            get
            {
                return ingredientsListForShow;
            }
            set
            {
                this.ingredientsListForShow = value;
                OnPropertyChanged();
            }
        }
        public User LoggedUser
        {
            get
            {
                return loggedUser;
            }
            set
            {
                this.loggedUser = value;
                OnPropertyChanged();
            }
        }
        public Storage Storage
        {
            get
            {
                return storage;
            }
            set
            {
                this.storage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public StorageViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            loggedUser = ((App)Application.Current).LoggedInUser;
            SetUserIngredients();
        }

        public async void SetUserIngredients()
        {
            Storage = await this.RecipesService.GetStoragesbyUser(LoggedUser.Id);
            List<Ingredient> ingredients = await this.RecipesService.GetIngredientsByStorage(storage.Id);
            this.IngredientsList = new ObservableCollection<Ingredient>(ingredients);
            this.IngredientsListForShow = new ObservableCollection<Ingredient>(ingredients);

        }

        //on SortCommand change the list and leave only the users that contain the given string
        public void Sort()
        {
            if (string.IsNullOrEmpty(SearchedIngredient))
            {
                ClearSort();
            }
            else
            {
                List<Ingredient> temp = IngredientsList.Where(i => i.IngredientName.ToLower().Contains(SearchedIngredient.ToLower())).ToList();
                this.IngredientsListForShow.Clear();
                foreach (Ingredient i in temp)
                {
                    this.IngredientsListForShow.Add(i);
                }
            }
        }
        public void ClearSort()
        {
            if (!string.IsNullOrEmpty(SearchedIngredient))
                this.SearchedIngredient = null;
            SetUserIngredients();
            this.IngredientsListForShow = IngredientsList;
        }

    }
}
