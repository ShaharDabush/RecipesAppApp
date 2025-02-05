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

namespace RecipesAppApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        #region attributes and properties
        private RecipesAppWebAPIProxy RecipesService;
        private User loggedUser;
        private ObservableCollection<User> usersWithSameStorage;
        private int recipesAmount;
        private int commentsAmount;
        private int ratingsAmount;
        public ICommand EditProfileCommand => new Command(GoToEditProfile);

        public User LoggedUser
        {
            get { return loggedUser; }
            set
            {
                this.loggedUser = value;
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
        public int CommentsAmount
        {
            get { return commentsAmount; }
            set
            {
                this.commentsAmount = value;
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
            GetsStats();
        }

        public async void GetsStats()
        {
            this.RecipesAmount = await RecipesService.GetRecipesAmountByUser(LoggedUser.Id);
            this.CommentsAmount = await RecipesService.GetCommentsAmountByUser(LoggedUser.Id);
            this.RatingsAmount = await RecipesService.GetRatingsAmountByUser(LoggedUser.Id);
            List<User> users = await RecipesService.GetUsersbyStorage(LoggedUser.Id);
            this.UsersWithSameStorage = new ObservableCollection<User>(users);

        }

        public async void GoToEditProfile()
        {
            await Shell.Current.GoToAsync("EditProfile"); 
        }
    }
}
