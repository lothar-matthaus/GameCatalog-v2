using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Repository.Interfaces
{
    public interface IRepository<Entity> where Entity : class
    {
        IEnumerable<Entity> Get();
        Entity Get(int id);

        int Save(Entity entity);
        int Update(Entity entity);
        int Remove(int id);
    }
}