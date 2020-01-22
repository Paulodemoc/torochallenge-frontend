using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("funds")]
        public double Funds { get; set; }

        [ForeignKey(nameof(User))]
        [BsonElement("userid")]
        public string UserId { get; set; }

        public double Ammount { get; set; }
    }
}