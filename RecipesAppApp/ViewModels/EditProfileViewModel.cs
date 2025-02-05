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


namespace RecipesAppApp.ViewModels
{
    public class EditProfileViewModel : ViewModelBase
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
        private User loggedUser;
        
        private ObservableCollection<User> usersWithSameStorage;
        public ICommand DiscardMembersCommand;
        private bool isNotAdmin;

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

        public EditProfileViewModel(RecipesAppWebAPIProxy service)
        {
            this.RecipesService = service;
            LoggedUser = ((App)Application.Current).LoggedInUser;
            LoggedUserStorage = ((App)Application.Current).UserStorage;
            this.Name = LoggedUser.UserName;
            this.Email = LoggedUser.Email;
            DiscardMembersCommand = new Command<int>(RemoveMembers);
            GetusersList();
            IsNotAdmin = !loggedUser.IsAdmin.Value;
            StorageName = LoggedUserStorage.StorageName;
        }
        public async void GetusersList()
        {
            List<User> users = await RecipesService.GetUsersbyStorage(LoggedUser.Id);
            this.UsersWithSameStorage = new ObservableCollection<User>(users);

        }
        public async void ChangeName()
        {
            bool isChanged;
            ValidateName();
            if (! ShowNameError)
            {
                LoggedUser.UserName = Name;
               isChanged = await this.RecipesService.ChangeName(LoggedUser);
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
                isChanged = await this.RecipesService.ChangeName(LoggedUser);
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

            User user = new User();
            foreach(User u in UsersWithSameStorage)
            {
                if (u.Id == Id)
                {
                    user = u;
                }
            }
            if (user != null)
            {
                isChanged = await this.RecipesService.RemoveMember(user);
                if (!isChanged)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Try again later", "ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Remove User", $"You Removed the user seccesfully!", "ok");
                }
            }
        }
    }
}
