using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public class CustomError
    {
        public string Error { get; }

        public CustomError(string message)
        {
            Error = message;
        }
    }
}
