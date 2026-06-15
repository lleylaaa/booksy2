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

        // FR-10: leesstatus. B-10-02: altijd een waarde, daarom een standaard.
        public string ReadingStatus { get; set; } = "Wil ik lezen";

        // FR-13: verwijzing naar de omslagafbeelding (mag leeg zijn).
        public string? CoverImage { get; set; }

        // B-13-01: is er geen omslag, dan tonen we een standaardafbeelding.
        public string CoverImageUrl =>
            string.IsNullOrWhiteSpace(CoverImage) ? "/images/no-cover.svg" : CoverImage;
    }
}
