using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models.Products
{
    public class CreateProduct
    {
        public long CategoryId { get; set; }

        [Required]
        public long SubCategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public float Price { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public IFormFile File { get; set; }
    }
}
