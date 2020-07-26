using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SyncTables
{
    public class SimpleScheduler : IScheduler
    {
        private readonly ConcurrentDictionary<int, SyncData> items = new ConcurrentDictionary<int, SyncData>();
        private int position = 0;

        public int Add(ISource source, IDestination destination)
        {
            int process = Interlocked.Increment(ref position);
            items.TryAdd(process, new SyncData(source, destination));
            return process; // -1;
        } // END Add

        public void Remove(int process)
        {
            items.TryRemove(process, out var item);
        } // END Remove

        public void Start()
        {
        } // END Start

        public void End()
        {
        } // END End

        private void Process()
        {
            while (true)
            {
                int index = -1;
                if (!items.TryGetValue(index, out var data))
                    continue;
                try
                {
                    //using (var table = await data.Source.GetAsync())
                    //{
                    //    await data.Destination.SetAsync(table);
                    //}
                    using (var table = data.Source.Get())
                    {
                        data.Destination.Set(table);
                    }
                }
                catch (Exception eRun)
                {
                }
            }
        } // END Process
    }
}
