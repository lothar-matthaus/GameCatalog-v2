using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Message
{
    public class MessageSuccess : BaseMessage
    {
        public object Content { get; set; }
    }
}