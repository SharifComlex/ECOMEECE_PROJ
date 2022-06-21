using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        // this is the cart id this will be unique for every individual visitoor
        public string CartId { get; set; }

        public long? ProductId { get; set; }
        public long? FrontChipId { get; set; }
        public long? BackChipId { get; set; }
        
        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Product Product { get; set; }
        public virtual Product FrontChip { get; set; }
        public virtual Product BackChip { get; set; }


     
    }
}
