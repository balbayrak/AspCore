using AspCore.Business.Abstract;
using AspCoreTest.Entities.SearchableEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCoreTest.Business.Abstract
{
    public interface IPersonSearchEntityService  : ISearchableEntityService<PersonSearchEntity>, IBusinessService
    {
    }
}
