using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.ApiAuthentication.JWT.Abstract
{
    public interface ITokenGenerator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        ServiceResult<AuthenticationToken> CreateToken(TJWTInfo jwtInfo);

        ServiceResult<AuthenticationToken> RefreshToken(AuthenticationToken token);

        ServiceResult<TJWTInfo> GetJWTInfo(AuthenticationToken token);
    }
}
