using System.Web;

namespace Microflake.Core
{
    public class Config
    {
        //public static string ConnectionString = "data source=.;Initial Catalog=F2;Integrated Security=true";
        public static string ConnectionString = "data source=207.180.192.176;Initial Catalog=RMS;UId=sa;Password=Madinah@tech@321;MultipleActiveResultSets=True";
        public static string BaseUrl = "";
       // public static string ConnectionString = "data source=207.180.192.176;Initial Catalog=Fashion_Ecommerce;UId=sa;Password=Madinah@tech@321;MultipleActiveResultSets=True";

        public static readonly string Products = "/Public/Uploads/Products/";
        public static readonly string CustomProducts = "/Public/Uploads/CustomProducts/";

        public static readonly string Profile = "/Public/Profile/User/";
    }
}
