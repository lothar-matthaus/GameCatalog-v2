using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalog.Entity.Message
{
    public class MessageError : BaseMessage
    {
        public string ErrorMessage { get; set; }
    }
}