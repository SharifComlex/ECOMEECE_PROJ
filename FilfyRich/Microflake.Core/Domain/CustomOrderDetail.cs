using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
   public class CustomOrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public long CustomColorId { get; set; }
        public virtual CustomColor CustomColor { get; set; }
        public long? CustomItem1Id { get; set; }
        public virtual CustomItem CustomItem1 { get; set; }
        public long? CustomItem2Id { get; set; }
        public virtual CustomItem CustomItem2 { get; set; }
        public virtual CustomOrder Order { get; set; }
    }
}
