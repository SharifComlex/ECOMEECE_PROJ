namespace ECap.Core.Config
{
    public class Database
    {
        public static string ConnectionString
        {
            get
            {
                return "data source=.;Initial Catalog=ECapdb;Integrated Security=true;MultipleActiveResultSets=True";
            }
        }
    }
}
