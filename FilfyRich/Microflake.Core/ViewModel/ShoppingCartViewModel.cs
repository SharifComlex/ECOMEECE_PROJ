using Microflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel
{
    public class ShoppingCartViewModel
    {
       
        public int Productcount { get; set; }

        public IEnumerable<CartItem> Items { get; set; }
        public double Total { get; set; }


        public ApplicationUser User { get; set; }


    }
}
