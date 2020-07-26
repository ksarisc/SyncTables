using System;

namespace SyncTables
{
    public interface IScheduler
    {
        int Add(ISource source, IDestination destination);
        void Remove(int process);
        void Start();
        void End();
    }
}
