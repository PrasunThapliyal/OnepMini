using NHibernate;
using NHibernate.Cfg;

namespace OnepMini.OrmNhib.Initializer
{
    public interface INHibernateInitializer
    {
        void CreateDatabase();
        bool DoesDatabaseExist();
        void ExecuteNonQuery(string sqlStatement);
        void ExportSchemaFile(string onepBackendVersion);
        Configuration GetConfiguration();
        ISessionFactory GetSessionFactory();
        bool ValidateSchema();
    }
}