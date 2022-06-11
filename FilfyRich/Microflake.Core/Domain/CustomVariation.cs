using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.Core.Domain
{
    public class CustomVariation
    {
        public long Id { get; set; }
        
        public long? CategoryId { get; set; }
        public virtual CustomCategory Category { get; set; }

        public long? CustomColorId { get; set; }
        public virtual CustomColor CustomColor { get; set; }

        public long? CustomItemId { get; set; }
        public virtual CustomItem CustomItem { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string FrontImage { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string BackImage { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string BottomImage { get; set; }
    }
}
