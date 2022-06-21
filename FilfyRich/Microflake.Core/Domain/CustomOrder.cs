using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Domain
{
   public class CustomOrder : BaseEntity
    {

        [Key]
        public int OrderId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        public string TransactionId { get; set; }

        [StringLength(160)]
        public string FirstName { get; set; }

        [StringLength(160)]
        public string LastName { get; set; }

        [StringLength(70, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [StringLength(40)]
        public string State { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Email { get; set; }

        public double Total { get; set; }

        public virtual List<CustomOrderDetail> CustomOrderDetails { get; set; }
        public string UserID { get; set; }
        public string Status { get; set; }
        public string OrderBy { get; set; }
        public string PaymentStatus { get; set; }

    }
}
