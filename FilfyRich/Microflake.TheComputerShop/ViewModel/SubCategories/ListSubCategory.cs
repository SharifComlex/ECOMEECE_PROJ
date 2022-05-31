using System;


namespace Microflake.Core.ViewModel.SubCategories
{
    public class ListSubCategory
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string ImageName { get; set; }
        
        public DateTime Created { get; set; }
        
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

        public bool Status { get; set; }
    }
}
