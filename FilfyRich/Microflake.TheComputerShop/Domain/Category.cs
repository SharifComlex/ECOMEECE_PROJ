using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.TheComputerShop.Domain
{
    public class Category: BaseEntity
    {
        public long Id { get; set; }

      
        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string English { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(190)]
        public string Arabic { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string ImageName { get; set; }

    }
}
