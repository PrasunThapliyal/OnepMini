
namespace OnepMini.OrmNhib.DBInterface
{
    using Microsoft.Extensions.DependencyInjection;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class CPostgresInterface : ICPostgresInterface
    {
        private NpgsqlConnection _npgsqlConnection = null;
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
            var strSQL = $"CREATE DATABASE {DBName} WITH ENCODING = 'UTF8' CONNECTION LIMIT = -1;";

            var command = DBConnection.CreateCommand();
            command.CommandText = strSQL;
            command.CommandTimeout = 0;

            try
            {
                command.ExecuteNonQuery();
                command.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Command failed: {strSQL}. Exception: {ex.ToString()}");
                command.Dispose();

                throw;
            }
        }

        public bool DoesDatabaseExist()
        {
            var strSQL = $"SELECT 1 FROM pg_database WHERE datname='{DBName}'";

            var command = DBConnection.CreateCommand();
            command.CommandText = strSQL;
            command.CommandTimeout = 0;

            try
            {
                var response = command.ExecuteScalar();
                int countDb = (int)response;
                command.Dispose();

                return (countDb == 0) ? false : true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Command failed: {strSQL}. Exception: {ex.ToString()}");
                command.Dispose();

                throw;
            }

        }
    }
}
