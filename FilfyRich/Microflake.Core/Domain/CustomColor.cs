using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.Core.Domain
{
    public class CustomColor
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        public float BuyPrice { get; set; }
        public float SellPrice { get; set; }
        public int Discount { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string Image { get; set; }

        public bool Status { get; set; }

        public long CategoryId { get; set; }
        public virtual CustomCategory Category { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}
