using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Microflake.TheComputerShop.Domain
{
  public  class SubCategory :BaseEntity
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(190)]
        public string English { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(190)]
        public string Arabic { get; set; }

        public string Description { get; set; }

        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
