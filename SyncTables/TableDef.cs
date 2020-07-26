using System;
using System.Collections.Generic;
using System.Data;

namespace SyncTables
{
    public class TableDef
    {
        public string Connection { get; set; }
        public string Name { get; set; }

        public List<string> Columns { get; set; } = new List<string>();

        //public DataTable Table { get; }
    }
}
