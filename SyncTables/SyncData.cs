using System;

namespace SyncTables
{
    class SyncData
    {
        public int Id { get; }

        public ISource Source { get; }
        public IDestination Destination { get; }

        public DateTime LastRun { get; set; }
        public int LastCount { get; set; }

        public SyncData(int id, ISource source, IDestination destination)
        {
            Id = id;
            Source = source;
            Destination = destination;
        }
    }
}
