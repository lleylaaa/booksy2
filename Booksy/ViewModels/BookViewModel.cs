using System.ComponentModel.DataAnnotations;

namespace Booksy.ViewModels
{
    public class BookViewModel
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "Titel is verplicht.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Auteur is verplicht.")]
        public string? Author { get; set; }

        public string? Genre { get; set; }

        [Range(1, 5, ErrorMessage = "Rating moet tussen 1 en 5 zijn.")]
        public int? Rating { get; set; }
    }
}