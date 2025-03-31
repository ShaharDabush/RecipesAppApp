using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using static Android.Graphics.Paint;

namespace RecipesAppApp.ViewModels
{
    public partial class CreateRecipeViewModel : ViewModelBase
    {
        #region attributes and properties
        private ObservableCollection<Ingredient> allIngredients;
        private ObservableCollection<Ingredient> searchedIngredient;
        private ObservableCollection<Level> directions;
        List<Level> ListOfDirections = new List<Level>();
        public event Action<List<string>> OpenPopup;
        private string recipeName;
        private string desciption;
        private string timeOfDay;
        private bool containMeat;
        private bool containDairy;
        private bool isKosher;
        private bool isGloten;
        private string searchedName;
        private bool inSearch;

        public string TimeOfDay
        {
            get
            {
                return this.timeOfDay;
            }
            set
            {
                this.timeOfDay = value;
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
        public ObservableCollection<Level> Directions
        {
            get => directions;
            set
            {
                directions = value;
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
            PhotoURL = proxy.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }

        public ICommand DiscardIngredientCommand { get; set; }
        public ICommand AddDirectionCommand { get; set; }
        public ICommand DiscardLevelCommand { get; set; }
        public ICommand SaveRecipeCommand { get; set; }
        #endregion
        private RecipesAppWebAPIProxy proxy;

        #endregion
        public CreateRecipeViewModel(RecipesAppWebAPIProxy proxy)
        {
            this.proxy = proxy;
            UploadPhotoCommand = new Command(OnUploadPhoto);
            SaveIngredientCommand = new Command(SaveIngredient);
            AddDirectionCommand = new Command(AddDirection);
            SaveRecipeCommand = new Command(SaveRecipe);
            DiscardLevelCommand = new Command<int>((int levelCount) => RemoveLevel(levelCount));
            DiscardIngredientCommand = new Command<int>((int IngredientId) => DiscardIngredient(IngredientId));
            PhotoURL = proxy.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            ImageResult = "";
            UploadPhotoCommand = new Command(OnUploadPhoto);
            RecipeName = "New Recipe";
            AmountError = "You must use only numbers!";
            MeasureUnitError = "You must choose a measure unit";
            Desciption = "";
            InSearch = false;
            ListOfMeasureUnits = new List<string>();
            ListOfMeasureUnits.Add("tsp");
            ListOfMeasureUnits.Add("tbsp");
            ListOfMeasureUnits.Add("fl");
            ListOfMeasureUnits.Add("cup");
            ListOfMeasureUnits.Add("pt");
            ListOfMeasureUnits.Add("qt");
            ListOfMeasureUnits.Add("gal");
            ListOfMeasureUnits.Add("oz");
            ListOfMeasureUnits.Add("lb");
            ListOfMeasureUnits.Add("g");
            ListOfMeasureUnits.Add("kg");
            ListOfMeasureUnits.Add("°C");
            ListOfMeasureUnits.Add("°F");
            ListOfMeasureUnits.Add("units");
            ListOfMeasureUnits.Add("L");
            GetIngredients();
        }

        public async void GetIngredients()
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
            GetIngredients();
            this.SearchedIngredient = AllIngredients;
            InSearch = false;
        }
        //public void DiscardIngredient(int IngredientId)
        //{
        //    for (int i = 0; i < ListOfAddedIngredients.Count; i++)
        //    {
        //        if (IngredientId == ListOfAddedIngredients[i].IngredientId)
        //        {
        //            ListOfAddedIngredients.Remove(ListOfAddedIngredients[i]);
        //        }
        //    }
        //    OnPropertyChanged("ListOfAddedIngredients");
        //}
        public void DiscardIngredient(int IngredientId)
        {
            for (int i = 0; i < ListOfNewIngredients.Count; i++)
            {
                if (IngredientId == ListOfNewIngredients[i].IngredientId)
                {
                    ListOfNewIngredients.Remove(ListOfNewIngredients[i]);
                }
            }
            ListOfAddedIngredient = new ObservableCollection<IngredientsWithNameAndAmount>(ListOfNewIngredients);
        }

        public void AddDirection()
        {
            Level newLevel = new Level(ListOfDirections.Count+1,"", ListOfDirections.Count + 1,999);
            ListOfDirections.Add(newLevel);
            Directions = new ObservableCollection<Level>(ListOfDirections);
        }
        public void RemoveLevel(int levelCount)
        {
            for (int i = 0; i < ListOfDirections.Count; i++)
            {
                if (levelCount == ListOfDirections[i].LevelCount)
                {
                    ListOfDirections.Remove(ListOfDirections[i]);
                    for (int j = i + 1; j < ListOfDirections.Count; j++)
                    {
                        ListOfDirections[j].LevelCount--;
                    }
                }
            }
            Directions = new ObservableCollection<Level>(ListOfDirections);
        }

        public async void SaveRecipe()
        {
            SaveRecipeInfo saveRecipeInfo = new SaveRecipeInfo();
            Recipe newRecipe = new Recipe();
            newRecipe.RecipeImage = photoURL;
            newRecipe.RecipesName = recipeName;
            newRecipe.RecipeDescription = Desciption;
            newRecipe.MadeBy = ((App)Application.Current).LoggedInUser.Id;
            newRecipe.Rating = 0;
            newRecipe.IsKosher = IsKosher;
            newRecipe.IsGloten = IsGloten;
            newRecipe.ContainsDairy = ContainDairy;
            newRecipe.ContainsMeat = ContainMeat;
            newRecipe.HowManyMadeIt = 0;
            newRecipe.TimeOfDay = "TimeOfDay";
            List<Level> levels = new List<Level>();
            levels = ListOfDirections;
            List<IngredientRecipe> ingredientRecipes = new List<IngredientRecipe>();
            foreach (IngredientsWithNameAndAmount i in ListOfNewIngredients)
            {
                ingredientRecipes.Add(new IngredientRecipe(i.RecipeId, i.IngredientId, i.Amount, i.MeasureUnits));
            }
            saveRecipeInfo.LevelsInfo = levels;
            saveRecipeInfo.IngredientsInfo = ingredientRecipes;
            saveRecipeInfo.RecipeInfo = newRecipe;
            saveRecipeInfo = await proxy.SaveRecipe(saveRecipeInfo);

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

        public void OnSingleSelectIngredient()
        {
            if (OpenPopup != null)
            {
                List<string> l = new List<string>();
                OpenPopup(l);
                this.IngredientName = SelectedIngredient.IngredientName;
                this.IngredientId = SelectedIngredient.Id;
                selectedIngredient = null;
            }
            
        }
     


        #endregion
    }
}



    

