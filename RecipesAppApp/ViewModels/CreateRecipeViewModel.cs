using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Service.Voice;
using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using static Android.Util.EventLogTags;

namespace RecipesAppApp.ViewModels
{
    public partial class CreateRecipeViewModel : ViewModelBase
    {
        #region attributes and properties
        private ObservableCollection<Ingredient> allIngredients;
        private ObservableCollection<Ingredient> searchedIngredient;
        private ObservableCollection<Allergy> allergies;
        private ObservableCollection<UserAllergyWithIsChecked> allergiesList;
        private ObservableCollection<Level> directions;
        private List<string> listOfType = new List<string>();
        List<Level> ListOfDirections = new List<Level>();
        public event Action<List<string>> OpenPopup;
        private string type;
        private string recipeName;
        private string desciption;
        private string timeOfDay;
        private bool containMeat;
        private bool containDairy;
        private bool isKosher;
        private bool isGloten;
        private bool morning;
        private bool noon;
        private bool evening;
        private bool anyTime;
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
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                OnPropertyChanged();
            }

        }
        public List<string> ListOfType
        {
            get
            {
                return this.listOfType;
            }
            set
            {
                this.listOfType = value;
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
        public bool Morning
        {
            get
            {
                return this.morning;
            }
            set
            {
                this.morning = value;
                OnPropertyChanged();
            }
        }
        public bool Noon
        {
            get
            {
                return this.noon;
            }
            set
            {
                this.noon = value;
                OnPropertyChanged();
            }
        }
        public bool Evening
        {
            get
            {
                return this.evening;
            }
            set
            {
                this.evening = value;
                OnPropertyChanged();
            }
        }
        public bool AnyTime
        {
            get
            {
                return this.anyTime;
            }
            set
            {
                this.anyTime = value;
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
        public ObservableCollection<Allergy> Allergies
        {
            get => allergies;
            set
            {
                allergies = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserAllergyWithIsChecked> AllergiesList
        {
            get => allergiesList;
            set
            {
                allergiesList = value;
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
            PhotoURL = RecipesService.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }
        #endregion

        public ICommand DiscardIngredientCommand { get; set; }
        public ICommand AddDirectionCommand { get; set; }
        public ICommand DiscardLevelCommand { get; set; }
        public ICommand SaveRecipeCommand { get; set; }
        #endregion
        private RecipesAppWebAPIProxy RecipesService;

        public CreateRecipeViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            UploadPhotoCommand = new Command(OnUploadPhoto);
            SaveIngredientCommand = new Command(SaveIngredient, () => (!string.IsNullOrEmpty(MeasureUnit) && Amount > 0));
            AddDirectionCommand = new Command(AddDirection);
            SaveRecipeCommand = new Command(SaveRecipe);
            DiscardLevelCommand = new Command<int>((int levelCount) => RemoveLevel(levelCount));
            DiscardIngredientCommand = new Command<int>((int IngredientId) => DiscardIngredient(IngredientId));
            PhotoURL = RecipesService.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            AnyTime = true;
            ImageResult = "";
            UploadPhotoCommand = new Command(OnUploadPhoto);
            RecipeName = "New Recipe";
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
            ListOfType.Add("Does not have");
            ListOfType.Add("Desert");
            ListOfType.Add("Japanise");
            ListOfType.Add("Italian");
            ListOfType.Add("French");
            ListOfType.Add("German");
            GetIngredients();
            GetAllergies();
        }

        public async void GetIngredients()
        {
            List<Ingredient> i = await RecipesService.GetAllIngredients();
            AllIngredients = new ObservableCollection<Ingredient>(i);
            SearchedIngredient = new ObservableCollection<Ingredient>(i);
        }
        public async void GetAllergies()
        {
            List<Allergy> allAllergies = await RecipesService.GetAllAllergeis();
            List<UserAllergyWithIsChecked> allergies = new List<UserAllergyWithIsChecked>();
            foreach (Allergy a in allAllergies)
            {
               UserAllergyWithIsChecked u1 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, false);
                allergies.Add(u1);
            }
            AllergiesList = new ObservableCollection<UserAllergyWithIsChecked>(allergies);
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
            Level newLevel = new Level();
            newLevel.TextLevel = "";
            newLevel.LevelCount = ListOfDirections.Count + 1;
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
                    for (int j = i ; j < ListOfDirections.Count; j++)
                    {
                        ListOfDirections[j].LevelCount--;                  
                    }
                }
            }
            Directions = new ObservableCollection<Level>(ListOfDirections);
            OnPropertyChanged("Directions");
        }

        public async void SaveRecipe()
        {
            //add try and catch
            SaveRecipeInfo saveRecipeInfo = new SaveRecipeInfo();
            Recipe newRecipe = new Recipe();
            newRecipe.RecipeImage = photoURL;
            newRecipe.RecipesName = recipeName;
            newRecipe.RecipeDescription = Desciption;
            if(Type == null)
            {
                newRecipe.Type = "Does not have";
            }
            else
            {
                newRecipe.Type = Type;
            }
            newRecipe.MadeBy = ((App)Application.Current).LoggedInUser.Id;
            newRecipe.Rating = 0;
            newRecipe.IsKosher = IsKosher;
            newRecipe.IsGloten = IsGloten;
            newRecipe.ContainsDairy = ContainDairy;
            newRecipe.ContainsMeat = ContainMeat;
            newRecipe.HowManyMadeIt = 0;
            if( Morning == true)
            {
                newRecipe.TimeOfDay = "Morning";
            }
            else if(Noon == true)
            {
                newRecipe.TimeOfDay = "Noon";
            }
            else if(Evening == true)
            {
                newRecipe.TimeOfDay = "Evening";
            }
            else
            {
                newRecipe.TimeOfDay = "Any Time";
            }
            List<Level> levels = new List<Level>();
            levels = ListOfDirections;
            List<IngredientRecipe> ingredientRecipes = new();
            foreach (IngredientsWithNameAndAmount i in ListOfNewIngredients)
            {
                ingredientRecipes.Add(new IngredientRecipe(i.IngredientId, i.RecipeId,(int)i.Amount , i.MeasureUnits));
            }
            List<Allergy> recipeAllergies = new();
            foreach(UserAllergyWithIsChecked a in AllergiesList)
            {
                if (a.IsChecked)
                {
                    Allergy allergy = new Allergy(a.AllergyId,a.AllergyName);
                    recipeAllergies.Add(allergy);
                }
            }
            saveRecipeInfo.RecipeInfo = newRecipe;
            saveRecipeInfo.RecipeInfo.Allergies = recipeAllergies;
            saveRecipeInfo.LevelsInfo = levels;
            saveRecipeInfo.IngredientsInfo = ingredientRecipes;
            saveRecipeInfo = await RecipesService.SaveRecipe(saveRecipeInfo);
            if (saveRecipeInfo != null)
            {
                OnPropertyChanged("ListOfAddedIngredient");
                OnPropertyChanged("ListOfDirections");
                ClearSort();
                //UPload profile imae if needed
                if (!string.IsNullOrEmpty(LocalPhotoPath))
                {

                    Recipe r = new(saveRecipeInfo.RecipeInfo);
                    r.RecipeImage = PhotoURL;
                    string updatedRecipe = await RecipesService.UploadRecipeImage(r);
                    if (updatedRecipe == null)
                    {
                        Refresh();
                        await Application.Current.MainPage.DisplayAlert("Registration", "Recipe Data Was Saved BUT recipe image upload failed", "ok");
                    }

                }
                Refresh();
                ((AppShell)Shell.Current).Refresh(typeof(HomePageViewModel));
                await Application.Current.MainPage.DisplayAlert("Registration", "Recipe Data Was Saved!", "ok");
                await AppShell.Current.GoToAsync("///HomePage");
            }
            else
            {

                await Application.Current.MainPage.DisplayAlert("Registration", "Recipe Data Was not Save, please try again", "ok");
            }

        }

        public override void Refresh()
        {
            IsKosher = false;
            IsGloten = false;
            ContainDairy = false;
            ContainMeat = false;
            RecipeName = "New Recipe";
            Desciption = "";
            Type = "";
            Morning = false;
            Noon = false;
            Evening = false;
            AnyTime = true;
            directions.Clear();
            ListOfNewIngredients.Clear();
            ListOfAddedIngredient.Clear();
            GetAllergies();
            photoURL = "";
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



    

