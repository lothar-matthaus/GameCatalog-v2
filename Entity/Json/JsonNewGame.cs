using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameCatalog.Entity.Json
{
    public class JsonNewGame
    {
        [Required(ErrorMessage = "O jogo precisa ter um título")]
        [MinLength(3, ErrorMessage = "O título do jogo deve ter ao menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "O título do jogo não deve conter mais que 50 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O jogo precisa ter uma descrição")]
        [MinLength(10, ErrorMessage = "A descrição do jogo deve ter ao menos 10 caracteres")]
        [MaxLength(400, ErrorMessage = "A descrição do jogo não deve conter mais que 400 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O jogo precisa ter uma capa.")]
        [MinLength(10, ErrorMessage = "A URL da capa do jogo deve ter ao menos 10 caracteres")]
        public string CoverUrl { get; set; }

        [Required(ErrorMessage = "O jogo precisa ter uma data de lançamento.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "O jogo precisa um gênero.")]
        [MinLength(1, ErrorMessage = "O jogo precisa ter ao menos um gênero.")]
        public ICollection<int> Genres { get; set; }
    }
}