using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.TheComputerShop.Domain
{
    public class BaseEntity
    {
        public bool Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedAt { get; set; }

        public string CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }

        public string ModifiedById { get; set; }
        public virtual ApplicationUser ModifiedBy { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsDeleted { get; set; }
    }
}
