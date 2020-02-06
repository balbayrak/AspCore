using AspCore.Entities.Configuration;

namespace AspCore.DataAccess.Configuration
{
    public interface IDataAccessLayerOption : IConfigurationEntity
    {
        EnumDataLayerType DataLayerType { get; set; }

        DatabaseSetting DatabaseSetting { get; set; }
    }
}
