using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameCatalog.Entity.Json {
    public class JsonUpdateGenre : JsonNewGenre
    {
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "Para atualizar o gênero, é preciso informar o ID.")]
        public int GenreId { get; set; }
    }
}