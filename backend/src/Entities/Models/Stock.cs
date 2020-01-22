using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Stock Code is required")]
        [BsonElement("stockcode")]
        public string StockCode { get; set; }

        [Required(ErrorMessage = "Ammount is required")]
        [BsonElement("ammount")]
        public int Ammount { get; set; }

        [ForeignKey(nameof(User))]
        [BsonElement("userid")]
        public string UserId { get; set; }
    }
}