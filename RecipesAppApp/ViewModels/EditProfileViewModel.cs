using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
using RecipesAppApp.Classes;
using RecipesAppApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Maui;
using Microsoft.Extensions.DependencyInjection;
using RecipesAppApp.Views;


namespace RecipesAppApp.ViewModels
{
    public partial class EditProfileViewModel : ViewModelBase
    {
        #region attributes and properties
        #region ValidationForm
        public ICommand NameCommand => new Command(ChangeName);
        public ICommand MailCommand => new Command(ChangeMail);
        public ICommand StorageNameCommand => new Command(ChangeStorageName);
        #region Name
        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                ValidateName();
                OnPropertyChanged("Name");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(Name);
        }
        #endregion
        #region Email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                ValidateEmail();
                OnPropertyChanged("Email");
            }
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!ShowEmailError)
            {
                //check if email is in the correct format using regular expression
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    EmailError = "Email is not valid";
                    ShowEmailError = true;
                }
                else
                {
                    EmailError = "";
                    ShowEmailError = false;
                }
            }
            else
            {
                EmailError = "Email is required";
            }
        }
        #endregion
        #region StorageName
        private string storageName;
        public string StorageName
        {
            get { return storageName; }
            set
            {
                this.storageName = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion
        private RecipesAppWebAPIProxy RecipesService;
        private IServiceProvider serviceProvider;
        private User loggedUser;
        public event Action<List<string>> OpenPopup;
        private ObservableCollection<UsersWithManager> usersWithSameStorage;
        public ICommand DiscardMembersCommand { get; set; }
        public ICommand SaveNewManagerCommand { get; set; }
        public ICommand LeaveStorageCommand { get; set; }
        private bool isNotAdmin;
        private bool isInStorage;

        public bool IsInStorage
        {
            get { return isInStorage; }
            set
            {
                this.isInStorage = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotAdmin
        {
            get { return isNotAdmin; }
            set
            {
                this.isNotAdmin = value;
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
        private Storage loggedUserStorage;
        public Storage LoggedUserStorage
        {
            get { return loggedUserStorage; }
            set
            {
                this.loggedUserStorage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UsersWithManager> UsersWithSameStorage
        {
            get { return usersWithSameStorage; }
            set
            {
                this.usersWithSameStorage = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public EditProfileViewModel(RecipesAppWebAPIProxy service, IServiceProvider sp)
        {
            this.RecipesService = service;
            serviceProvider = sp;
            LoggedUser = ((App)Application.Current).LoggedInUser;
            LoggedUserStorage = ((App)Application.Current).UserStorage;
            if(LoggedUserStorage == null)
            {
                IsInStorage = false;
            }
            else
            {
                StorageName = LoggedUserStorage.StorageName;
                IsInStorage = true;
            }
            this.Name = LoggedUser.UserName;
            this.Email = LoggedUser.Email;
            DiscardMembersCommand = new Command<int>((int Id) => RemoveMembers(Id));
            LeaveStorageCommand = new Command(LeaveStorage);
            SaveNewManagerCommand = new Command(ChangeManager);
            GetusersList();
            IsNotAdmin = !loggedUser.IsAdmin.Value;

        }
        public async void GetusersList()
        {
            List<User> users = await RecipesService.GetUsersbyStorage(LoggedUser.Id);
            List<UsersWithManager> usersWithManager = new List<UsersWithManager>();
            if(LoggedUserStorage != null)
            {
            foreach (User user in users) 
            {
                UsersWithManager u = new UsersWithManager();
                if(LoggedUserStorage.Manager == user.Id)
                {
                     u = new UsersWithManager(user, true, false, false,false);
                    usersWithManager.Add(u);
                }
                else if(loggedUser.Id == LoggedUserStorage.Manager)
                {
                     u = new UsersWithManager(user, false, true, false,false);
                    usersWithManager.Add(u);
                }
                else
                {
                     u = new UsersWithManager(user, false, false, false, false);
                    usersWithManager.Add(u);
                }

                if(loggedUser.Id == u.Id && u.Id == LoggedUserStorage.Manager)
                {
                    u.IsLoggedUser = true;
                    u.IsManager = false;
                }
                else if(loggedUser.Id == u.Id)
                {
                    u.IsLoggedUser = true;
                }
            }
            this.UsersWithSameStorage = new ObservableCollection<UsersWithManager>(usersWithManager);
            }

        }
        public async void ChangeName()
        {
            bool isChanged;
            ValidateName();
            if (! ShowNameError)
            {
                LoggedUser.UserName = Name;
               isChanged = await this.RecipesService.UpdateUser(LoggedUser);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("UpdateUser", $"Update seccesful! please open the profile page again to see your new details!", "ok");
                }
            }

        }
        
        public async void ChangeMail()
        {
            bool isChanged;
            ValidateEmail();
            if (!ShowEmailError)
            {
                LoggedUser.Email = Email;
                isChanged = await this.RecipesService.UpdateUser(LoggedUser);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("UpdateUser", $"Update seccesful! please open the profile page again to see your new details!", "ok");
                }
            }
        }
        public async void ChangeStorageName()
        {
            bool isChanged;
            if (StorageName != null)
            {
                LoggedUserStorage.StorageName = StorageName;
                isChanged = await this.RecipesService.ChangeStorageName(LoggedUserStorage);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("UpdateUser", $"Update seccesful! please open the profile page again to see your new details!", "ok");
                }
            }
        }
        
        public async void RemoveMembers(int Id)
        {
            bool isChanged;
            if (Id != 0)
            {
                isChanged = await this.RecipesService.RemoveMember(Id);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                GetusersList();
            }
        }
        public async void LeaveStorage()
        {
           bool isChanged = false;
           if((LoggedUserStorage.Manager == LoggedUser.Id) && (UsersWithSameStorage.Count > 1))
           {
                //await Application.Current.MainPage.DisplayAlert("Error", "You are the manager of the storage, please assign another manager before leaving", "ok");
                //isChanged = await this.RecipesService.RemoveMember(LoggedUser.Id);
              ReadyToChooseManager();
              if (OpenPopup != null)
              {
                  List<string> l = new List<string>();
                  OpenPopup(l);
              }
           }
           else
           {
                if((LoggedUserStorage.Manager == LoggedUser.Id))
                {
                    isChanged = await this.RecipesService.RemoveMember(LoggedUser.Id);
                    await this.RecipesService.DeleteStorage(LoggedUserStorage);
                    ((App)Application.Current).LoggedInUser.StorageId = null;
                    ((App)Application.Current).Refresh();
                }
                else
                {
                    isChanged = await this.RecipesService.RemoveMember(LoggedUser.Id);
                    ((App)Application.Current).LoggedInUser.StorageId = null;
                    ((App)Application.Current).Refresh();
                }
                if (!isChanged)
                {
                   await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                else
                {
                 await AppShell.Current.GoToAsync("///HomePage");
                }
                IsInStorage = false;
                GetusersList();
           }
        }
    }
}
