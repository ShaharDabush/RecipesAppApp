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

namespace RecipesAppApp.ViewModels
{
    public partial class CreateRecipeViewModel : ViewModelBase
    {
        #region attributes and properties

        #region Amount
        public int amount;
        public int Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                    this.amount = value;
                    OnPropertyChanged();
                ValidateAmount();
                ((Command)SaveIngredientCommand).ChangeCanExecute();

            }
        }
        private bool showAmountError;

        public bool ShowAmountError
        {
            get => showAmountError;
            set
            {
                showAmountError = value;
                OnPropertyChanged("showAmountError");
            }
        }
        private string amountError;

        public string AmountError
        {
            get => amountError;
            set
            {
                amountError = value;
                OnPropertyChanged("amountError");
            }
        }
        private bool ValidateAmount()
        {
            if(Amount < 0)
            {
                ShowAmountError = true;
                AmountError = "Please enter a valid amount";
                return false;
            }
            else
            {
                ShowAmountError = false;
                AmountError = "";
                return true;
            }
        }
        #endregion
        #region MeasureUnits
        private string measureUnit;
        public string MeasureUnit
        {
            get => measureUnit;
            set
            {
                measureUnit = value;
                OnPropertyChanged();
                ((Command)SaveIngredientCommand).ChangeCanExecute();
            }
        }
        private bool showMeasureUnitError;

        public bool ShowMeasureUnitError
        {
            get => showMeasureUnitError;
            set
            {
                showMeasureUnitError = value;
                OnPropertyChanged("showMeasureUnitError");
            }
        }
        private string measureUnitError;

        public string MeasureUnitError
        {
            get => measureUnitError;
            set
            {
                measureUnitError = value;
                OnPropertyChanged("measureUnitError");
            }
        }
        private bool ValidateMeasurementUnits()
        {
            return this.showMeasureUnitError = string.IsNullOrEmpty(MeasureUnit);
        }
        #endregion
        private string ingredientName;
        List<IngredientsWithNameAndAmount> ListOfNewIngredients = new List<IngredientsWithNameAndAmount>();
        private ObservableCollection<IngredientsWithNameAndAmount> listOfAddedIngredient;
        private int ingredientId;
        private List<string> listOfMeasureUnits;
        public List<string> ListOfMeasureUnits
        {
            get => listOfMeasureUnits;
            set
            {
                listOfMeasureUnits = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IngredientsWithNameAndAmount> ListOfAddedIngredient
        {
            get => listOfAddedIngredient;
            set
            {
                listOfAddedIngredient = value;
                OnPropertyChanged();
            }
        }
        public string IngredientName
        {
            get => ingredientName;
            set
            {
                ingredientName = value;
                OnPropertyChanged();
            }
        }
        public int IngredientId
        {
            get => ingredientId;
            set
            {
                ingredientId = value;
                OnPropertyChanged();
            }
        }
        public Command SaveIngredientCommand { get; set; }

        #endregion

        public void SaveIngredient()
        {

            if (!ValidateMeasurementUnits() && ValidateAmount())
            {
                IngredientsWithNameAndAmount NewIngredient = new IngredientsWithNameAndAmount(IngredientId ,0,Amount,MeasureUnit, IngredientName,false);
                ListOfNewIngredients.Add(NewIngredient);
                ListOfAddedIngredient = new ObservableCollection<IngredientsWithNameAndAmount>(ListOfNewIngredients);
                MeasureUnit = "";
                Amount = 0;
            }
           
        }

    }
}
