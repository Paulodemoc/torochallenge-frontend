using Contracts;

namespace StocksService
{
    public class DependenciesManager : IDependenciesManager
    {
        public string stocksendpoint { get; set; }
    }
}