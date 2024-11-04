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

namespace RecipesAppApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
            #region attributes and properties
            private RecipesAppWebAPIProxy RecipesService;
            private SignUpView signupView;

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
        public LoginViewModel(RecipesAppWebAPIProxy service, SignUpView signUp)
            {
                //InServerCall = false;
                this.signupView = signUp;
                this.RecipesService = service;
                this.LoginCommand = new Command(OnLogin);
                this.SignUpCommand = new Command(GoToSignUp);
            }

            //command on pressing the login button
            public ICommand LoginCommand { get; set; }

            //command on pressing the signup button sends you to to SignUpView
            public Command SignUpCommand { protected set; get; }

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
                    await Application.Current.MainPage.DisplayAlert("Login", $"Login Succeed!", "ok");
                    u = null;
                    Mail = "";
                    Pass = "";


                    Application.Current.MainPage = new AppShell(new ShellViewModel());

                }
            }

        //method
        //activated by SignUpCommand
        //send you to SignUpView
        private async void GoToSignUp()
        {
            await App.Current.MainPage.Navigation.PushAsync(signupView);

        }
    }
    
}
