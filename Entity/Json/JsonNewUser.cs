using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entity.Enum;

namespace GameCatalog.Entity.Json
{
    public class JsonNewUser
    {
        [Required(ErrorMessage = "O usuário precisa de um nome.")]
        [MinLength(3, ErrorMessage = "O nome do usuário é muito curto.")]
        [MaxLength(50, ErrorMessage = "O nome do usuário é muito longo.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O usuário precisa de um endereço de E-Mail.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O usuário precisa ter um cargo.")]
        public UserRole UserRole { get; set; }

        [Required(ErrorMessage = "Uma senha deve ser inserida.")]
        [MinLength(8, ErrorMessage = "A senha é muito curta.")]
        public string Password { get; set; }
    }
}