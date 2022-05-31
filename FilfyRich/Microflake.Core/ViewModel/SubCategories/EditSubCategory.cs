using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.SubCategories
{
    public  class EditSubCategory
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }


        public string ImageName { get; set; }

        [Required]
        public long CategoryId { get; set; }
        
        public bool Status { get; set; }
    }
}
