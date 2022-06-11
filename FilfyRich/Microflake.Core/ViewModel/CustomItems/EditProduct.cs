using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Microflake.Core.ViewModel.CustomItems
{
    public class EditProduct
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float BuyPrice { get; set; }

        [Required]
        public float SellPrice { get; set; }

        public int Discount { get; set; }

        [Required]
        public long CategoryId { get; set; }

        public bool Status { get; set; }

        public string Image { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}
