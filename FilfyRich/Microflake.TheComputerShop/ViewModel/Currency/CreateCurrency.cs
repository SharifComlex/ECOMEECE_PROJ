using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel.Currency
{
   public class CreateCurrency
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Currency_Rate { get; set; }

        public bool Base_Currency { get; set; }


    }
}
