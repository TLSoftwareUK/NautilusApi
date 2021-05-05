using System.Linq;
using System.Security.Claims;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class ApiUser
    {
        /*public ApiUser(ServerCallContext context)
        {
            var user = context.GetHttpContext().User;
            UserId = user.Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Select(c => c.Value).SingleOrDefault();
        }*/
        
        public ApiUser(ClaimsPrincipal user)
        {
            UserId = user.Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Select(c => c.Value).SingleOrDefault();
        }
        
        public string UserId { get; private set; }
    }
}
