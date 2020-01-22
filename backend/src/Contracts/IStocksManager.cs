using Entities.Models;
using System.Collections.Generic;

namespace Contracts
{
    public interface IStocksManager
    {
        public List<StockValue> stocksValues { get; }
    }
}