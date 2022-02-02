using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCatalog.Entity.Models;

namespace GameCatalog.Services.Interfaces
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}