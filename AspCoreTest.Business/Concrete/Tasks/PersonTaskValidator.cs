using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCoreTest.Business.Concrete.Tasks
{
    public class PersonTaskValidator : TaskValidator<Person>, ITaskValidator
    {
        public PersonTaskValidator(Person person) : base(person)
        {

        }

        public override Task<ServiceResult<bool>> Validate()
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>();
            if (!Input.Name.StartsWith("Bilal"))
            {
                serviceResult.IsSucceeded = false;
                serviceResult.Result = false;
                serviceResult.ErrorMessage = "Name must startwith Bilal";
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
