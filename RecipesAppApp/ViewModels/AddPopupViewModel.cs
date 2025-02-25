using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RecipesAppApp.ViewModels
{
    public partial class CreateRecipeViewModel : ViewModelBase
    {
        #region attributes and properties
        private string ingredientName;
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

        #endregion
    }
}
