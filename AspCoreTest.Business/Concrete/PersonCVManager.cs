using AspCore.Business.Concrete;
using AspCore.Dependency.DependencyAttributes;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.DataAccess.General;

namespace testbusiness.Concrete
{
    [ExposedService(typeof(IPersonCVService))]
    public class PersonCVManager : BaseEntityManager<IPersonCvDAL,PersonCv, PersonCvDto>, IPersonCVService
    {
        private IPersonDal personDal;
        public PersonCVManager(IServiceProvider serviceProvider, IPersonDal personDal) : base(serviceProvider)
        {
            this.personDal = personDal;
        }

        public async Task<ServiceResult<List<PersonCvDto>>> GetWithInclude()
        {
            var dataFilter = new DataAccessFilter<PersonCv>();
            dataFilter.Load(t => t.Person).Load(t=>t.Person.Admin);
            dataFilter.query = t => t.Id == new Guid("e2845ff9-4f00-4c17-8e4b-0e5d8617197b") ||
                       t.Id == new Guid("7553c60d-3495-4132-a160-0ede96e50618");
            var result =
               await DataAccess.GetListAsync(dataFilter);
            var data = AutoObjectMapper.Mapper.Map<List<PersonCvDto>>(result.Result);
            return result.ChangeResult(data);

        }

        public override async Task<ServiceResult<bool>> AddAsync(params PersonCvDto[] entities)
        {
            var entityArray = AutoObjectMapper.Mapper.Map<PersonCvDto[], PersonCv[]>(entities);
            var entityPerson =new Person()
            {
                Id = Guid.NewGuid(),
                Name = "asddasd",
                Surname = "sdaasd",
                
            };

            TransactionBuilder.BeginTransaction();
            try
            {
                var result1 = await DataAccess.AddAsync(entityArray);
                var result = await personDal.AddAsync(entityPerson);
                TransactionBuilder.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                TransactionBuilder.RollbackTransaction();
            }
            finally
            {
                TransactionBuilder.DisposeTransaction();
            }

            return null;
        }
    }
  
    public class PersonCVManager2 : BaseEntityManager<IPersonCvDAL, PersonCv, PersonCvDto>, IPersonCVService
    {
        public PersonCVManager2(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Task<ServiceResult<List<PersonCvDto>>> GetWithInclude()
        {
            throw new NotImplementedException();
        }
    }
}
