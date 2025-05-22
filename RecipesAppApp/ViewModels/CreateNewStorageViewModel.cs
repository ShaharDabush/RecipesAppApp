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
            Storage newStorage = new Storage();
            if(StorageNewName == null || StorageNewName == "")
            {
                newStorage.StorageName = LoggedUser.UserName;
                newStorage.StorageName += "'s Storage";
            }
            else
            {
                newStorage.StorageName = StorageNewName;
            }
            newStorage.Manager = LoggedUser.Id;
           newStorage.StorageCode = "";
            int? NewStorageId = 0;
            NewStorageId = await RecipesService.SaveNewStorage(newStorage);
            if (NewStorageId != null)
            {
                ((App)Application.Current).LoggedInUser.StorageId = NewStorageId;
                ((App)Application.Current).Refresh();
                await AppShell.Current.GoToAsync("///Storage");
            }
            else
            {
                await AppShell.Current.DisplayAlert("Error", "Failed to create storage", "OK");
                await AppShell.Current.GoToAsync("///HomePage");
            }
        }

        public async void Cancel()
        {
            await AppShell.Current.GoToAsync("///HomePage");
        }
    }
}
