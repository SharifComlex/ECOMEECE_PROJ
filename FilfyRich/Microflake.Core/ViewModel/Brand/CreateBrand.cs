﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel.Brand
{
   public class CreateBrand
    {

        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
  
        public string ImageName { get; set; }
        [Required]
        public long CategoryId { get; set; }


        public bool Status { get; set; }
    }
}