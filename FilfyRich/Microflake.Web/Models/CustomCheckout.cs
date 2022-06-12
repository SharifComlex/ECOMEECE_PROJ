﻿using Microflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Microflake.Web.Models
{
    public class CustomCheckout
    {
        public long ProductId { get; set; }
        public long FrontChipId { get; set; }
        public long BackChipId { get; set; }
        public long Qty { get; set; }


        public CustomVariation Product { get; set; }
        public CustomVariation BackChip { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

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

        [Required]
        [StringLength(40)]
        public string Country { get; set; }

        [Required]
        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        // public ApplicationUser User { get; set; }

        [Required]
        public string stripeToken { get; set; }

        public string cardnumber { get; set; }
        public string exp_date { get; set; }
        public string cvc { get; set; }
    }
}