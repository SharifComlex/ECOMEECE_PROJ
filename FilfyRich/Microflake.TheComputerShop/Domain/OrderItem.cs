using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class OrderItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

        public long ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
