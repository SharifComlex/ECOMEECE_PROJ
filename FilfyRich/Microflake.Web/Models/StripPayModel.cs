using System;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Web.Models
{
    public class StripPayModel
    {
        [Required]
        [Range(1, 3)]
        public int Package { get; set; }

        [Required]
        public string carrierName { get; set; }

        [Required]
        public int MC2 { get; set; }

        [Required]
        public string printedName { get; set; }

        [Required]
        public DateTime Adate { get; set; }

        [Required]
        public string firstName { get; set; }
        public string middleName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string phone { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string stripeToken { get; set; }

        public string cardnumber { get; set; }
        public string exp_date { get; set; }
        public string cvc { get; set; }

    }
}