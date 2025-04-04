using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Authentication.Core.Interfaces;

namespace Authentication.Core.Models
{
    public class User
    {
        private bool isAdmin1;

        [BsonId] // Marca esta propiedad como el _id de MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Le dice al driver que lo trate como un ObjectId
        public string? Id { get; set; } // Puede ser string o ObjectId directamente

        [BsonElement("username")] // Opcional: si el nombre en C# es diferente al de MongoDB
        public string Username { get; set; } = null!; // Non-nullable string

        [BsonElement("password")] // Opcional: si el nombre en C# es diferente al de MongoDB
        public string Password { get; set; } = null!; // Non-nullable string

        [BsonElement("isAdmin")] // Opcional: si el nombre en C# es diferente al de MongoDB
        public bool isAdmin { get => isAdmin1; set => isAdmin1 = value; }
        //public string Username { get; set; }
        //public string Password { get; set; }
    }
}
