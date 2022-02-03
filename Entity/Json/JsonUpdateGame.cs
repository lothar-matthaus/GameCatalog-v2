using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameCatalog.Entity.Json
{
    public class JsonUpdateGame : JsonNewGame
    {

        [JsonPropertyName("id")]
        [Required(ErrorMessage = "Para atualizar o jogo, é necessário informar o ID.")]
        public int GameId { get; set; }
    }
}