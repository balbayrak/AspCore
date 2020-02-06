namespace AspCore.DataAccess.Configuration
{
    public class DataAccessLayerOption : IDataAccessLayerOption
    {
        public EnumDataLayerType DataLayerType { get; set; }
        public DatabaseSetting DatabaseSetting { get; set; }

    }
}
