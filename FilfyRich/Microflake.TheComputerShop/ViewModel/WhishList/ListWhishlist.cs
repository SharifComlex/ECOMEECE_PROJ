using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel.WhishList
{
   public class ListWhishlist
    {
        public string CreatedById { get; set; }

        public long Id { get; set; }

        public long productId { get; set; }

        public string productName { get; set; }

        public  double productPrice { get; set; }
        public string Image { get; set; }
        public string Image1 { get; set; }

        public string UserID { get; set; }

    }
}
