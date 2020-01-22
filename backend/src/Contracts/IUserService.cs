using Entities.Models;
using System.Collections.Generic;

namespace Contracts
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
    }
}