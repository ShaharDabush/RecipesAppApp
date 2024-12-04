using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.ViewModels;
using RecipesAppApp.Services;
using RecipesAppApp.Models;
using RecipesAppApp.Views;
using System.Runtime.CompilerServices;

namespace RecipesAppApp.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        //this page is just for a log out command and not showing views if you dont have permission
        #region attributes and paramaters
        private bool isMaster;
        private bool isAdmin;

        public bool IsAdmin
        {
            get
            {
                if ((App)Application.Current == null)
                    return true;
                else if (((App)Application.Current).LoggedInUser.IsAdmin > 1)
                    return true;
                else
                    return false;
            }
        }
        //this command will be used for logout menu item
        public ICommand LogoutCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        private User? currentUser;
        private bool adminPermission;
        private RecipesAppWebAPIProxy RecipesService;
        public bool AdminPermission
        {
            get => adminPermission;
            set
            {
                adminPermission = value;
                OnPropertyChanged("AdminPermission");
            }
        }
        #endregion

        //constractor
        //initilizing the logout command

        public ShellViewModel()
        {
            AdminPermission = false;
            this.LogoutCommand = new Command(OnLogout);
            this.LoginCommand = new Command(OnLogIn);
            this.SignUpCommand = new Command(OnSignUp);
            this.currentUser = ((App)Application.Current).LoggedInUser;
            
        }

        //on pressing logout in the shell bar on the left
        //on LogoutCommand
        //clear the current user and send them to the login screen exiting the shell
        public async void OnLogout()
        {
            ((App)Application.Current).LoggedInUser = null;
            ((App)Application.Current).MainPage = ((App)Application.Current).Login;
        }
        public async void OnLogIn()
        {
            ((App)Application.Current).MainPage = ((App)Application.Current).Login;
        }

        private async void OnSignUp()
        {
            ((App)Application.Current).MainPage = ((App)Application.Current).SignUp;

        }
        
    }
}
