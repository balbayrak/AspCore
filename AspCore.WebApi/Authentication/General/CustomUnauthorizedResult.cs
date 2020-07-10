using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspCore.WebApi.Authentication.General
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
