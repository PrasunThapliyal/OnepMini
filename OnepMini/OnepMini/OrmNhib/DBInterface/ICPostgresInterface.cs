using NHibernate;
using Npgsql;
using System;

namespace OnepMini.OrmNhib.DBInterface
{
    public interface ICPostgresInterface
    {
        string ConnectionString { get; set; }
        NpgsqlConnection DBConnection { get; }
        string DBName { get; set; }

        void CreateDatabase();
        bool DoesDatabaseExist();
        bool DoesTableExist(string tableName);
        void ExecuteNonQuery(string sqlStatement);
    }
}