using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entity.Models;

namespace GameCatalog.Services.Interfaces
{
    public interface IUserService
    {
        string Encrypt(string salt);
        string Encrypt(string salt, string password, int interations);
    }
}