using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class Whislist : BaseEntity
    {
        public long Id { get; set; }

        public string UserID { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
