using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Booksy.ViewModels
{
    public class BookViewModel
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "Titel is verplicht.")]
        public string? Name { get; set; }

        // FR-14: de gebruiker selecteert of typt een auteur. We werken in de UI
        // met de naam; de service zoekt of maakt de bijbehorende auteur-entiteit.
        [Required(ErrorMessage = "Auteur is verplicht.")]
        public string? Author { get; set; }

        // FR-15: een boek kan meerdere genres hebben. In het formulier komen die
        // binnen als komma-gescheiden tekst, in de details tonen we de lijst.
        public string? Genres { get; set; }

        public List<string> GenreNames { get; set; } = new();
    }
}
