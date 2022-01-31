using System;
using System.Collections.Generic;

namespace GameCatalog.Entity.Models
{
    public class Game
    {
        public int? GameId { get; set; }
        public string Title { get; set; }
        public string CoverUrl { get; set; }
        public string Description { get; set; }
        public List<Genre> Genre { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}