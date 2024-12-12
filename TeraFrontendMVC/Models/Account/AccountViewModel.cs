using System.ComponentModel.DataAnnotations;

namespace TeraFrontendMVC.Models.Account
{
    public class Register
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(2, ErrorMessage = "Debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "No debe exceder 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(2, ErrorMessage = "Debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "No debe exceder 50 caracteres.")]
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
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un número y un carácter especial.")]
        public string Password { get; set; }
    }

    public class ChangePassword
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula, un número y un carácter especial.")]
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

    public class UserListResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalUsers { get; set; }
        public int TotalPages { get; set; }
        public List<UserProfile> Users { get; set; }
    }

    public class UpdateUser
    {
        [Required(ErrorMessage = "Campo Requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo no es correcto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(2, ErrorMessage = "Debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "No debe exceder 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [MinLength(2, ErrorMessage = "Debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "No debe exceder 50 caracteres.")]
        public string Apellido { get; set; }
    }
}
