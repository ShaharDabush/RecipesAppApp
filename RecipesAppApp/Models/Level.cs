namespace RecipesAppApp.Models
{
    public class Level
    {
        public int Id { get; set; }

        public string TextLevel { get; set; } = null!;

        public int LevelCount { get; set; }

        public int RecipeId { get; set; }

        public Level (int id , string textLevel, int levelCount, int recipeId)
        {
            Id = id;
            TextLevel = textLevel;
            LevelCount = levelCount;
            RecipeId = recipeId;
        }
        public Level(Level l)
        {
            Id = l.Id;
            TextLevel = l.TextLevel;
            LevelCount = l.LevelCount;
            RecipeId = l.RecipeId;
        }
    }
}
