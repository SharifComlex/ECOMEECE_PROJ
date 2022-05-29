using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Domain
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "varchar")]
        [MaxLength(1000)]
        public string EnglishMessage { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(1000)]
        public string ArabicMessage { get; set; }


        public bool IsSoundPlayed { get; set; }

        public bool IsRead { get; set; }

        [Column(TypeName = "nvarchar")]
        [MaxLength(55)]
        public string Time_Zone { get; set; }

        public DateTime? ReadAt { get; set; }

        //public string Created_By { get; set; }

        public DateTime Created_At { get; set; }

    }
}
