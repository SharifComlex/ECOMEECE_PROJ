using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microflake.Core.Domain
{
    public class CustomVariation
    {
        public long Id { get; set; }
        
        public long? CapId { get; set; }
        public virtual Product Cap { get; set; }

        public long? BadgeId { get; set; }
        public virtual Product Badge { get; set; }

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
