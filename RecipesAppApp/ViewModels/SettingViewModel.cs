using AndroidX.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using Android.Service.Voice;
using System.Windows.Input;

namespace RecipesAppApp.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        #region attributes and properties

        private RecipesAppWebAPIProxy RecipesService;
        public Command SaveCommand { get; }
        private bool none;
        private bool vegetarian;
        private bool vegan;
        private ObservableCollection<Allergy> allergies;
        private bool? isKosher;
        private string vegetarianism;
        private User loggedUser;
        private List<Allergy> usersAllergy = new List<Allergy>();
        private List<Allergy> allAllergies = new List<Allergy>();
        private ObservableCollection<UserAllergyWithIsChecked> hasAllergy = new ObservableCollection<UserAllergyWithIsChecked>();

        public ObservableCollection<Allergy> Allergies
        {
            get { return allergies; }
            set
            {
                this.allergies = value;
                OnPropertyChanged();
            }
        }
        public List<Allergy> UsersAllergy
        {
            get { return usersAllergy; }
            set
            {
                this.usersAllergy = value;
                OnPropertyChanged();
            }
        }
        public List<Allergy> AsersAllergy
        {
            get { return allAllergies; }
            set
            {
                this.allAllergies = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserAllergyWithIsChecked> HasAllergy
        {
            get { return hasAllergy; }
            set
            {
                this.hasAllergy = value;
                OnPropertyChanged();
            }
        }
        public User LoggedUser
        {
            get { return loggedUser; }
            set
            {
                this.loggedUser = value;
                OnPropertyChanged();
            }
        }
        public bool? IsKosher
        {
            get { return isKosher; }
            set
            {
                this.isKosher = value;
                OnPropertyChanged();
            }
        }
        public bool None
        {
            get { return none; }
            set
            {
                this.none = value;
                OnPropertyChanged();
            }
        }
        public bool Vegetarian
        {
            get { return vegetarian; }
            set
            {
                this.vegetarian = value;
                OnPropertyChanged();
            }
        }
        public bool Vegan
        {
            get { return vegan; }
            set
            {
                this.vegan = value;
                OnPropertyChanged();
            }
        }
        public string Vegetarianism
        {
            get { return vegetarianism; }
            set
            {
                this.vegetarianism = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public SettingViewModel(RecipesAppWebAPIProxy service)
        {
            RecipesService = service;
            loggedUser = ((App)Application.Current).LoggedInUser;
            Vegetarianism =  ((App)Application.Current).LoggedInUser.Vegetarianism;
            IsKosher = ((App)Application.Current).LoggedInUser.IsKohser;
            SaveCommand = new Command(SaveAllergy);
            GetAllergies();
            SetSetting();
        }
        public async void GetAllergies()
        {
            List<Allergy> allAllergies = await RecipesService.GetAllAllergeis();
            Allergies = new ObservableCollection<Allergy>(allAllergies);
        }

        public async void SaveAllergy()
        {
            LoggedUser.IsKohser = IsKosher;
            if (None == true)
            {
                LoggedUser.Vegetarianism = "None";
            }
            else if (Vegetarian == true)
            {
                LoggedUser.Vegetarianism = "Vegetarian";
            }
            else if (Vegan == true)
            {
                LoggedUser.Vegetarianism = "Vegan";
            }
            List<Allergy> SaveAllergy = new List<Allergy>();
            foreach(UserAllergyWithIsChecked a in HasAllergy)
            {
                if(a.IsChecked == true)
                {
                Allergy al = new Allergy(a.AllergyId,a.AllergyName);
                SaveAllergy.Add(al);
                }
            }
            bool update = await RecipesService.UpdateUser(LoggedUser);
            bool saveAllergy = await RecipesService.SaveAllergy(SaveAllergy, LoggedUser.Id);
            if(update != true || saveAllergy != true)
            {
                await Application.Current.MainPage.DisplayAlert("Allergies", "Update failed!", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Allergies", "Update succeeded!", "ok");
            }

        }

        public async void SetSetting()
        {
            None = false;
            Vegetarian = false;
            Vegan = false;
            if (Vegetarianism == "None")
            {
                None = true;
            }
            else if (Vegetarianism == "Vegetarian")
            {
                Vegetarian = true;
            }
            else if (Vegetarianism == "Vegan")
            {
                Vegan = true;
            }
            List<Allergy> UsersAllergy = await RecipesService.GetAllergiesByUser(LoggedUser.Id);
            foreach(Allergy a in Allergies)
            {
                bool b = false;
                foreach(Allergy au in UsersAllergy)
                {
                    if(a.Id == au.Id)
                    {
                        UserAllergyWithIsChecked u1 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, true);
                        HasAllergy.Add(u1);
                        b = true;
                    }
                }
                UserAllergyWithIsChecked u2 = new UserAllergyWithIsChecked(a.Id, a.AllergyName, false);
                if (!b)
                    HasAllergy.Add(u2);
            }
        }

    }

}
