namespace RecipesAppApp.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public double Rate { get; set; }

        public int UserId { get; set; }

        public int RecipeId { get; set; }
    }
}
