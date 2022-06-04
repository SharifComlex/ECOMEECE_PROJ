using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel
{
   public enum SortByEnum
    {
        Default = 1,
        Popularity = 2,
        PriceLowToHigh = 3,
        PriceHighToLow = 4,
        Between0to500 = 5,
        Between500to1000 = 6,
        Between1000to1500 = 7,
        Between1500to2000 = 8,
    }
}
