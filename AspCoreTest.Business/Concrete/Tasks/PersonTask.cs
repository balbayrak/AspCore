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

        }
        public override bool RunWithTransaction => false;

        public override List<ITaskValidator> Validators => new List<ITaskValidator> { new PersonTaskValidator(this.Entity), new PersonTaskValidator2(this.Entity) };

    }
}
