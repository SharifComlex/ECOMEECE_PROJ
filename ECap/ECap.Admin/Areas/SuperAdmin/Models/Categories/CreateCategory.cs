using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models.Categories
{
    public class CreateCategory
    {
        [Required]
        public string Name { get; set; }
    }
}
