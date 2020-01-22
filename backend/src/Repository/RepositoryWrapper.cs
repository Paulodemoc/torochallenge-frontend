using Contracts;
using Entities;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IDatabaseManager _dbmanager;
        private IAccountRepository _account;
        private IUserRepository _user;
        private IStockRepository _stock;

        public IAccountRepository Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepository(_dbmanager);
                }

                return _account;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_dbmanager);
                }

                return _user;
            }
        }

        public IStockRepository Stock
        {
            get
            {
                if (_stock == null)
                {
                    _stock = new StockRepository(_dbmanager);
                }

                return _stock;
            }
        }

        public RepositoryWrapper(IDatabaseManager dbmanager)
        {
            _dbmanager = dbmanager;
        }
    }
}