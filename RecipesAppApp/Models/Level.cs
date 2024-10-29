namespace RecipesAppApp.Models
{
    public class Level
    {
        public int Id { get; set; }

        public string TextLevel { get; set; } = null!;

        public int LevelCount { get; set; }

        public int RecipeId { get; set; }
    }
}
