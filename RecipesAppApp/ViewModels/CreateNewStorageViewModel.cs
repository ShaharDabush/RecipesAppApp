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
    public partial class StorageViewModel : ViewModelBase
    {
        #region attributes and properties
        public ICommand SaveStorageCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private string storageNewName;
        private string newStorageCode;
        public string NewStorageCode
        {
            get
            {
                return this.newStorageCode;
            }
            set
            {
                this.newStorageCode = value;
                OnPropertyChanged();
            }
        }
        public string StorageNewName
        {
            get
            {
                return this.storageNewName;
            }
            set
            {
                this.storageNewName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public async void SaveStorage()
        {
            // Create a new storage object
            
            if(StorageNewName == null && NewStorageCode == null)
            {
                await AppShell.Current.DisplayAlert("Error", "both entries cannot be empty", "OK");
                return;
            }
            if (NewStorageCode == null)
            {
            Storage newStoraged = new Storage();
            if(StorageNewName == "")
            {
                newStoraged.StorageName = LoggedUser.UserName;
                newStoraged.StorageName += "'s Storage";
            }
            else
            {
                newStoraged.StorageName = StorageNewName;
            }
            newStoraged.Manager = LoggedUser.Id;
            newStoraged.StorageCode = "";
            int? NewStorageId = 0;
            NewStorageId = await RecipesService.SaveNewStorage(newStoraged);
            if (NewStorageId != null)
            {
                ((App)Application.Current).LoggedInUser.StorageId = NewStorageId;
                ((App)Application.Current).Refresh();
                    StorageNewName = "";
                    NewStorageCode = "";
                    await AppShell.Current.GoToAsync("///Storage");
            }
            else
            {
                    StorageNewName = "";
                    NewStorageCode = "";
                    await AppShell.Current.DisplayAlert("Error", "Failed to create storage", "OK");
                await AppShell.Current.GoToAsync("///HomePage");
            }
            }
            else
            {
                int NewStorageId = await RecipesService.EnterNewStorage(NewStorageCode, LoggedUser.Id);
                if(NewStorageId != 0)
                {
                    ((App)Application.Current).LoggedInUser.StorageId = NewStorageId;
                    Storage = await RecipesService.GetStoragesbyUser(loggedUser.Id);
                    ((App)Application.Current).UserStorage = Storage;
                    ((App)Application.Current).Refresh();
                    StorageName = ((App)Application.Current).UserStorage.StorageName;
                    SetUserIngredients();
                    StorageNewName = "";
                    NewStorageCode = "";
                    await AppShell.Current.GoToAsync("///Storage");
                }
                else
                {
                    StorageNewName = "";
                    NewStorageCode = "";
                    await AppShell.Current.DisplayAlert("Error", "Failed to connect to the new storage", "OK");
                    await AppShell.Current.GoToAsync("///HomePage");
                }
            }
        }

        public async void Cancel()
        {
            await AppShell.Current.GoToAsync("///HomePage");
        }
    }
}
