using Microflake.Core.Domain;
using System.Collections.Generic;

namespace Microflake.Web.Areas.SuperAdmin.Models
{
    public class CustomItemModel
    {
        public List<Product> Colors { get; set; }
        public List<Product> Items { get; set; }
    }
}