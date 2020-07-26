using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace SyncTables
{
    public class SampleProcessor
    {
        private static readonly int sleep = 10000;

        private bool running = false;

        private readonly string mainOdbcConnection = "";
        private readonly SqlConnection conn;
        private readonly SqlCommand get;
        private readonly SqlCommand update;
        private readonly SqlParameter updateId;

        //int Add(ISource source, IDestination destination);
        //void Remove(int process);

        private void Run()
        {
            while (running)
            {
                var start = DateTime.Now;
                var list = GetReadySyncs();
                foreach (var item in list)
                {
                }
                int diff = (int)(DateTime.Now - start).TotalMilliseconds;
                if (diff < sleep)
                    Thread.Sleep(sleep - diff);
            }
        } // END Run

        private IEnumerable<SyncData> GetReadySyncs()
        {
            var list = new List<SyncData>();
            conn.Open();
            using (var rdr = get.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (rdr.Read())
                {
                    int id = rdr.GetInt32(0);
                    var odbc = new OdbcSource(new TableDef
                    {
                        Connection = mainOdbcConnection,
                        Name = rdr.GetString(1)
                    });
                    var sql = new SqlDestination(new TableDef
                    {
                        Connection = conn.ConnectionString,
                        Name = rdr.GetString(2)
                    });

                    list.Add(new SyncData(id, odbc, sql));
                }
            }
            return null;
        } // END GetReadySyncs

        private void Synchronize(ISource source, IDestination destination, int id)
        {
            try
            {
                //using (var table = await data.Source.GetAsync())
                //{
                //    await data.Destination.SetAsync(table);
                //}
                using (var table = source.Get())
                {
                    destination.Set(table);

                    conn.Open();
                    updateId.Value = id;
                    update.ExecuteNonQuery();
                }
            }
            catch (Exception eRun)
            {
                log.ExecuteNonQuery();
            }
        } // END Synchronize

        public void Start()
        {
        } // END Start

        public void End()
        {
        } // END End
    }
}
