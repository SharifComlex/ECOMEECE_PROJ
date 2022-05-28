using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECap.Core.Domain
{
    public class AdminUser : IdentityUser
    {
        [MaxLength(20)]
        public string Name { get; set; }

        public Roles Role { get; set; }

        public string Address { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}
