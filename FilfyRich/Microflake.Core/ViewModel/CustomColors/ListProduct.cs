using System;

namespace Microflake.Core.ViewModel.CustomColors
{
    public class ListProduct
    {
        public long Id { get; set; }

        public string Image { get; set; }
        
        public string Name { get; set; }

        public float BuyPrice { get; set; }

        public float SellPrice { get; set; }

        public int Discount { get; set; }

        public long CategoryId { get; set; }
        public string CategoryName{ get; set; }

        public bool Status { get; set; }
        public DateTime Created { get; set; }
    }
}
