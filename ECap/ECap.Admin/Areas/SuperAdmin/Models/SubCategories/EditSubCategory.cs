using System.ComponentModel.DataAnnotations;

namespace ECap.Admin.Areas.SuperAdmin.Models.SubCategories
{
    public class EditSubCategory
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long? CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
