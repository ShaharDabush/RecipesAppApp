﻿namespace RecipesAppApp.Models
{
    public class User
    {
        public int  Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string UserPassword { get; set; } = null!;

        public string? UserImage { get; set; }

        public int? StorageId { get; set; }
        public int? IsAdmin { get; set; }

        public User() { }
    }
}
