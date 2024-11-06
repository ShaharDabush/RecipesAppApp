namespace RecipesAppApp.Models
{
    public class Storage
    {
        public int Id { get; set; }

        public string StorageName { get; set; } = null!;

        public string StorageCode { get; set; } = null!;

        public int Manager { get; set; }

        public Storage() { } 
    }
}
