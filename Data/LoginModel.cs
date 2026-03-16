using System.ComponentModel.DataAnnotations;

namespace EIKON.UI.Data
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuario requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*]).{6,}$",
            ErrorMessage = "Debe incluir mayúscula, minúscula, número y símbolo.")]
        public string Password { get; set; }
    }
}
