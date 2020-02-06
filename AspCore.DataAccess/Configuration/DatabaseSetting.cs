namespace AspCore.DataAccess.Configuration
{
    public class DatabaseSetting 
    {
        public DatabaseType DatabaseType { get; set; }
        public string MSSQL_ConnectionString { get; set; }
        public string MySQL_ConnectionString { get; set; }
        public string Oracle_ConnectionString { get; set; }
        public DataBaseTransaction DataBaseTransaction { get; set; }

    }
}
