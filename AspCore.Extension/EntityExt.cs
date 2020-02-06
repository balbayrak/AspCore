using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Utilities.DataProtector;

namespace AspCore.Extension
{
    public static class EntityExt
    {
        public static void ProtectEntity(this IEntity entity)
        {
            IDataProtectorHelper protectorHelper = DependencyResolver.Current.GetService<IDataProtectorHelper>();

            entity.EncryptedId = protectorHelper.Protect(entity.Id.ToString());

        }
    }
}
