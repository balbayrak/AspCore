using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace AspCore.Business.Validation
{
   public static class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            var context=new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                var arr = result.Errors.Select(x => $"{x.ErrorMessage}");
                var error= string.Join(string.Empty, arr);
                throw new ArgumentException(error);
            }
        }
    }
}
