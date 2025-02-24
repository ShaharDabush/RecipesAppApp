using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
using RecipesAppApp.Services;

namespace RecipesAppApp.ViewModels
{
    public class CreateRecipeViewModel : ViewModelBase
    {
        #region attributes and properties
        private ObservableCollection<Ingredient> allIngredients;
        private ObservableCollection<Ingredient> searchedIngredient;
        private string recipeName;
        private string desciption;
        private string searchedName;
        private bool inSearch;

        public bool InSearch
        {
            get
            {
                return this.inSearch;
            }
            set
            {
                this.inSearch = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> AllIngredients
        {
            get => allIngredients;
            set
            {
                allIngredients = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Ingredient> SearchedIngredient
        {
            get => searchedIngredient;
            set
            {
                searchedIngredient = value;
                OnPropertyChanged();
            }
        }
        public string Desciption
        {
            get => desciption;
            set
            {
                desciption = value;
                OnPropertyChanged();
            }
        }
        public string SearchedName
        {
            get => searchedName;
            set
            {
                searchedName = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public string RecipeName
        {
            get => recipeName;
            set
            {
                recipeName = value;
                OnPropertyChanged();
            }
        }

        #region Photo

        private string imageResult;
        public string ImageResult
        {
            get => imageResult;
            set
            {
                imageResult = value;
                OnPropertyChanged("PhotoURL");
            }
        }

        private string photoURL;

        public string PhotoURL
        {
            get => photoURL;
            set
            {
                photoURL = value;
                OnPropertyChanged("PhotoURL");
            }
        }

        private string localPhotoPath;

        public string LocalPhotoPath
        {
            get => localPhotoPath;
            set
            {
                localPhotoPath = value;
                OnPropertyChanged("LocalPhotoPath");
            }
        }

        public Command UploadPhotoCommand { get; }
        //This method open the file picker to select a photo
        private async void OnUploadPhoto()
        {
            try
            {
                var result = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Please select a photo",
                });

                if (result != null)
                {
                    // The user picked a file
                    this.LocalPhotoPath = result.FullPath;
                    this.PhotoURL = result.FullPath;
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void UpdatePhotoURL(string virtualPath)
        {
            Random r = new Random();
            PhotoURL = proxy.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }

        #endregion
        private RecipesAppWebAPIProxy proxy;

        #endregion
        public CreateRecipeViewModel(RecipesAppWebAPIProxy proxy)
        {
            this.proxy = proxy;
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = proxy.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            ImageResult = "";
            UploadPhotoCommand = new Command(OnUploadPhoto);
            RecipeName = "New Recipe";
            Desciption = "";
            InSearch = false;
            GetRecipes();
        }

        public async void GetRecipes()
        {
            List<Ingredient> i = await proxy.GetAllIngredients();
            AllIngredients = new ObservableCollection<Ingredient>(i);
            SearchedIngredient = new ObservableCollection<Ingredient>(i);
        }

        public void Sort()
        {
            if (string.IsNullOrEmpty(SearchedName))
            {
                ClearSort();
            }
            else
            {
                List<Ingredient> temp = AllIngredients.Where(i => i.IngredientName.ToLower().Contains(SearchedName.ToLower())).ToList();
                this.SearchedIngredient.Clear();
                foreach (Ingredient r in temp)
                {
                    this.SearchedIngredient.Add(r);
                }
                InSearch = true;
                //this.IsLogged = false;
            }
        }

        public void ClearSort()
        {
            if (!string.IsNullOrEmpty(SearchedName))
                this.SearchedName = null;
            this.SearchedIngredient = AllIngredients;
            InSearch = false;
        }
        #region Single Selection

        private Recipe selectedIngredient;
        public Recipe SelectedIngredient
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

        async void OnSingleSelectIngredient()
        {
            var navParam = new Dictionary<string, object>()
                {
                    { "Ingredient",SelectedIngredient }
                };
            await Shell.Current.GoToAsync("RecipeDetails", navParam);
            SelectedIngredient = null;
        }
      


        #endregion
    }
}



    

