using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.TheComputerShop.ViewModel.Products
{
   public class EditProduct
    {
        public double Discount { get; set; }

        public string English { get; set; }
        public string Arabic { get; set; }

        public int Qty { get; set; }
        public string ProductImage1 { get; set; }
        public double SellPrice { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        public long SubCategoryId { get; set; }

        public string ProductImage { get; set; }

        public HttpPostedFileBase File { get; set; }
        public bool Status { get;  set; }
   
        public long Id { get;  set; }
    }
}
