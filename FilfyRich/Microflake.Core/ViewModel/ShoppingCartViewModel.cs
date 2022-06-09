using Microflake.Core.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel
{
    public class ShoppingCartViewModel
    {
       
        public int Productcount { get; set; }

        public IEnumerable<CartItem> Items { get; set; }
        public double Total { get; set; }


        public ApplicationUser User { get; set; }

        [Required]
        public string stripeToken { get; set; }

        public string cardnumber { get; set; }
        public string exp_date { get; set; }
        public string cvc { get; set; }
    }
}
