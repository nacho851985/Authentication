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

        public async Task<User?> GetByNameAndPasswordAsync(string name, string password)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Username, name) &
                   Builders<User>.Filter.Eq(u => u.Password, password);


            var result = await _UserCollection.Find(filter)
                                           .FirstOrDefaultAsync();
            if (result != null)
            {
                Console.WriteLine("ID encontrado: " + result.Id);
            }
            else
            {
                Console.WriteLine("No se encontró el usuario.");
            }

            return result;
        }

        public Task UpdateAsync(string id, User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
