using System.ComponentModel.DataAnnotations;

namespace BookManagementApi_GO.Model.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int Views { get; set; } = 0;
    }
}
