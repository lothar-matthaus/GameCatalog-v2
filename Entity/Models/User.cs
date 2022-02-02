using System;
using GameCatalogv2.Entity.Enum;

namespace GameCatalog.Entity.Models
{
    public class User
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public Login Login { get; set; }
        public string UserRole { get; set; }
        public DateTime SignUpDate { get; set; }
    }
}