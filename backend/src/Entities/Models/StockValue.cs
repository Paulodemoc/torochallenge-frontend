using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class StockValue
    {
        public string StockCode { get; set; }
        public double Value { get; set; }
        public double Timestamp { get; set; }
    }
}