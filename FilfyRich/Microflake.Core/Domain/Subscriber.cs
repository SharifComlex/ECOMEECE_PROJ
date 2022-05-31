using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
   public class Subscriber :BaseEntity
    {
        public long Id { get; set; }

        public string  email { get; set; }
    }
}
