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
using Microsoft.Extensions.DependencyInjection;

namespace RecipesAppApp.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        //this page is just for a log out command and not showing views if you dont have permission
        #region attributes and paramaters
        private bool isMaster;
        private bool isAdmin;
        private IServiceProvider serviceProvider;
        private bool isNotLogged;
        public bool IsNotLogged
        {
            get { return !IsLogged; }
        }
        private bool isLogged;
        public bool IsLogged
        {
            get { return ((App)Application.Current).LoggedInUser != null; }
            
        }

        public bool IsAdmin
        {
            get
            {
                if (!IsLogged)
                    return false;
                else if (((App)Application.Current).LoggedInUser.IsAdmin.Value)
                    return true;
                else
                    return false;
            }
        }
        //this command will be used for logout menu item
        public ICommand LogCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        private User? currentUser;
        private bool adminPermission;
        private RecipesAppWebAPIProxy RecipesService;
        
        #endregion

        //constractor
        //initilizing the logout command

        public ShellViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.LogCommand = new Command(OnLog);
            this.SignUpCommand = new Command(OnSignUp);
            this.currentUser = ((App)Application.Current).LoggedInUser;
            LogText = "Login";
            
        }

        //on pressing logout in the shell bar on the left
        //on LogoutCommand
        //clear the current user and send them to the login screen exiting the shell
        public async void OnLog()
        {
            if (IsLogged)
            {
                ((App)Application.Current).LoggedInUser = null;
                ((App)Application.Current).UserStorage = null;                
                //LogText = "Login";
                //Refresh();
                await ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<LoadingPageView>());
            }
            else
            {
                OnLogIn();
                Refresh();
            }
            

        }

        private string logText;
        public string LogText
        {
            get => this.logText;
            set
            {
                this.logText = value;
                OnPropertyChanged("LogText");
            }
        }
        public async void OnLogIn()
        {
            await Shell.Current.GoToAsync("Login");
            Shell.Current.FlyoutIsPresented = false;
        }

        private async void OnSignUp()
        {
            await Shell.Current.GoToAsync("SignUp");

        }

        public void Refresh()
        {
            OnPropertyChanged("IsNotLogged");
            OnPropertyChanged("IsLogged");
            OnPropertyChanged("IsAdmin");
            OnPropertyChanged("LogText");
        }
        
    }
}
