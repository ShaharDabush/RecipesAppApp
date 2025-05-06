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

        public bool IsNotManager { get; set; }

        public bool? IsKohser { get; set; }

        public string? Vegetarianism { get; set; }

        public UsersWithManager() { }
        public UsersWithManager(User u , bool isNotManager) 
        {
            u.Id = Id;
            u.UserName = UserName;
            u.Email = Email;
            u.UserPassword = UserPassword;
            u.UserImage = UserImage;
            u.StorageId = StorageId;
            u.IsAdmin = IsAdmin;
            u.IsKohser = IsKohser;
            u.Vegetarianism = Vegetarianism;
            isNotManager = IsNotManager;

        }
    }
}
