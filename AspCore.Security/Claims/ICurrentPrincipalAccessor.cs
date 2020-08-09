using AspCore.Dependency.Abstract;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AspCore.Security.Claims
{
    public interface ICurrentPrincipalAccessor : ISingletonType
    {
        ClaimsPrincipal Principal { get; }
    }
}
