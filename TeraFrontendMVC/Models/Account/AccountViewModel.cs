using System.ComponentModel.DataAnnotations;

namespace TeraFrontendMVC.Models.Account
{
    public class Register
    {
        [Required(ErrorMessage = "Campo Requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un número y un carácter especial.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class Login
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string Password { get; set; }
    }

    public class ChangePassword
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        public string PasswordActual { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un número y un carácter especial.")]
        public string PasswordNuevo { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }

    public class UserProfile
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
