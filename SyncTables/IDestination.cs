using System;
using System.Data;
using System.Threading.Tasks;

namespace SyncTables
{
    public interface IDestination
    {
        // should it be possible to set the connection and/or transaction here too?
        void Set(DataTable table);
        Task SetAsync(DataTable table);
    }
}
