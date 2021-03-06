using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECap.Core.Domain
{
    public class SubCategory
    {
        public long Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        public string UserId { get; set; }
        public AdminUser User { get; set; }

        public long? CategoryId { get; set; }
        public Category Category { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}
