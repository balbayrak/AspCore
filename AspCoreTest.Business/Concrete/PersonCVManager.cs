﻿using AspCore.Business.Concrete;
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

namespace testbusiness.Concrete
{
    [ExposedService(typeof(IPersonCVService))]
    public class PersonCVManager : BaseEntityManager<IPersonCvDAL,PersonCv, PersonCvDto>, IPersonCVService
    {
        public PersonCVManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<ServiceResult<List<PersonCvDto>>> GetWithInclude()
        {
            var result =
               await DataAccess.GetListAsync(
                    t => t.Id == new Guid("e2845ff9-4f00-4c17-8e4b-0e5d8617197b") ||
                         t.Id == new Guid("7553c60d-3495-4132-a160-0ede96e50618"), t => t.Person);
            var data = AutoObjectMapper.Mapper.Map<List<PersonCvDto>>(result.Result);
            return result.ChangeResult(data);

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
