using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel.Orders
{
    public class EditOrder
    {
        public double SellPrice { get; set; }
        public long Id { get; set; }
        public string Status { get;  set; }

        public bool IsPaid { get; set; }
    }
}
