using Microflake.TheComputerShop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel
{
    public class ShoppingCartViewModel
    {
       
        public int Productcount { get; set; }

        public IEnumerable<CartItem> Items { get; set; }
        public double Total { get; set; }


        public ApplicationUser User { get; set; }


    }
}
