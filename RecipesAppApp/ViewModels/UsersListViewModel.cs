﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using System.Windows.Input;
using RecipesAppApp.Classes;
using Android.App;

namespace RecipesAppApp.ViewModels
{
    public class UsersListViewModel: ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private bool isRefreshing;
        public ICommand RefreshCommand => new Command(Refresh);
        private ObservableCollection<FullUserForList> userList;
        private ObservableCollection<FullUserForList> allUsers;
        private ObservableCollection<FullUserForList> usersForList;
        private string searchedName;
        private int recipesAmount;

        public int RecipesAmount
        {
            get { return recipesAmount; }
            set
            {
                this.recipesAmount = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FullUserForList> UsersForList
        {
            get { return usersForList; }
            set
            {
                this.usersForList = value;
                OnPropertyChanged();
            }
        }
        public string SearchedName
        {
            get { return searchedName; }
            set
            {
                this.searchedName = value;
                OnPropertyChanged();
                Sort();
            }
        }
        public ObservableCollection<FullUserForList> UserList
        {
            get { return userList; }
            set
            {
                this.userList = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FullUserForList> AllUsers
        {
            get { return allUsers; }
            set
            {
                this.allUsers = value;
                OnPropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshing;
            }
            set
            {
                this.isRefreshing = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public UsersListViewModel(RecipesAppWebAPIProxy service)
        {
            RecipesService = service;
            GetUsers();
        }

        public async void GetUsers()
        {
            List<User> u = await RecipesService.GetAllUsers();
            UserList = new ObservableCollection<FullUserForList>();
            foreach (User user in u) 
            {
                this.RecipesAmount = await RecipesService.GetRecipesAmountByUser(user.Id);
                UserList.Add(new FullUserForList(user,RecipesAmount));
            }
            this.AllUsers = new ObservableCollection<FullUserForList>(UserList);
        }


        public void Sort()
        {
            if (string.IsNullOrEmpty(SearchedName))
            {
                ClearSort();
            }
            else 
            {
                List<FullUserForList> temp = UserList.Where(u => u.UserName.ToLower().Contains(SearchedName.ToLower())).ToList();
                this.AllUsers.Clear();
                foreach (FullUserForList r in temp)
                {
                    this.AllUsers.Add(r);
                }
                //this.IsLogged = false;
            }
        }

        public void ClearSort()
        {
            if (!string.IsNullOrEmpty(SearchedName))
                this.SearchedName = null;
            this.AllUsers = UserList;
        }

        public override void Refresh()
        {
            GetUsers();
            OnPropertyChanged("AllUsers");
        }
    }
}
