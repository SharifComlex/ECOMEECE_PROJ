using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models.Categories
{
    public class EditCategory
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
