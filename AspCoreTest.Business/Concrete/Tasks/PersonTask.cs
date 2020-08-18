using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;

namespace AspCoreTest.Business.Concrete.Tasks
{
    public class PersonTask : EntityTask<Person, bool, IPersonDal>, ITask
    {
        public PersonTask(IServiceProvider serviceProvider, Person person, EnumCrudOperation enumCrudOperation) : base(serviceProvider, person, enumCrudOperation)
        {
            this.AddValidator(new PersonTaskValidator(person));
            this.AddValidator(new PersonTaskValidator2(person));
        }
        public override bool RunWithTransaction => false;

    }
}
