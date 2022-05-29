using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class SubBrand : BaseEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(50)]
        public string ImageName { get; set; }

        public long BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
