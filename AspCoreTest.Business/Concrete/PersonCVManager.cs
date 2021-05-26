using AspCore.Business.Concrete;
using AspCore.DataAccess.General;
using AspCore.Dependency.DependencyAttributes;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Entities.Expression;
using AspCore.Utilities;

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
            dataFilter.sorter = new SortingExpression<PersonCv>(t => t.Name, EnumSortingDirection.Descending);
            dataFilter.page = 0;
            dataFilter.pageSize = 10;
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
