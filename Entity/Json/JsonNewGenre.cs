using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Json
{
    public class JsonNewGenre
    {
        [Required(ErrorMessage = "O gênero é obrigadório")]
        [MinLength(3, ErrorMessage = "O gênero deve ter ao menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "O gênero deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}