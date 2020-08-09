using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Web.Configuration.Options
{
    public class AuthenticationControllerOption
    {
        public string ControllerName { get; set;}

        public bool SameDomain { get; set; }
    }
}
