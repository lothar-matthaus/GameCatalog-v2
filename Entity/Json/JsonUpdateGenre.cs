using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Json
{
    public class JsonUpdateGenre
    {
        [Required(ErrorMessage = "Para atualizar o gênero, é preciso informar o ID.")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "O nome do gênero é obrigatório")]
        [MinLength(3, ErrorMessage = "O gênero deve ter ao menos 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "O gênero deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }
    }
}