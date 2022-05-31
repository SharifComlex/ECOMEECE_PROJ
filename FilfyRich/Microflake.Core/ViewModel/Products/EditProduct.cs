using System.Web;

namespace Microflake.Core.ViewModel.Products
{
    public class EditProduct
    {
        public double Discount { get; set; }

        public string Name { get; set; }

        public int Qty { get; set; }
        public string ProductImage1 { get; set; }
        public double SellPrice { get; set; }
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
