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

        #endregion

        public async void GetIngredientByBarcode()
        {
            IsInCameraMode = false;
            IngredientCode = "Barkod";
            IngredientBarcode = await RecipesService.GetIngredientsByBarcode(IngredientCode);
            if (IngredientBarcode == null)
            {
                PopupSize = new Size(340, 600);
                IsNewingredientVisible = true;
                IngredientBarcode = new Ingredient();
                IngredientBarcode.IngredientName = "";

            }
            else
            {
                PopupSize = new Size(340, 150);
                IsAddingredientVisible = true;
            }
    
            
        }

        public async void AddIngredientToStorage()
        {
            bool result = await RecipesService.AddIngredietToStorage(IngredientBarcode.Id,LoggedUser.StorageId.Value);
            if(result)
            {
                await Application.Current.MainPage.DisplayAlert("Add Ingredient", "Ingredient was added to your storage", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Add Ingredient", "Something went wrong, please try again later", "ok");
            }
        }


    }
}
