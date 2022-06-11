using Microflake.Core.ViewModel.Category.CustomCategories;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.CustomCategories
{
    public class EditCategory
    {
        public long Id { get; set; }

        [Required]
        [CategoryDuplicateName]
        public string Name { get; set; }

        public bool Status { get; set; }
    }
}
