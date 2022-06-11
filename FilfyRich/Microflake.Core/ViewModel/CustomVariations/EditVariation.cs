using System.ComponentModel.DataAnnotations;

namespace Microflake.Core.ViewModel.CustomVariations
{
    public class EditVariation
    {
        public long Id { get; set; }

        [Required]
        public long? CategoryId { get; set; }

        [Required]
        public long? ColorId { get; set; }

        [Required]
        public long? ItemId { get; set; }

        public string FrontImage { get; set; }

        public string BackImage { get; set; }

        public string BottomImage { get; set; }

    }
}
