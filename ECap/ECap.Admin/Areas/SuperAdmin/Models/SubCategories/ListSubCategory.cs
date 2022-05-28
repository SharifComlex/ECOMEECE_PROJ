namespace ECap.Admin.Areas.SuperAdmin.Models.SubCategories
{
    public class ListSubCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
    }
}
