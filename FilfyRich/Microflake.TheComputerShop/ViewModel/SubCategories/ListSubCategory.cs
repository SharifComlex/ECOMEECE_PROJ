using System;
using System.Collections.Generic;
using System.Text;


namespace Microflake.TheComputerShop.ViewModel.SubCategories
{
    public class ListSubCategory
    {
        public long Id { get; set; }

        public string English { get; set; }
        public string Arabic { get; set; }
        public string ImageName { get; set; }
        public DateTime Created { get; set; }
        public long CategoryId { get; set; }
       
        public string CategoryName { get; set; }
        public bool Status { get; set; }
    }
}
