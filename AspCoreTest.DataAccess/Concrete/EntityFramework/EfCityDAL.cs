using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.DataAccess.EntityFramework;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfCityDAL : EfEntityRepositoryBase<AspCoreTestDbContext, City>, ICityDAL
    {
        public ServiceResult<IList<City>> GetListNoTracking()
        {
            ServiceResult<IList<City>> result = new ServiceResult<IList<City>>();
            try
            {
            
                var list = TableNoTracking.ToList();
                result.Result = list;
            }
            catch (Exception e)
            {
                result.ErrorMessage("hata oluştu", e);
            }
            return result;
        }
    }
}
