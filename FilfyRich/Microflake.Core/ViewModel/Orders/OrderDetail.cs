using Microflake.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.Orders
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public string Name { get; set; }

        public bool IsCustom { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }

        public string CapImage { get; set; }
        public string FrontBadge { get; set; }
        public string BackBadge { get; set; }

        public int OrderId { get; set; }

    }
}
