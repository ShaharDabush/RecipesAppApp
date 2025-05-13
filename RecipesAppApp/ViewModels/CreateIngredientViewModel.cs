using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using ZXing.QrCode.Internal;


namespace RecipesAppApp.ViewModels
{
    public partial class StorageViewModel : ViewModelBase
    {
        #region attributes and properties
        private string ingredientCode;
        private Ingredient? ingredientBarcode;
        private Size popupSize;
        #region New Ingredient
        private bool containMeat;
        private bool containDairy;
        private bool isKosher;
        private bool isGloten;
        private string newIngredientName;
        public string NewIngredientName
        {
            get
            {
                return this.newIngredientName;
            }
            set
            {
                this.newIngredientName = value;
                OnPropertyChanged();
            }
        }
        public bool ContainMeat
        {
            get
            {
                return this.containMeat;
            }
            set
            {
                this.containMeat = value;
                OnPropertyChanged();
            }
        }
        public bool ContainDairy
        {
            get
            {
                return this.containDairy;
            }
            set
            {
                this.containDairy = value;
                OnPropertyChanged();
            }
        }
        public bool IsKosher
        {
            get
            {
                return this.isKosher;
            }
            set
            {
                this.isKosher = value;
                OnPropertyChanged();
            }
        }
        public bool IsGloten
        {
            get
            {
                return this.isGloten;
            }
            set
            {
                this.isGloten = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region IsVisible properties
        private bool isInCameraMode;
        private bool isNewingredientVisible;
        private bool isAddingredientVisible;
        public bool IsAddingredientVisible
        {
            get
            {
                return isAddingredientVisible;
            }
            set
            {
                this.isAddingredientVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsNewingredientVisible
        {
            get
            {
                return isNewingredientVisible;
            }
            set
            {
                this.isNewingredientVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsInCameraMode
        {
            get
            {
                return isInCameraMode;
            }
            set
            {
                this.isInCameraMode = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public Size PopupSize
        {
            get
            {
                return this.popupSize;
            }
            set
            {
                this.popupSize = value;
                OnPropertyChanged();
            }
        }
        public Ingredient? IngredientBarcode
        {
            get
            {
                return this.ingredientBarcode;
            }
            set
            {
                this.ingredientBarcode = value;
                OnPropertyChanged();
            }
        }
        public string IngredientCode
        {
            get
            {
                return ingredientCode;
            }
            set
            {
                this.ingredientCode = value;
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
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
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
            PhotoURL = RecipesService.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }
        #endregion
        public Command BackToBarcodeCommend { set; get; }
        public Command SaveingredientCommand { set; get; }
        #endregion

        public async void GetIngredientByBarcode()
        {
            IsInCameraMode = false;
            IngredientBarcode = await RecipesService.GetIngredientsByBarcode(IngredientCode);
            if (IngredientBarcode == null)
            {
                PopupSize = new Size(340, 600);
                IsNewingredientVisible = true;
                IngredientBarcode = new Ingredient();
                NewIngredientName = "New Ingredient";
                IngredientBarcode.IngredientImage = PhotoURL;
                IngredientBarcode.Barkod = IngredientCode;

            }
            else
            {
                bool AlreadyExists = false;
                foreach (Ingredient i in IngredientsListForStorage)
                {
                    if (i.Id == IngredientBarcode.Id)
                    {
                        AlreadyExists = true;
                    }
                }
                if(AlreadyExists)
                {
                     await Application.Current.MainPage.DisplayAlert("Add Ingredient", "you already has this ingredient in your storage", "ok");
                }
                else
                {
                    PopupSize = new Size(340, 150);
                    IsAddingredientVisible = true;
                }
            }
    
            
        }

        public async void AddIngredientToStorage()
        {
            bool result = await RecipesService.AddIngredietToStorage(IngredientBarcode.Id,LoggedUser.StorageId.Value);
            if(result)
            {
                await Application.Current.MainPage.DisplayAlert("Add Ingredient", "Ingredient was added to your storage", "ok");
                OnPropertyChanged("IngredientsListForStorage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Add Ingredient", "Something went wrong, please try again later", "ok");
            }
        }

        public async void Saveingredient()
        {
            IngredientBarcode = new Ingredient();
            IngredientBarcode.IngredientName = NewIngredientName;
            IngredientBarcode.IngredientImage = PhotoURL;
            IngredientBarcode.IsKosher = IsKosher;
            IngredientBarcode.IsGloten = IsGloten;
            IngredientBarcode.Dairy = ContainDairy;
            IngredientBarcode.Meat = ContainMeat;
            IngredientBarcode.Barkod = IngredientCode;
            IngredientBarcode.KindId = 1;
            bool result = await RecipesService.SaveIngredient(IngredientBarcode,LoggedUser.StorageId.Value);
            if(result)
            {
                Ingredient i = new(IngredientBarcode);
                string updatedIngredient = await RecipesService.UploadIngredientImage(i);
                if (updatedIngredient == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Save Ingredient", "Ingredient Data Was Saved BUT recipe image upload failed", "ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Save Ingredient", "Ingredient was saved and added to your storage", "ok");
                    SetUserIngredients();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Save Ingredient", "Something went wrong, please try again later", "ok");
            }
        }

    }
}
