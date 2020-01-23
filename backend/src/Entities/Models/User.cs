using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [BsonElement("password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        public string Token { get; set; }

        public Account Account { get; set; }

        public Stock[] Portfolio { get; set; }
    }
}