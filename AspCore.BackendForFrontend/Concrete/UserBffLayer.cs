using AspCore.ApiClient.Entities.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using System;
using System.Threading.Tasks;

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
