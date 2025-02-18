using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipesAppApp.Models;
namespace RecipesAppApp.Classes
{
    public class FullUserForList
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string? UserImage { get; set; }

        public int? StorageId { get; set; }

        public bool? IsAdmin { get; set; }

        public bool? IsNotAdmin { get => !IsAdmin; }

        public int RecipeAmout { get; set; }

        public FullUserForList() { }

        public FullUserForList(User user, int recipeAmout) 
        { 
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            UserPassword = user.UserPassword;
            UserImage = user.UserImage;
            StorageId = user.StorageId;
            IsAdmin = user.IsAdmin;
            RecipeAmout = recipeAmout;
        }
    }
}
