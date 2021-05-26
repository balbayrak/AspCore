using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspCore.Business.Validation;
using FluentValidation;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace AspCoreTest.Business.Interceptors
{
    [PSerializable]
    public class ValidationAspect : OnMethodBoundaryAspect
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            _validatorType = validatorType;
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = args.Arguments.Where(t=>t.GetType()==entityType).ToList();
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
