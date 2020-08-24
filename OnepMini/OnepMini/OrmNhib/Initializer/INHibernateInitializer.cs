using NHibernate;
using NHibernate.Cfg;

namespace OnepMini.OrmNhib.Initializer
{
    public interface INHibernateInitializer
    {
        void CreateSchemaInEmptyDatabase();
        bool DoesDatabaseExist();
        void ExportSchemaFile(string onepBackendVersion);
        Configuration GetConfiguration();
        ISessionFactory GetSessionFactory();
        bool ValidateSchema();
    }
}