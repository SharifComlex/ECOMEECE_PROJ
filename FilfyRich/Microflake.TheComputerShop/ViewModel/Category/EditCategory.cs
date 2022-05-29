using Microflake.TheComputerShop.ViewModel.Category.Validations;
using System.ComponentModel.DataAnnotations;

namespace Microflake.TheComputerShop.ViewModel.Category
{
    public class EditCategory
    {
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = (typeof(Lang.Validations)), ErrorMessageResourceName = "categoryNameRequired")]
        [RegularExpression("^[A-Za-z][A-Za-z ]+[A-Za-z]$", ErrorMessageResourceType = (typeof(Lang.Validations)), ErrorMessageResourceName = "OnlyAlphabats")]
        [CategoryDuplicateName]
        public string English { get; set; }

        // [Required(ErrorMessageResourceType = (typeof(Language.ValidationMessage)), ErrorMessageResourceName = "DoctorNameRequired")]
        public string Arabic { get; set; }

        public bool Status { get; set; }
    }
}
