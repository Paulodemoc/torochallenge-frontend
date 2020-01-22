using Contracts;

namespace StocksService
{
    public class DatabaseManager : IDatabaseManager
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}