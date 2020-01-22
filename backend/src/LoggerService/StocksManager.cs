using Contracts;
using Entities.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace StocksService
{
    public class StocksManager : IStocksManager
    {
        public List<StockValue> stocksValues { get; }

        public StocksManager(IDependenciesManager _dependencies)
        {
            stocksValues = new List<StockValue>();
            Task.Run(() =>
            {
                using (var client = new WebsocketClient(new Uri(_dependencies.stocksendpoint)))
                {
                    client.ReconnectTimeout = TimeSpan.FromSeconds(30);
                    //client.ReconnectionHappened.Subscribe(info =>
                    //    Log.Information($"Reconnection happened, type: {info.Type}"));

                    client.MessageReceived.Subscribe(msg =>
                    {
                        try
                        {
                            var value = JsonConvert.DeserializeObject<Dictionary<string, double>>(msg.ToString());
                            StockValue stock = stocksValues.FirstOrDefault(s => s.StockCode.Equals(value.Keys.ElementAt(0))) ?? new StockValue { StockCode = value.Keys.ElementAt(0) };
                            if (stock.Timestamp < value["timestamp"])
                            {
                                stock.Value = value[stock.StockCode];
                                stock.Timestamp = value["timestamp"];
                                if (!stocksValues.Any(s => s.StockCode.Equals(value.Keys.ElementAt(0))))
                                    stocksValues.Add(stock);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    });
                    client.Start();
                    new ManualResetEvent(false).WaitOne();
                }
            });
        }
    }
}