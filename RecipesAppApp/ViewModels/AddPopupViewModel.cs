using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using System;
using System.Collections.Generic;
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
        public string amount;
        public string Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                if (!ValidateAmount())
                {
                    this.amount = value;
                    OnPropertyChanged();
                }
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
        #endregion
        private string ingredientName;
        private string measureUnit;
        private List<string> listOfMeasureUnits;
        public List<string> ListOfMeasureUnits
        {
            get => listOfMeasureUnits;
            set
            {
                listOfMeasureUnits = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public string MeasureUnit
        {
            get => measureUnit;
            set
            {
                measureUnit = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public string IngredientName
        {
            get => ingredientName;
            set
            {
                ingredientName = value;
                OnPropertyChanged();
                Sort();
            }
        }
        private bool ValidateAmount()
        {
            int d = 0;
            if(!int.TryParse(this.Amount, out d))
            {
                this.ShowAmountError = false;
                return false;
            }
            else
            {
                this.ShowAmountError = true;
                return true;
            }
           
        }
        public ICommand SaveIngredientCommand { get; set; }
        #endregion

        public void SaveIngredient(Ingredient i)
        {
            IngredientsWithNameAndAmount NewIngredient = new IngredientsWithNameAndAmount(i.Id, 999, Amount, MeasureUnit);
        }
    }
}
