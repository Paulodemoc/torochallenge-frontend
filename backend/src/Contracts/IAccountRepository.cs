using Entities.Models;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IAccountRepository
    {
        public Account Get(string userId);

        public Account Create(Account acc);

        public void Update(string userId, Account accIn);

        public void Remove(Account accIn);

        public void Remove(string userId);
    }
}