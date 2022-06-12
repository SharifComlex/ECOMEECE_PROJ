using Microflake.Core.Domain;
using System.Collections.Generic;

namespace Microflake.Web.Areas.SuperAdmin.Models
{
    public class CustomItemModel
    {
        public List<CustomColor> Colors { get; set; }
        public List<CustomItem> Items { get; set; }
    }
}