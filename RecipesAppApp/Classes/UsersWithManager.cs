using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;

namespace RecipesAppApp.Classes
{
    public class UsersWithManager
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string? UserImage { get; set; }

        public int? StorageId { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsNotAdmin { get => !IsAdmin; }

        public bool IsManager { get; set; }

        public bool IsNotManager { get; set; }

        public bool IsLoggedUser { get; set; }

        public bool? IsKohser { get; set; }

        public string? Vegetarianism { get; set; }

        public bool? IsNewManager { get; set; }


        public UsersWithManager() { }
        public UsersWithManager(User u , bool isManager, bool isNotManager, bool isLoggedUser, bool? isNewManager)
        {
            Id = u.Id;
            UserName = u.UserName;
            Email = u.Email;
            UserPassword = u.UserPassword;
            UserImage = u.UserImage;
            StorageId = u.StorageId;
            IsAdmin = u.IsAdmin;
            IsKohser = u.IsKohser;
            Vegetarianism = u.Vegetarianism;
            IsManager = isManager;
            IsNotManager = isNotManager;
            IsLoggedUser = isLoggedUser;
            IsNewManager = isNewManager;
        }
    }
}
