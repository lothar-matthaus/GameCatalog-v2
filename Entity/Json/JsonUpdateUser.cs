using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GameCatalog.Entity.Json
{
    public class JsonUpdateUser : JsonNewUser
    {
        [JsonPropertyName("id")]
        [Required(ErrorMessage = "O Id do usuário é obrigatório para a atualização.")]
        public int UserId { get; set; }
    }
}