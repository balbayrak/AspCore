using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataAccess.General
{
    public class MessageExtension
    {
        public static string  SuccessMessage<TEntity>(CoreEntityState entityState)
              where TEntity : class, IEntity, new()
        {
            if (entityState == CoreEntityState.Added)
            {
                return string.Format(DALConstants.DALErrorMessages.DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER, typeof(TEntity).Name);
            }
            else if (entityState == CoreEntityState.Deleted)
            {
                return string.Format(DALConstants.DALErrorMessages.DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER, typeof(TEntity).Name);
            }
            else
            {
                return string.Format(DALConstants.DALErrorMessages.DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER, typeof(TEntity).Name);
            }
        }
    }
}
