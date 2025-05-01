using Amazon.Lambda.APIGatewayEvents;
using Authentication.Core.Models;

namespace Authentication.Core.Interfaces
{
    public interface IAuthService
    {
        public APIGatewayProxyResponse Authenticate(User user);
    }
}
