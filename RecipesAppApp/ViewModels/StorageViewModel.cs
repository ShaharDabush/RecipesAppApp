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
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace RecipesAppApp.ViewModels
{
    public partial class StorageViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private IServiceProvider serviceProvider;
        private User loggedUser;
        private ObservableCollection<Ingredient> ingredientsList1;
        private ObservableCollection<Ingredient> ingredientsList2;
        private ObservableCollection<Ingredient> ingredientsListForStorage;
        private ObservableCollection<Ingredient> ingredientsListForNewIngredient;
        public event Action<List<string>> OpenPopup;
        public event Action<List<string>> OpenPopup1;
        private Storage storage;
        private string searchedIngredientInStorage;
        private string storageName;
        private string searchedNewIngredient;
        public ICommand OpenCreateIngredientCommand { get; set; }
        public ICommand RemoveIngredientCommand { get; set; }

        public string SearchedNewIngredient
        {
            get
            {
                return searchedNewIngredient;
            }
            set
            {
                this.searchedNewIngredient = value;
                OnPropertyChanged();
                SortForNewIngredients();
            }
        }
        public string StorageName
        {
            get
            {
                return storageName;
            }
            set
            {
                this.storageName = value;
                OnPropertyChanged();
                SortForNewIngredients();
            }
        }

        public string SearchedIngredientInStorage
        {
            get
            {
                return searchedIngredientInStorage;
            }
            set
            {
                this.searchedIngredientInStorage = value;
                OnPropertyChanged();
                SortForStorage();
            }
        }
        public ObservableCollection<Ingredient> IngredientsList1
        {
            get
            {
                return ingredientsList1;
            }
            set
            {
                this.ingredientsList1 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> IngredientsList2
        {
            get
            {
                return ingredientsList2;
            }
            set
            {
                this.ingredientsList2 = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> IngredientsListForStorage
        {
            get
            {
                return ingredientsListForStorage;
            }
            set
            {
                this.ingredientsListForStorage = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> IngredientsListForNewIngredient
        {
            get
            {
                return ingredientsListForNewIngredient;
            }
            set
            {
                this.ingredientsListForNewIngredient = value;
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

        public StorageViewModel(RecipesAppWebAPIProxy service, IServiceProvider sp)
        {
            this.RecipesService = service;
            serviceProvider = sp;
            LoggedUser = ((App)Application.Current).LoggedInUser;
            SaveStorageCommand = new Command(SaveStorage);
            CancelCommand = new Command(Cancel);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            OpenCreateIngredientCommand = new Command(OpenCreateIngredient);
            SaveingredientCommand = new Command(Saveingredient);
            BackToBarcodeCommend = new Command(BackToBarcode);
            RemoveIngredientCommand = new Command<int>((int Id) => RemoveIngredient(Id));
            IsInCameraMode = false;

            if (loggedUser.StorageId != null)
            {
                SetUserIngredients();
               StorageName = ((App)Application.Current).UserStorage.StorageName;
            }
        }

        public async void SetUserIngredients()
        {
            Storage = await this.RecipesService.GetStoragesbyUser(LoggedUser.Id);
            List<Ingredient> ingredients = await this.RecipesService.GetIngredientsByStorage(storage.Id);
            this.IngredientsList1 = new ObservableCollection<Ingredient>(ingredients);
            this.IngredientsListForStorage = new ObservableCollection<Ingredient>(ingredients);
            List<Ingredient> allIngredients = await this.RecipesService.GetAllIngredients();
            this.IngredientsList2 = new ObservableCollection<Ingredient>(allIngredients);
            this.IngredientsListForNewIngredient = new ObservableCollection<Ingredient>(allIngredients);
        }

        //on SortCommand change the list and leave only the users that contain the given string
        public void SortForStorage()
        {
            if (string.IsNullOrEmpty(SearchedIngredientInStorage))
            {
                ClearSortForStorage();
            }
            else
            {
                List<Ingredient> temp1 = IngredientsList1.Where(i => i.IngredientName.ToLower().Contains(SearchedIngredientInStorage.ToLower())).ToList();
                this.IngredientsListForStorage.Clear();
                foreach (Ingredient i in temp1)
                {
                    this.IngredientsListForStorage.Add(i);
                }
            }
        }
        public void ClearSortForStorage()
        {
            if (!string.IsNullOrEmpty(SearchedIngredientInStorage))
                this.SearchedIngredientInStorage = null;
            SetUserIngredients();
            this.IngredientsListForStorage = IngredientsList1;
        }

        public void SortForNewIngredients()
        {
            if (string.IsNullOrEmpty(SearchedNewIngredient))
            {
                ClearForNewIngredients();
            }
            else
            {
                List<Ingredient> temp2 = IngredientsList2.Where(i => i.IngredientName.ToLower().Contains(SearchedNewIngredient.ToLower())).ToList();
                this.IngredientsListForNewIngredient.Clear();
                foreach (Ingredient i in temp2)
                {
                    this.IngredientsListForNewIngredient.Add(i);
                }
            }
        }
        public void ClearForNewIngredients()
        {
            if (!string.IsNullOrEmpty(SearchedNewIngredient))
                this.SearchedNewIngredient = null;
            SetUserIngredients();
            this.IngredientsListForNewIngredient = IngredientsList2;
        }

        public void OpenCreateIngredient()
        {
            IsAddingredientVisible = false;
            IsNewingredientVisible = false;
            IngredientCode = "";
            IsInCameraMode = true;
            PopupSize = new Size(270,300);
            if (OpenPopup != null)
            {
                List<string> l = new List<string>();
                OpenPopup(l);
            }
            //GetIngredientByBarcode();
        }
        public void BackToBarcode()
        {
            IsAddingredientVisible = false;
            IsNewingredientVisible = false;
            IngredientCode = "";
            IsInCameraMode = true;
            PopupSize = new Size(270, 300);
        }

        public async void IsStorageNullInitData()
        {
            if(LoggedUser.StorageId == null)
            {
                await StorageNull();
            }
            else
            {
                SetUserIngredients();
                StorageName = ((App)Application.Current).UserStorage.StorageName;
            }
        }
        public async Task StorageNull()
        {
            //await AppShell.Current.GoToAsync("///HomePage");
            if (OpenPopup1 != null)
            {
                List<string> l = new List<string>();
                OpenPopup1(l);
            }
            //await Application.Current.MainPage.DisplayAlert("Storage does not exeist", "You have been kicked out of your storage please create new one", "ok");
        }
        public async void RemoveIngredient(int Id)
        {
            bool isChanged;
            if (Id != 0)
            {
                isChanged = await this.RecipesService.RemoveStorageIngredient(Id, ((App)Application.Current).UserStorage.Id);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                SetUserIngredients();
                OnPropertyChanged("IngredientsListForStorage");
            }
        }

        #region Single Selection

        private Ingredient selectedIngredient;
        public Ingredient SelectedIngredient
        {
            get
            {
                return this.selectedIngredient;
            }
            set
            {
                if (this.selectedIngredient != value)
                {
                    this.selectedIngredient = value;

                    if (selectedIngredient != null)
                        OnSingleSelectIngredient();
                    OnPropertyChanged();
                }

            }
        }

        public async void OnSingleSelectIngredient()
        {
            bool AlreadyExists = false;
            foreach (Ingredient i in IngredientsListForStorage)
            {
                if(i.Id == SelectedIngredient.Id)
                {
                    AlreadyExists = true;
                }
            }
            if (AlreadyExists)
            {
                await Application.Current.MainPage.DisplayAlert("Add Ingredient", "you already has this ingredient in your storage", "ok");
            }
            else
            {
              if (OpenPopup != null)
                {
                    List<string> l = new List<string>();
                    OpenPopup(l);
                    this.IngredientCode = SelectedIngredient.Barcode;
                    selectedIngredient = null;
                    GetIngredientByBarcode();
                }       
            }


        }



        #endregion
    }
}
