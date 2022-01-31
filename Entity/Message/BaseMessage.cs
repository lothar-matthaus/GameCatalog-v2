using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Message
{
    public abstract class BaseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}