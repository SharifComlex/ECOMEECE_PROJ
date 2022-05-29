using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel.Category
{
    public class ListCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string English { get; set; }
        public string Arabic { get; set; }
        public bool Status { get; set; }
        public DateTime Created { get; set; }
    }
}
