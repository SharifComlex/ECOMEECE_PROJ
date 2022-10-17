using System.Web;

namespace Microflake.Core
{
    public class Config
    {
        //public static string ConnectionString = "data source=A2NWPLSK14SQL-v05.shr.prod.iad2.secureserver.net;Initial Catalog=ph10046517109_db;UId=ph10046517109_db;Password=kVzi74&0;MultipleActiveResultSets=True";
        public static string ConnectionString = "data source=207.180.192.176;Initial Catalog=RMS;UId=sa;Password=Madinah@tech@321;MultipleActiveResultSets=True";
        public static string BaseUrl = "";
        
        public static readonly string Products = "/Public/Uploads/Products/";
        public static readonly string CustomProducts = "/Public/Uploads/CustomProducts/";

        public static readonly string Profile = "/Public/Profile/User/";
    }
}
