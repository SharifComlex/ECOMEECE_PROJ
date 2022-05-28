namespace ECap.Admin.Areas.SuperAdmin.Models.Products
{
    public class ListProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
    }
}
