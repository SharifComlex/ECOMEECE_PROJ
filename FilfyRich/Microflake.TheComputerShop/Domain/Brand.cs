using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.TheComputerShop.Domain
{
    public class Brand : BaseEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
