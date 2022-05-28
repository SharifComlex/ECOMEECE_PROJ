using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models.SubCategories
{
    public class CreateSubCategory
    {
        [Required]
        public long CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
