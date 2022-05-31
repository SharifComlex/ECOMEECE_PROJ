using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
   public class Currency :BaseEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Simbal { get; set; }

        public double Currency_Rate { get; set; }

        public bool Base_Currency { get; set; }
    }
}
