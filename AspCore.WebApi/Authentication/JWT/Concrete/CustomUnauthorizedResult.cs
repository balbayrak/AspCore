using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public class CustomUnauthorizedResult : JsonResult
    {
        public CustomUnauthorizedResult(string message)
            : base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
