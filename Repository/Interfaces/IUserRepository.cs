using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entity.Models;

namespace GameCatalog.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        int Save(Login login);
    }
}