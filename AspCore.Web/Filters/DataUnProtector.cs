using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dependency.Concrete;
using AspCore.Utilities.DataProtector;

namespace AspCore.Web.Filters
{
    public class DataUnProtector : ActionFilterAttribute
    {
        private string _parameterName { get; set; }
        private IDataProtectorHelper _protectorHelper;
        public DataUnProtector(string parameterName)
        {
            _protectorHelper = DependencyResolver.Current.GetService<IDataProtectorHelper>();
            _parameterName = parameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.ContainsKey(_parameterName))
            {
                var value = context.ActionArguments[_parameterName].ToString();
                if (!string.IsNullOrEmpty(value) && value != "-1")
                {
                    context.ActionArguments[_parameterName] = _protectorHelper.UnProtect(context.ActionArguments[_parameterName].ToString());
                }
            }
        }
    }
}
