using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Models
{
    public class ChangePassword
    {
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string NewPassword { get; set; }
    }
}
