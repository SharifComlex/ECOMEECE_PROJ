using Microflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.ViewModel.Orders
{
   public class OrderDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PRoductTitle { get; set; }
        public int Quanty { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }

        public double SellPrice { get; set; }
        public string OrderBy { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }


        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public virtual OrderDetals OrderDetails { get; set; }

    }
}
