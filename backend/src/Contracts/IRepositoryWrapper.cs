using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryWrapper
    {
        IAccountRepository Account { get; }

        IUserRepository User { get; }

        IStockRepository Stock { get; }
    }
}