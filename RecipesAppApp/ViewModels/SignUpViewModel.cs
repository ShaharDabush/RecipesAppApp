using RecipesAppApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RecipesAppApp.Views;
using RecipesAppApp.Services;
//using Android.Webkit;
//using Android.Webkit;

namespace RecipesAppApp.ViewModels
{

    public class SignUpViewModel : ViewModelBase
    {
        private RecipesAppWebAPIProxy proxy;
        public SignUpViewModel(RecipesAppWebAPIProxy proxy)
        {
            this.proxy = proxy;
            RegisterCommand = new Command(OnRegister);
            CancelCommand = new Command(OnCancel);
            ShowPasswordCommand = new Command(OnShowPassword);
            UploadPhotoCommand = new Command(OnUploadPhoto);
            PhotoURL = proxy.GetDefaultProfilePhotoUrl();
            LocalPhotoPath = "";
            ImageResult = "";
            IsPassword = true;
            NameError = "Name is required";
            EmailError = "Email is required";
            PasswordError = "Password must be at least 4 characters long and contain letters and numbers";
            StorageError = "Choose to create new storage or enter one";
            StorageNameError = "Storage Name is required";
            StorageCodeError = "Storage Code is required";
        }

        //Defiine properties for each field in the registration form including error messages and validation logic
        #region Name
        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                ValidateName();
                OnPropertyChanged("Name");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(Name);
        }
        #endregion
        #region Email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                ValidateEmail();
                OnPropertyChanged("Email");
            }
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!ShowEmailError)
            {
                //check if email is in the correct format using regular expression
                if (!System.Text.RegularExpressions.Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    EmailError = "Email is not valid";
                    ShowEmailError = true;
                }
                else
                {
                    EmailError = "";
                    ShowEmailError = false;
                }
            }
            else
            {
                EmailError = "Email is required";
            }
        }
        #endregion
        #region Password
        private bool showPasswordError;

        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidatePassword();
                OnPropertyChanged("Password");
            }
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private void ValidatePassword()
        {
            //Password must include characters and numbers and be longer than 4 characters
            if (string.IsNullOrEmpty(password) ||
                password.Length < 4 ||
                !password.Any(char.IsDigit) ||
                !password.Any(char.IsLetter))
            {
                this.ShowPasswordError = true;
            }
            else
                this.ShowPasswordError = false;
        }

        //This property will indicate if the password entry is a password
        private bool isPassword = true;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                isPassword = value;
                OnPropertyChanged("IsPassword");
            }
        }
        //This command will trigger on pressing the password eye icon
        public Command ShowPasswordCommand { get; }
        //This method will be called when the password eye icon is pressed
        public void OnShowPassword()
        {
            //Toggle the password visibility
            IsPassword = !IsPassword;
        }
        #endregion
        #region Photo

        private string imageResult;
        public string ImageResult
        {
            get => imageResult;
            set
            {
                imageResult = value;
                OnPropertyChanged("PhotoURL");
            }
        }

        private string photoURL;

        public string PhotoURL
        {
            get => photoURL;
            set
            {
                photoURL = value;
                OnPropertyChanged("PhotoURL");
            }
        }

        private string localPhotoPath;

        public string LocalPhotoPath
        {
            get => localPhotoPath;
            set
            {
                localPhotoPath = value;
                OnPropertyChanged("LocalPhotoPath");
            }
        }

        public Command UploadPhotoCommand { get; }
        //This method open the file picker to select a photo
        private async void OnUploadPhoto()
        {
            try
            {
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please select a photo",
                });

                if (result != null)
                {
                    // The user picked a file
                    this.LocalPhotoPath = result.FullPath;
                    this.PhotoURL = result.FullPath;
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void UpdatePhotoURL(string virtualPath)
        {
            Random r = new Random();
            PhotoURL = proxy.GetImagesBaseAddress() + virtualPath + "?v=" + r.Next();
            LocalPhotoPath = "";
        }

        #endregion
        #region Choose Storage


        private bool showStorageError;

        public bool ShowStorageError
        {
            get => showStorageError;
            set
            {
                showStorageError = value;
                OnPropertyChanged("ShowStorageError");
            }
        }

        private bool isNewStorage;

        public bool IsNewStorage
        {
            get => isNewStorage;
            set
            {
                isNewStorage = value;
                ValidateStorage();
                ValidateStorageCode();
                ValidateStorageName();
                StorageCode = "";
                OnPropertyChanged("IsNewStorage");
            }
        }

        private bool isCodeStorage;

        public bool IsCodeStorage
        {
            get => isCodeStorage;
            set
            {
                isCodeStorage = value;
                ValidateStorage();
                ValidateStorageCode();
                ValidateStorageName();
                StorageName = "";
                OnPropertyChanged("IsCodeStorage");
            }
        }

        private string storageError;

        public string StorageError
        {
            get => storageError;
            set
            {
                storageError = value;
                OnPropertyChanged("StorageError");
            }
        }
        private void ValidateStorage()
        {
            if(IsCodeStorage == false && IsNewStorage == false)
            {
                this.ShowStorageError = true;
            }
            else 
            {
                this.ShowStorageError = false;
            }
            
            
        }
        #endregion
        #region Storage Name

        private bool showStorageNameError;

        public bool ShowStorageNameError
        {
            get => showStorageNameError;
            set
            {
                showStorageNameError = value;
                OnPropertyChanged("ShowStorageNameError");
            }
        }

        private String storageName;

        public String StorageName
        {
            get => storageName;
            set
            {
                storageName = value;
                ValidateStorageName();
                OnPropertyChanged("StorageName");
            }
        }
        private string storageNameError;

        public string StorageNameError
        {
            get => storageNameError;
            set
            {
                storageNameError = value;
                OnPropertyChanged("StorageNameError");
            }
        }

        private void ValidateStorageName()
        {
            if((IsCodeStorage == false && IsNewStorage == false) || (IsCodeStorage == true))
            {
                this.ShowStorageNameError = false;
            }
            else
            {
                this.ShowStorageNameError = string.IsNullOrEmpty(StorageName);
            }
            
        }
        #endregion
        #region Storage Code

        private bool showStorageCodeError;

        public bool ShowStorageCodeError
        {
            get => showStorageCodeError;
            set
            {
                showStorageCodeError = value;
                OnPropertyChanged("ShowStorageCodeError");
            }
        } 

        private String storageCode;

        public String StorageCode
        {
            get => storageCode;
            set
            {
                storageCode = value;
                ValidateStorageCode();
                OnPropertyChanged("StorageCode");
            }
        }

        private string storageCodeError;

        public string StorageCodeError
        {
            get => storageCodeError;
            set
            {
                storageCodeError = value;
                OnPropertyChanged("StorageCodeError");
            }
        }
        private void ValidateStorageCode()
        {
            if ((IsCodeStorage == false && IsNewStorage == false) || (IsNewStorage == true))
            {
                this.ShowStorageCodeError = false;
            }
            else
            {
                this.ShowStorageCodeError = string.IsNullOrEmpty(StorageCode);
            }       
        }
        #endregion

        //Define a command for the register button
        public Command RegisterCommand { get; }
        public Command CancelCommand { get; }

        //Define a method that will be called when the register button is clicked
        public async void OnRegister()
        {
            ValidateName();
            ValidateEmail();
            ValidatePassword();
            ValidateStorage();
            ValidateStorageCode();
            ValidateStorageName();
            var newUser = new User { };
            var newStorage = new Storage { };
            bool IsNewStorage = false;
            if (!ShowNameError && !ShowEmailError && !ShowPasswordError && !ShowStorageError &&(!ShowStorageCodeError || !ShowStorageNameError))
            {
                if (StorageName != "" && StorageName != null)
                {
                    newStorage.StorageName = StorageName;
                    newStorage.StorageCode = "";
                    newStorage.Manager = newUser.Id;
                    newUser.StorageId = newStorage.Id;
                    IsNewStorage = true;
                }
                else
                {
                    IsNewStorage = false;
                    newStorage.StorageName = "" ;
                    newStorage.StorageCode = "";
                }
                newUser.UserName = Name;
                newUser.Email = Email;
                newUser.UserPassword= Password;
                newUser.IsAdmin = false;
                newUser.UserImage = PhotoURL;
                newUser.IsKohser = false;
                newUser.Vegetarianism = "None";
                RegisterInfo registerInfo = new RegisterInfo {UserInfo = newUser,StorageInfo = newStorage,StorageCodeInfo = StorageCode,IsNewStorage = IsNewStorage};

                //Create a new User object with the data from the registration form


                //Call the Register method on the proxy to register the new user
                InServerCall = true;
                registerInfo = await proxy.Register(registerInfo);
                InServerCall = false;

                //If the registration was successful, navigate to the login page
                if (registerInfo.UserInfo != null)
                {
                    //UPload profile imae if needed
                    if (!string.IsNullOrEmpty(LocalPhotoPath))
                    {
                        
                        LoginInfo li = new(newUser.Email, newUser.UserPassword);
                        await proxy.LoginAsync(li);
                        string updatedUser = await proxy.UploadUserImage(registerInfo.UserInfo);
                        if (updatedUser == null)
                        {
                            InServerCall = false;
                            await Application.Current.MainPage.DisplayAlert("Registration", "User Data Was Saved BUT Profile image upload failed", "ok");
                        }
                    }
                    InServerCall = false;

                    ((App)(Application.Current)).MainPage.Navigation.PopAsync();
                }
                else
                {

                    //If the registration failed, display an error message
                    string errorMsg = "Registration failed. Please try again.";
                    await Application.Current.MainPage.DisplayAlert("Registration", errorMsg, "ok");
                }
            }
        }

        //Define a method that will be called upon pressing the cancel button
        public void OnCancel()
        {
            //Navigate back to the login page
            ((App)(Application.Current)).MainPage.Navigation.PopAsync();
        }
    }
            //#endregion
        
    
}
