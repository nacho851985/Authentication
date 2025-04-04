using Authentication.Core.Interfaces;
using Authentication.Core.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options; 

namespace Authentication.Core.Services
{
    public class UserService(IMongoDatabase database) : IUserService
    {
        private readonly IMongoCollection<User> _UserCollection = database.GetCollection<User>("users");

        public Task CreateAsync(User newProducto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var userList = await _UserCollection.Find(_ => true).ToListAsync(); // Encuentra todos
            return userList;
        }

        public Task<User?> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
