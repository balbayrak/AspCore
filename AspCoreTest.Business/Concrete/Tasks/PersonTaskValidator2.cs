using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCoreTest.Business.Concrete.Tasks
{
    public class PersonTaskValidator2 : TaskValidator<Person>, ITaskValidator
    {
        public PersonTaskValidator2(Person person) : base(person)
        {

        }

        public override Task<ServiceResult<bool>> Validate()
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>();
            if (!Input.Surname.StartsWith("Albay"))
            {
                serviceResult.IsSucceeded = false;
                serviceResult.Result = false;
                serviceResult.ErrorMessage = "Surname must startwith Albay";
            }
            else
            {
                serviceResult.IsSucceeded = true;
                serviceResult.Result = true;
            }

            return Task.FromResult(serviceResult);
        }
    }
}
