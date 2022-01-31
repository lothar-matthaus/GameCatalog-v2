using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entity.Models;

namespace GameCatalog.Repository.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        IEnumerable<Genre> Get(ICollection<int> genre);
    }
}