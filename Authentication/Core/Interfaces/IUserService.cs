using Authentication.Core.Models;

namespace Authentication.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task CreateAsync(User newProducto);
        Task UpdateAsync(string id, User updatedUser);
        Task DeleteAsync(string id);
    }
}
