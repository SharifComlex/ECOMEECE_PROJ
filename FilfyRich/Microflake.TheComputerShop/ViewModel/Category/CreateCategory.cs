using Microflake.Core.ViewModel.Category.Validations;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.Category
{
    public class CreateCategory
    {
        [Required]
        [CategoryDuplicateName]
        public string Name { get; set; }
       
    }
}
