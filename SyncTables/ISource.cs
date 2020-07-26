using System;
using System.Data;
using System.Threading.Tasks;

namespace SyncTables
{
    public interface ISource
    {
        DataTable Get();
        Task<DataTable> GetAsync();
    }
}
