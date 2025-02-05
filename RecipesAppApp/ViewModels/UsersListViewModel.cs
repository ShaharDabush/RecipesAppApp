using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipesAppApp.Models;
using RecipesAppApp.Services;
using System.Windows.Input;

namespace RecipesAppApp.ViewModels
{
    public class UsersListViewModel: ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private bool isRefreshing;
        public ICommand RefreshCommand => new Command(Refresh);
        private ObservableCollection<User> userList;
        public ObservableCollection<User> UserList
        {
            get { return userList; }
            set
            {
                this.userList = value;
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
            this.UserList = new ObservableCollection<User>(u);
        }
       
        private async void Refresh()
        {
            GetUsers();
        }
    }
}
