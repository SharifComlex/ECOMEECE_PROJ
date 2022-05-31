namespace Microflake.Core.Domain
{
    public class Whislist : BaseEntity
    {
        public long Id { get; set; }

        public string UserID { get; set; }

        public long ProductId { get; set; }

        public virtual Product Product { get; set; }

    }
}
