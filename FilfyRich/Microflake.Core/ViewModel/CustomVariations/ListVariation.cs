using System;

namespace Microflake.Core.ViewModel.CustomVariations
{
    public class ListVariation
    {
        public long Id { get; set; }

        public string CategoryName { get; set; }
        public string ColorName { get; set; }
        public string ItemName { get; set; }
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
        public string BottomImage { get; set; }

        public bool Status { get; set; }
        public DateTime Created { get; set; }
    }
}
