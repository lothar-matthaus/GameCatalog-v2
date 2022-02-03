using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameCatalog.Entity.Models
{
    public class Genre
    {
        [JsonPropertyName("id")]
        public int? GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}