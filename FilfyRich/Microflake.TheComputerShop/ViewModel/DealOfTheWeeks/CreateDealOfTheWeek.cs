using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.TheComputerShop.ViewModel.DealOfTheWeeks
{
   public class CreateDealOfTheWeek
    {

        public double Discount { get; set; }

        [Required(ErrorMessageResourceType = (typeof(Lang.Validations)), ErrorMessageResourceName = "RequiredQty")]

        public int Qty { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        [Required(ErrorMessageResourceType = (typeof(Lang.Validations)), ErrorMessageResourceName = "RequiredsellingPrice")]

        public double SellPrice { get; set; }
        [Required(ErrorMessageResourceType = (typeof(Lang.Validations)), ErrorMessageResourceName = "RequiredPrice")]

        public double Price { get; set; }

        public long SubBrandId { get; set; }
        [Required]
        public long CategoryId { get; set; }

        public long BrandId { get; set; }


        public long AttributeGroupId { get; set; }
        public string ProductImage { get; set; }

        public string ProductImage1 { get; set; }

        public string Description { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        public HttpPostedFileBase File { get; set; }

        //Deal Of The Week Data

        [Column(TypeName = "datetime2")]
        public DateTime DealTillDate { get; set; }

        public int DealForDays { get; set; }
    }
}
