using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public class UserBffLayer : BaseAuthenticationBffLayer<AuthenticationInfo, ActiveUser>, IAuthenticationBffLayer<AuthenticationInfo, ActiveUser>
    {
        public override string authenticationRoute => "api/AuthenticationToken";

        public UserBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
