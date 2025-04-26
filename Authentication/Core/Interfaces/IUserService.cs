using Authentication.Core.Models;

namespace Authentication.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByNameAndPasswordAsync(string name, string password);
        Task CreateAsync(User newProducto);
        Task UpdateAsync(string id, User updatedUser);
        Task DeleteAsync(string id);
    }
}
