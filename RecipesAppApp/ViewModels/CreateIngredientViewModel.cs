using RecipesAppApp.Classes;
using RecipesAppApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;


namespace RecipesAppApp.ViewModels
{
    public partial class StorageViewModel : ViewModelBase
    {
        //# region attributes and properties
        //#region Photo

        //private string imageResult;
        //public string ImageResult
        //{
        //    get => imageResult;
        //    set
        //    {
        //        imageResult = value;
        //        OnPropertyChanged("PhotoURL");
        //    }
        //}

        //private string photoURL;

        //public string PhotoURL
        //{
        //    get => photoURL;
        //    set
        //    {
        //        photoURL = value;
        //        OnPropertyChanged("PhotoURL");
        //    }
        //}

        //private string localPhotoPath;

        //public string LocalPhotoPath
        //{
        //    get => localPhotoPath;
        //    set
        //    {
        //        localPhotoPath = value;
        //        OnPropertyChanged("LocalPhotoPath");
        //    }
        //}

        //public Command UploadPhotoCommand { get; }
        ////This method open the file picker to select a photo
        //private async void OnUploadPhoto()
        //{
        //    try
        //    {
        //        var result = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
        //        {
        //            Title = "Please select a photo",
        //        });

        //        if (result != null)
        //        {
        //            // The user picked a file
        //            this.LocalPhotoPath = result.FullPath;
        //            this.PhotoURL = result.FullPath;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        //private void UpdatePhotoURL(string virtualPath)
        //{
        //    Random r = new Random();
        //    PhotoURL = RecipesService.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
        //    LocalPhotoPath = "";
        //}

        //#endregion
        //private RecipesAppWebAPIProxy RecipesService;
        //#endregion
        //public CreateIngredientViewModel(RecipesAppWebAPIProxy service)
        //{
        //    this.RecipesService = service;
        //    UploadPhotoCommand = new Command(OnUploadPhoto);
        //    PhotoURL = RecipesService.GetDefaultProfilePhotoUrl();
        //    LocalPhotoPath = "";
        //    ImageResult = "";
        //}

    }
}
