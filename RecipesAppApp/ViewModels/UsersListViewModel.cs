using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RecipesAppApp.Models;
using RecipesAppApp.Services;

namespace RecipesAppApp.ViewModels
{
    public class UsersListViewModel: ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
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
    }
}
