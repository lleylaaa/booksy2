using System.ComponentModel.DataAnnotations;

namespace Booksy.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mailadres is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
