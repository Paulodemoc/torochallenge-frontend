using Contracts;
using Entities;
using Entities.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly IMongoCollection<Stock> _stocks;
        private readonly string CollectionName = "Stocks";

        public StockRepository(IDatabaseManager settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _stocks = database.GetCollection<Stock>(this.CollectionName);
        }

        public List<Stock> Get(string userId) =>
            _stocks.Find(acc => acc.UserId.Equals(userId)).ToList();

        public Stock Get(string userId, string stockCode) =>
            _stocks.Find(acc => acc.UserId.Equals(userId) && acc.StockCode.Equals(stockCode)).FirstOrDefault();

        public Stock Create(Stock acc)
        {
            _stocks.InsertOne(acc);
            return acc;
        }

        public void Update(string userId, Stock accIn) =>
            _stocks.ReplaceOne(acc => acc.UserId == userId, accIn);

        public void Remove(Stock accIn) =>
            _stocks.DeleteOne(acc => acc.UserId == accIn.UserId);

        public void Remove(string userId) =>
            _stocks.DeleteMany(acc => acc.UserId == userId);

        public void Remove(string userId, string stockCode) =>
            _stocks.DeleteOne(acc => acc.UserId == userId && acc.StockCode == stockCode);
    }
}