using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IDatabaseManager
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}