using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models
{
    public class UpdatePassword
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
