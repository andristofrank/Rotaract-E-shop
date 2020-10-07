using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using RotaractServer.Data;

namespace RotaractServer.Authorization
{
    public class RotaractAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var _repo = new RotaractServrerContext())
            {

                var user = _repo.Users.FirstOrDefault(n => n.Username == context.UserName && n.Password == context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "Provided Username and password is incorrect");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));
                identity.AddClaim(new Claim("Username", user.Username));
                identity.AddClaim(new Claim("UserId", user.UserId.ToString()));
                identity.AddClaim(new Claim("FirstName", user.FirstName));
                identity.AddClaim(new Claim("LastName", user.LastName));
                identity.AddClaim(new Claim("Email", user.Email));
                identity.AddClaim(new Claim("DistrictNo", user.DistrictNumber.ToString()));
                context.Validated(identity);
            }
        }
    }
}