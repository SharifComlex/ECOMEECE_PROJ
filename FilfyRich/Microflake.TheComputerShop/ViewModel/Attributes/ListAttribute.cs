using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel.Attributes
{
   public class ListAttribute
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public long AttributeGroupId { get; set; }


        public bool Status { get; set; }
        public DateTime Created { get; set; }
        public string AttributeGroupName { get; internal set; }
    }
}
