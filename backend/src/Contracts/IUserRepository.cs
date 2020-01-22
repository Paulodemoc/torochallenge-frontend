using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        List<User> Get();

        User Get(string id);

        User Get(string username, string password);

        User Create(User user);

        void Update(string id, User userIn);

        void Remove(User userIn);

        void Remove(string id);
    }
}