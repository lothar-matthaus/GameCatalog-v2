using System;
using GameCatalog.Entity.Enum;

namespace GameCatalog.Entity.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime SignUpDate { get; set; }
    }
}