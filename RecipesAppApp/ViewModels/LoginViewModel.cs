﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RecipesAppApp.Services;
using RecipesAppApp.Models;
using RecipesAppApp.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace RecipesAppApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
            #region attributes and properties
            private RecipesAppWebAPIProxy RecipesService;
            private IServiceProvider serviceProvider;

        private string pass;
            public string Pass
            {
                get { return pass; }
                set
                {
                    pass = value;
                    OnPropertyChanged("Pass");
                }
            }

        private string mail;
            public string Mail
            {
                get { return mail; }
                set
                {
                    mail = value;
                    OnPropertyChanged("Mail");
                }
            }
        private bool inServerCall;
        public bool InServerCall
        {
            get
            {
                return this.inServerCall;
            }
            set
            {
                this.inServerCall = value;
                OnPropertyChanged("NotInServerCall");
                OnPropertyChanged("InServerCall");
            }
        }

        public bool NotInServerCall
        {
            get
            {
                return !this.InServerCall;
            }
        }

        #endregion


        //constractor
        //initialize the properties, attributes and commands
        public LoginViewModel(RecipesAppWebAPIProxy service, IServiceProvider serviceProvider)
        
        {
            //InServerCall = false;
                this.serviceProvider = serviceProvider;                
                this.RecipesService = service;
                this.LoginCommand = new Command(OnLogin);
                this.SignUpCommand = new Command(GoToSignUp);
                this.CancelCommand = new Command(OnCancel);
        }

            //command on pressing the login button
            public ICommand LoginCommand { get; set; }

            //command on pressing the signup button sends you to to SignUpView
            public Command SignUpCommand { protected set; get; }
            public Command CancelCommand { get; }


        //method
        //activated by the LoginCommand
        //checks with the servise if the given email and password match a user in the DB
        //and if so login to the user and go to profile page in the shell
        private async void OnLogin()
        {
            //Choose the way you want to blob the page while indicating a server call
            InServerCall = true;
            //await Shell.Current.GoToAsync("connectingToServer");
            LoginInfo loginInfo = new(mail, pass);
            User? u = await this.RecipesService.LoginAsync(loginInfo);
            //await Shell.Current.Navigation.PopModalAsync();
            InServerCall = false;

            //Set the application logged in user to be whatever user returned (null or real user)
            ((App)Application.Current).LoggedInUser = u;
            
            if (u == null)
            {
                await Application.Current.MainPage.DisplayAlert("Login", "Login failed!", "ok");
            }
            else
            {
                //ShellViewModel vm = (ShellViewModel)(((AppShell)Shell.Current).BindingContext);
                //vm.AdminPermission = IsAdmin(u);
                ((App)Application.Current).GetUserStorage();
                Mail = "";
                Pass = "";

                Shell.Current.Navigation.PopAsync();
                //((App)Application.Current).MainPage = serviceProvider.GetService<AppShell>();
                ShellViewModel vm = (ShellViewModel)(Shell.Current.BindingContext);
                vm.LogText = "Logout";
                vm.Refresh();
                ((AppShell)Shell.Current).Refresh(typeof(HomePageViewModel));

            }
        }

        //method
        //activated by SignUpCommand
        //send you to SignUpView
        private async void GoToSignUp()
        {
            //ErrorMsg = "";
            //Email = "";
            //Password = "";
            // Navigate to the Register View page
            ((App)Application.Current).MainPage.Navigation.PushAsync(serviceProvider.GetService<SignUpView>());

        }
        public void OnCancel()
        {
            //Navigate back to the login page
            ((App)(Application.Current)).MainPage.Navigation.PopAsync();
        }
        //private async Task NavToEditPage()
        //{
        //    Dictionary<string, object> data = new Dictionary<string, object>();
        //    data.Add("IsNotLogged", IsNotLogged);
        //    await AppShell.Current.GoToAsync("HomePageViewModel", data);
        //}
        private bool IsAdmin(User u)
        {
            if(u.IsAdmin.Value)
            {
                return true;
            }
            return false;
        }
    }
    
}
