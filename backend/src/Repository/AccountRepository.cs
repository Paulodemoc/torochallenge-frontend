using Contracts;
using Entities;
using Entities.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;
        private readonly string CollectionName = "Accounts";

        public AccountRepository(IDatabaseManager settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _accounts = database.GetCollection<Account>(this.CollectionName);
        }

        public Account Get(string userId) =>
            _accounts.Find(acc => acc.UserId.Equals(userId)).FirstOrDefault();

        public Account Create(Account acc)
        {
            _accounts.InsertOne(acc);
            return acc;
        }

        public void Update(string userId, Account accIn) =>
            _accounts.ReplaceOne(acc => acc.UserId == userId, accIn);

        public void Remove(Account accIn) =>
            _accounts.DeleteOne(acc => acc.UserId == accIn.UserId);

        public void Remove(string userId) =>
            _accounts.DeleteOne(acc => acc.UserId == userId);
    }
}