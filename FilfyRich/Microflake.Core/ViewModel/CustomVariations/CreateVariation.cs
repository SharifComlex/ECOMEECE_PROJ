using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microflake.Core.ViewModel.CustomVariations
{
   public class CreateVariation
   {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public long ColorId { get; set; }

        [Required]
        public long ItemId { get; set; }

        [Required]
        public string FrontImage { get; set; }

        [Required]
        public string BackImage { get; set; }

        [Required]
        public string BottomImage { get; set; }

    }
}
