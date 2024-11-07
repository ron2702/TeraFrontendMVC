using System.ComponentModel.DataAnnotations;

namespace TeraFrontendMVC.Models.Account
{
    public class Register
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un número y un carácter especial.")]
        public string Password { get; set; }
    }

    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class ChangePassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PasswordActual { get; set; }
        [Required]
        public string PasswordNuevo { get; set; }
    }
}
