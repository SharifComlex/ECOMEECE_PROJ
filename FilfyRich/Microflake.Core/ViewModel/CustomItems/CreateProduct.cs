using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.Core.ViewModel.CustomItems
{
   public class CreateProduct
   {
        [Required]
        public string Name { get; set; }

        [Required]
        public float BuyPrice { get; set; }

        [Required]
        public float SellPrice { get; set; }

        public int Discount { get; set; }

        [Required]
        public long CategoryId { get; set; }

        [Required]
        public string Image { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}
