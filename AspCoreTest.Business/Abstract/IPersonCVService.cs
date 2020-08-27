using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Business.Abstract;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Business.Abstract
{
    public interface IPersonCVService : IEntityService <PersonCv,PersonCvDto>, IBusinessService
    {
        Task<ServiceResult<List<PersonCvDto>>> GetWithInclude();
    }
}
