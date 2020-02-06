using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Authentication.Abstract;

namespace AspCore.Authentication.Concrete
{
    public abstract class WebAuthenticationProvider<TApiAuthenticationProvider>
        where TApiAuthenticationProvider : class, IAuthenticationService, new()
    {
        public string apiAuthenticationType
        {
            get
            {
                return typeof(TApiAuthenticationProvider).Name;
            }
        }
    }
}
