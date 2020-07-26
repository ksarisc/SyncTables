using System;
using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace SyncTables
{
    public class OdbcSource: ISource
    {
        private readonly TableDef def;
        private readonly string select;

        public OdbcSource(TableDef tableDef)
        {
            def = tableDef;
            // columns?
            if (def.Columns != null && def.Columns.Count > 0)
                select = $"SELECT \"{String.Join("\", \"", def.Columns)}\" FROM {def.Name}";
            else
                select = $"SELECT * FROM {def.Name}";
            // where clause?
        }

        private OdbcCommand GetCommand(OdbcConnection connection)
        {
            return new OdbcCommand(select, connection);
        }

        public DataTable Get()
        {
            using (var conn = new OdbcConnection(def.Connection))
            using (var cmd = GetCommand(conn))
            {
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(rdr);
                    return table;
                }
            }
        } // END Get

        public async Task<DataTable> GetAsync()
        {
            using (var conn = new OdbcConnection(def.Connection))
            using (var cmd = GetCommand(conn))
            {
                await conn.OpenAsync();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    var table = new DataTable();
                    table.Load(rdr);
                    //table.BeginLoadData
                    return table;
                }
            }
        } // END GetAsync
    }
}
