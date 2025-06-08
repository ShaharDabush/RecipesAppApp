using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using System.Collections.ObjectModel;

using System.Windows.Input;
using System.Security.Cryptography.X509Certificates;

namespace RecipesAppApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private User loggedUser;
        private Storage loggedUserStorage;
        private ObservableCollection<User> usersWithSameStorage;
        private int recipesAmount;
        private int ratingsAmount;
        private string loggedUserStorageCode;
        private bool isHasStorage;
        public ICommand EditProfileCommand => new Command(GoToEditProfile);


        public bool IsHasStorage
        {
            get { return isHasStorage; }
            set
            {
                this.isHasStorage = value;
                OnPropertyChanged();
            }
        }
        public string LoggedUserStorageCode
        {
            get { return loggedUserStorageCode; }
            set
            {
                this.loggedUserStorageCode = value;
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
        public Storage LoggedUserStorage
        {
            get { return loggedUserStorage; }
            set
            {
                this.loggedUserStorage = value;
                OnPropertyChanged();
            }
        }


        public int RecipesAmount
        {
            get { return recipesAmount; }
            set
            {
                this.recipesAmount = value;
                OnPropertyChanged();
            }
        }
        public int RatingsAmount
        {
            get { return ratingsAmount; }
            set
            {
                this.ratingsAmount = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<User> UsersWithSameStorage
        {
            get { return usersWithSameStorage; }
            set
            {
                this.usersWithSameStorage = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ProfileViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            LoggedUser = ((App)Application.Current).LoggedInUser;
            if(((App)Application.Current).UserStorage == null)
            {
                IsHasStorage = false;
            }
            else
            {
                LoggedUserStorage = ((App)Application.Current).UserStorage;
                LoggedUserStorageCode = LoggedUserStorage.StorageCode;
                IsHasStorage = true;
            }            
            GetsStats();
        }

        public async void GetsStats()
        {
            this.RecipesAmount = await RecipesService.GetRecipesAmountByUser(LoggedUser.Id);
            this.RatingsAmount = await RecipesService.GetRatingsAmountByUser(LoggedUser.Id);
            List<User> users = await RecipesService.GetUsersbyStorage(LoggedUser.Id);
            this.UsersWithSameStorage = new ObservableCollection<User>(users);

        }

        public async void IsStorageNullInitData()
        {
             if (LoggedUser.StorageId == null)
             {
                IsHasStorage = false;
            }
             else
             {
                LoggedUserStorage = ((App)Application.Current).UserStorage;
                LoggedUserStorageCode = LoggedUserStorage.StorageCode;
                IsHasStorage = true;
             }
             GetsStats();
        }

        public async void GoToEditProfile()
        {
            await Shell.Current.GoToAsync("EditProfile"); 
        }

        public override void Refresh()
        {
            GetsStats();
            OnPropertyChanged("UsersWithSameStorage");
        }
    }
}
