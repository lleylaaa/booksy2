using System;
using System.ComponentModel.DataAnnotations;

namespace Booksy.ViewModels
{
    public class ReviewViewModel
    {
        public int ReviewID { get; set; }
        public int BookID { get; set; }

        [Required(ErrorMessage = "Tekst is verplicht.")]
        public string? Tekst { get; set; }

        [Range(1, 5, ErrorMessage = "Rating moet tussen 1 en 5 zijn.")]
        public int Rating { get; set; }

        public DateTime Datum { get; set; }
    }
}
