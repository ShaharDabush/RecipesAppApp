namespace RecipesAppApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Comment1 { get; set; } = null!;

        public int UserId { get; set; }

        public int RecipeId { get; set; }
    }
}
