using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.Core.Domain
{
    public  class SubCategory
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public bool Status { get; set; }
    }
}
