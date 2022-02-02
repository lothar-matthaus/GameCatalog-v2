using System.ComponentModel.DataAnnotations;

namespace GameCatalog.Entity.Json
{
    public class JsonUserLogin
    {
        [Required(ErrorMessage = "Insira o endereço de E-Mail")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Insira a sua senha.")]
        [MinLength(8, ErrorMessage = "A senha é muito curta.")]
        public string Password { get; set; }
    }
}