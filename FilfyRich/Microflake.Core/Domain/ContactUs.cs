﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
  public  class ContactUs :BaseEntity
    {
        public long Id { get; set; }
        
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string  subject { get; set; }
        public string Message { get; set; }

        public string Email { get; set; }
    }
}
