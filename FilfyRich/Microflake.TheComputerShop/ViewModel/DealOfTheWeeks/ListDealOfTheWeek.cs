using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.TheComputerShop.ViewModel.DealOfTheWeeks
{
   public class ListDealOfTheWeek
    {
        public double Discount { get; set; }

        public int Qty { get; set; }
        public IEnumerable<Product> Productlist { get; set; }
        public IEnumerable<Whislist> Whislists { get; set; }

        public IEnumerable<Domain.Category> Categorylist { get; set; }
        public int? SortBy { get; set; }
        public int? CategoryID { get; set; }
        public double MaximumPrice { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        public string Image { get; set; }

        public string Image1 { get; set; }
        public double SellPrice { get; set; }
        public string SearchTerm { get; set; }
        public int RecordSize { get; set; }
        public Pager Pager { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }

        public bool IsNew { get; set; }

        public bool Status { get; set; }
        public string CategoryName { get; set; }
        public DateTime Created { get; set; }

        public HttpPostedFileBase File { get; set; }
        public long CategoryId { get; set; }
        public long Id { get; set; }
        public string CategoryEnglish { get;  set; }
        public string CategoryArabic { get;  set; }
    }
}
