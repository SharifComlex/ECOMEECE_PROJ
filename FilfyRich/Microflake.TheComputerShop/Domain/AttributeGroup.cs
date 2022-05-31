using System.Collections.Generic;

namespace Microflake.Core.Domain
{
    public class AttributeGroup : BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public List<Domain.Attribute> Attribute { get; set; }
    }
}
