using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class OrderDetals
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
