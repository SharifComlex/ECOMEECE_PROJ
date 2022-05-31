using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel.Brand
{
  public  class ListBrand
    {
        public long Id { get; set; }

        public string Name { get; set; }
     
        public string ImageName { get; set; }
        public DateTime Created { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get;  set; }
        public bool Status { get;  set; }
    }
}
