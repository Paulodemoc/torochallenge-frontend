using Entities.Models;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IStockRepository
    {
        List<Stock> Get(string userId);

        Stock Get(string userId, string stockCode);

        Stock Create(Stock acc);

        void Update(string userId, Stock accIn);

        void Remove(Stock accIn);

        void Remove(string userId);

        void Remove(string userId, string stockCode);
    }
}