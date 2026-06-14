using System.ComponentModel.DataAnnotations;

namespace Booksy.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string? Email { get; set; }

        // B-11-02: het wachtwoord moet minimaal 6 tekens bevatten.
        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(6, ErrorMessage = "Het wachtwoord moet minimaal 6 tekens bevatten.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
