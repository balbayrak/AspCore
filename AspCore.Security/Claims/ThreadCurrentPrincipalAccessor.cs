using System.Security.Claims;
using System.Threading;

namespace AspCore.Security.Claims
{
    public class ThreadCurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        public ThreadCurrentPrincipalAccessor()
        {
                
        }
        public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;
    }
}
