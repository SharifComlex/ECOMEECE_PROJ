using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel.Orders
{
    public class ListOrder
    {
        public int orderId { get; set; }

        public long Id { get; set; }

        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public int Quanty { get;  set; }
        public double ShippingCharges { get;  set; }
        public double Total { get;  set; }

        public string Status { get;  set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderDetail> Items { get; set; }
    }

}
