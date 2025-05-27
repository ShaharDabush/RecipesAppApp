using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Classes;

namespace RecipesAppApp.ViewModels
{
    public partial class EditProfileViewModel : ViewModelBase
    {
        public void ReadyToChooseManager()
        {
            List<UsersWithManager> usersWithoutManager = new List<UsersWithManager>();
            foreach(UsersWithManager u in UsersWithSameStorage)
            {
                if(u.Id != LoggedUserStorage.Manager)
                {
                    usersWithoutManager.Add(u);
                }
            }
            UsersWithSameStorage = new ObservableCollection<UsersWithManager>(usersWithoutManager);
        }

        public async void ChangeManager()
        {
            bool IsSuccessful = false;
            foreach(UsersWithManager u in UsersWithSameStorage)
            {
                if (u.IsNewManager == true)
                {
                    IsSuccessful =  await RecipesService.ChangeManager(u.Id);
                }
            }
            if (IsSuccessful == true)
            { 
                await this.RecipesService.RemoveMember(LoggedUser.Id);
                await AppShell.Current.GoToAsync("///HomePage");
                IsInStorage = false;
            }
            else
            {
                await AppShell.Current.DisplayAlert("Error", "you did not choose new manager, please try again", "OK");
            }
            GetusersList();
        }
    }
}
