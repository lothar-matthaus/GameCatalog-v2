using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Message
{
    public class TokenMessage : BaseMessage
    {
        public string Token { get; set; }
    }
}