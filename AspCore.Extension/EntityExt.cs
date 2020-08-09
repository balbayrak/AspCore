using AspCore.Dtos.Dto;
using AspCore.Entities.EntityType;
using AspCore.Utilities.DataProtector;

namespace AspCore.Extension
{
    public static class EntityExt
    {
        public static void ProtectEntity(this IEntity entity)
        {
            entity.EncryptedId = DataProtectorFactory.Instance.Protect(entity.Id.ToString());
        }
        public static void ProtectEntity(this IEntityDto entity)
        {
            entity.EncryptedId = DataProtectorFactory.Instance.Protect(entity.Id.ToString());
        }
    }
}
