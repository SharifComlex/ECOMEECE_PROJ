using Microflake.Core.ViewModel.Category.CustomCategories;
using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.CustomCategories
{
    public class CreateCategory
    {
        [Required]
        [CategoryDuplicateName]
        public string Name { get; set; }
       
    }
}
