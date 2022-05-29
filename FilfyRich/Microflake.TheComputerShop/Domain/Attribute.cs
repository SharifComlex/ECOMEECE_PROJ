using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class Attribute : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public long AttributeGroupId { get; set; }
        public virtual AttributeGroup AttributeGroup { get; set; }
    }
}
