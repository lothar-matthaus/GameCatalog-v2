using System;
using System.Collections.Generic;

namespace GameCatalog.Entity.Models
{
    public class Genre
    {
        public int? GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}