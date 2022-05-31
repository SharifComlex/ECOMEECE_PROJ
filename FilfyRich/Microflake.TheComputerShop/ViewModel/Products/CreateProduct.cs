using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.Core.ViewModel.Products
{
   public class CreateProduct
    {
        [Required]
        public long SubCategoryId { get; set; }
        public double Discount { get; set; }

        [Required]
        public int Qty { get; set; }
    
        public string Name { get; set; }
        [Required]
        public double SellPrice { get; set; }
        [Required]
        public double Price { get; set; }
        public long SubBrandId { get; set; }
       
       
        public long BrandId { get; set; }


        public long AttributeGroupId { get; set; }
        public string ProductImage { get; set; }

        public string ProductImage1 { get; set; }

        public string Description { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}
