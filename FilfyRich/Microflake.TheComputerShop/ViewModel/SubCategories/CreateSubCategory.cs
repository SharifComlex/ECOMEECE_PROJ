using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microflake.TheComputerShop.ViewModel.SubCategories
{
   public class CreateSubCategory
    {
        public long Id { get; set; }

     
        public string English { get; set; }
        public string Arabic { get; set; }

        public string ImageName { get; set; }
        [Required]
        public long CategoryId { get; set; }


        public bool Status { get; set; }
    }
}
