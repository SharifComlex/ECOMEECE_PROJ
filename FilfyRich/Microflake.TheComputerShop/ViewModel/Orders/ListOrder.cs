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
        public string PRoductTitle { get;  set; }
        public int Quanty { get;  set; }
        public double UnitPrice { get;  set; }
        public double Total { get;  set; }

        public double SellPrice { get; set; }
        public string OrderBy { get; set; }
        public string Status { get;  set; }
        public string Currency { get; set; }
    }

}
