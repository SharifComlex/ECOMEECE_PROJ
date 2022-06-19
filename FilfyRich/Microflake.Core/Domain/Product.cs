using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.Core.Domain
{
    public class Product : BaseEntity
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public double SellPrice { get; set; }
        public double Discount { get; set; }

        public int Qty { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        public long SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string Image { get; set; }


        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string Image1 { get; set; }

        public bool IsHasVariation { get; set; }
        public bool IsVariationOverlay { get; set; }

        public bool IsPaid { get; set; }

        public bool DealOfTheWeek { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime DealTillDate { get; set; }
    }
}
