using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECap.Core.Domain
{
    public class Product
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string Name { get; set; }

        public float Price { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsInStock { get; set; }

        public int StockQuantity { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(100)]
        public string Image { get; set; }

        public bool Status { get; set; }

        public long? SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public string UserId { get; set; }
        public AdminUser User { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}
