using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.Abstract;
using AspCore.Entities.General;

namespace AspCoreTest.DataAccess.Abstract
{
    public interface ICityDAL : IEntityRepository<City>
    {
        ServiceResult<IList<City>> GetListNoTracking();
    }
}
