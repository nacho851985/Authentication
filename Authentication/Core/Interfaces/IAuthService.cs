using Authentication.Core.Models;

namespace Authentication.Core.Interfaces
{
    public interface IAuthService
    {
        public Token Authenticate(User user);
    }
}
