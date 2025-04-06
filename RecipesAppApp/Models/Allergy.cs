namespace RecipesAppApp.Models
{
    public class Allergy
    {
        public int Id { get; set; }

        public string AllergyName { get; set; } = null!;

        public Allergy(int id, string allergyName)
        {
            Id = id;
            AllergyName = allergyName;
        }
    }
}
