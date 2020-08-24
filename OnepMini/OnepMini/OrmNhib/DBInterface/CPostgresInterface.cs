
namespace OnepMini.OrmNhib.DBInterface
{
    using Microsoft.Extensions.DependencyInjection;
    using NHibernate;
    using NHibernate.Mapping;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class CPostgresInterface : ICPostgresInterface
    {
        private NpgsqlConnection _npgsqlConnection = null;
        private Dictionary<string, string> _tableNames = new Dictionary<string, string>();

        public CPostgresInterface(string DatabaseName, string connectionString)
        {
            DBName = DatabaseName;
            ConnectionString = connectionString;
        }

        public string DBName { get; set; }
        public string ConnectionString { get; set; }

        public NpgsqlConnection DBConnection
        {
            get
            {
                if (_npgsqlConnection == null)
                {
                    _npgsqlConnection = new NpgsqlConnection(ConnectionString);
                    _npgsqlConnection.Open();
                }
                return _npgsqlConnection;
            }
        }

        public void CreateDatabase()
        {
            //var strSQL = $"CREATE DATABASE {DBName} WITH ENCODING = 'UTF8' CONNECTION LIMIT = -1;";

            //var command = DBConnection.CreateCommand();
            //command.CommandText = strSQL;
            //command.CommandTimeout = 0;

            //try
            //{
            //    command.ExecuteNonQuery();
            //    command.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"Command failed: {strSQL}. Exception: {ex.ToString()}");
            //    command.Dispose();

            //    throw;
            //}

            var connectionStringWithoutDBName = "Host=localhost;Port=5432;Username=postgres;Password=password;";

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStringWithoutDBName))
            {
                using (NpgsqlCommand command = new NpgsqlCommand
                    (@"CREATE DATABASE OnepMini WITH ENCODING = 'UTF8' CONNECTION LIMIT = -1;", conn))
                {
                    try
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Create DB failed: {ex.ToString()}");
                        throw;
                    }
                }
            }
        }

        public bool DoesDatabaseExist()
        {
            //var strSQL = $"SELECT 1 FROM pg_database WHERE datname='{DBName}'";
            //try
            //{
            //    var command = DBConnection.CreateCommand();
            //    command.CommandText = strSQL;
            //    command.CommandTimeout = 0;

            //    var response = command.ExecuteScalar();
            //    int countDb = (int)response;
            //    command.Dispose();

            //    return (countDb == 0) ? false : true;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"Command failed: {strSQL}. Exception: {ex.ToString()}");
            //    return false;
            //}

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand
                    ($"SELECT DATNAME FROM pg_catalog.pg_database WHERE DATNAME = '{DBName}'", conn))
                {
                    try
                    {
                        conn.Open();
                        var i = command.ExecuteScalar();
                        conn.Close();
                        if (i.ToString().Equals(DBName)) //always 'true' (if it exists) or 'null' (if it doesn't)
                            return true;
                        else return false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Check for DB failed: {ex.ToString()}"); // ex = {"3D000: database \"OnepMini\" does not exist"}
                        return false;
                    }
                }
            }

        }

        private Dictionary<string, string> GetTableNames()
        {
            if (_tableNames.Count == 0)
            {
                var strSQL = "SELECT table_name FROM information_schema.tables WHERE table_type='BASE TABLE' AND table_schema='public';";

                var command = DBConnection.CreateCommand();
                command.CommandText = strSQL;
                command.CommandTimeout = 240;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tableName = reader["table_name"] as string;
                    tableName = tableName.ToLower();
                    if (!_tableNames.ContainsKey(tableName))
                    {
                        _tableNames.Add(tableName, tableName);
                    }
                }
                reader.Close();
                command.Dispose();
            }

            return _tableNames;
        }

        public bool DoesTableExist(string tableName)
        {
            tableName = tableName.ToLower();
            var tableNames = GetTableNames();

            return tableNames.ContainsKey(tableName);
        }

        public void ExecuteNonQuery(string sqlStatement)
        {
            //IQuery query = session.CreateSQLQuery(sqlstatement);
            //query.ExecuteUpdate();


            var command = DBConnection.CreateCommand();
            command.CommandText = sqlStatement;
            command.CommandTimeout = 0;

            try
            {
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Command failed: {sqlStatement}. Exception: {ex.ToString()}");
                command.Dispose();

                throw;
            }
        }
    }
}
