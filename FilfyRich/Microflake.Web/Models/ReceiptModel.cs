using Microflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microflake.Web.Models
{
    public class ReceiptModel
    {
        public Order Order { get; set; }
        public List<OrderDetals> Items { get; set; }
    }
}