using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SyncTables
{
    public class SqlDestination : IDestination
    {
        private readonly string connect;
        private readonly string name;
        private readonly string truncate;

        public SqlDestination(TableDef tableDef)
        {
            connect = tableDef.Connection;
            name = tableDef.Name;
            truncate = $"TRUNCATE TABLE [{tableDef.Name.Replace(".", "].[")}];";
        }

        private SqlCommand GetDelete(SqlConnection connection, SqlTransaction transaction)
        {
            //var cmd = 
            return new SqlCommand(truncate, connection, transaction);
        } // END GetDelete

        private SqlBulkCopy GetBulkCopy(SqlConnection connection, SqlTransaction transaction)
        {
            var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction);
            bulk.DestinationTableName = name;
            //bulk.ColumnMappings.Add("Full Name", "Name");
            return bulk;
        } // END GetBulkCopy

        public void Set(DataTable table)
        {
            using (var conn = new SqlConnection(connect))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // truncate
                        using (var cmd = GetDelete(conn, trans))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        // load
                        using (var bulk = GetBulkCopy(conn, trans))
                        {
                            bulk.WriteToServer(table);
                            trans.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }
        } // END Set

        public async Task SetAsync(DataTable table)
        {
            using (var conn = new SqlConnection(connect))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        // truncate
                        using (var cmd = GetDelete(conn, trans))
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        // load
                        using (var bulk = GetBulkCopy(conn, trans))
                        {
                            await bulk.WriteToServerAsync(table);
                            trans.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                    }
                }
            }
        } // END SetAsync
    }
}
