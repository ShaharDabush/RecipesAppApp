namespace RecipesAppApp.Models
{
    public class User
    {
         public required int  Id { get; set; }

        public required string UserName { get; set; } = null!;

        public required string Email { get; set; } = null!;

        public required string UserPassword { get; set; } = null!;

        public required string? UserImage { get; set; }

        public required int? StorageId { get; set; }
    }
}
