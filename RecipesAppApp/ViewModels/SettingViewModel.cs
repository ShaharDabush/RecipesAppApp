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

namespace RecipesAppApp.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        #region attributes and properties

        private RecipesAppWebAPIProxy RecipesService;
        public Command SaveCommand { get; }
        private ObservableCollection<Allergy> allergies;
        private bool isKosher;
        private string vegetarianism;
        private User loggedUser;

        public ObservableCollection<Allergy> Allergies
        {
            get { return allergies; }
            set
            {
                this.allergies = value;
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
        public bool IsKosher
        {
            get { return isKosher; }
            set
            {
                this.isKosher = value;
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
            Vegetarianism = "None";
            SaveCommand = new Command(SaveAllergy);
            GetAllergies();
        }

        public async void GetAllergies()
        {
            List<Allergy> allAllergies = await RecipesService.GetAllAllergeis();
            Allergies = new ObservableCollection<Allergy>(allAllergies);
        }

        public async void SaveAllergy()
        {

        }

    }

}
