using Microflake.Core.ViewModel.Category.Validations;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.Category
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
