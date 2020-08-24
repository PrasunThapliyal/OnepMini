using NHibernate;
using NHibernate.Tool.hbm2ddl;
using OnepMini.OrmNhib.DBInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OnepMini.OrmNhib.Initializer
{
    // This is akin to OnePlannerORM
    public class NHibernateInitializer : INHibernateInitializer
    {
        private NHibernate.Cfg.Configuration _configuration = null;
        private ISessionFactory _sessionFactory = null;
        private CPostgresInterface _postgresInterface = null;

        public NHibernateInitializer()
        {
            _configuration = GetConfiguration();

            _postgresInterface = new CPostgresInterface("OnepMini", _configuration.GetProperty("connection.connection_string"));
        }

        public NHibernate.Cfg.Configuration GetConfiguration()
        {
            if (_configuration != null)
            {
                return _configuration;
            }

            var cfg = new NHibernate.Cfg.Configuration();
            {
                // TODO : To cater to P2 scenario where we have Ref and Proj DBs
                // We need to create config through code and not via cfg.xml so that we are able to
                // pass the connection string as a parameter.
                // For now, we continue with just a single DB.
                // This wont be a big change.

                cfg.Configure(@"OrmNhib/NHibernateConfig/hibernate.cfg.xml");

                foreach (var mapping in cfg.ClassMappings)
                {
                    string x = $"(1) {mapping.ClassName}, (2) {mapping.Discriminator}, (3) {mapping.DiscriminatorValue}, (4) {mapping.IsDiscriminatorValueNotNull}";
                    System.Diagnostics.Debug.WriteLine(x);
                }

            }

            _configuration = cfg;
            return _configuration;
        }

        public void CreateDatabase()
        {
            if (!_postgresInterface.DoesDatabaseExist())
            {
                _postgresInterface.CreateDatabase();
            }
        }

        public bool DoesDatabaseExist()
        {
            return _postgresInterface.DoesDatabaseExist();
        }

        public void ExportSchemaFile(string onepBackendVersion)
        {
            NHibernate.Cfg.Configuration cfg = _configuration;

            var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
            schemaExport.SetOutputFile($"Postgres.{onepBackendVersion}.Snapshot.sql").Execute(
                useStdOut: true, execute: false, justDrop: false);
        }

        private void CreateSchemaInEmptyDatabase()
        {
            // Expect an Empty Database to be present
            if (! _postgresInterface.DoesDatabaseExist())
            {
                // Database doesn't exist
                // Lets call the Initial Migration
            }
            else
            {
                // Database already exists

                // We need to check if any previous migrations were run, and then run the remaining ones
                // as a corollary, we need to be able to define the sequence of migrations

            }


            // Note
            // SchemaUpdate.Execute is way cooler than SchemaExport.Filename.Execute
            // When I added a new property in Movie.hbm.xml (and in the .cs), SchemaUpdate automatically created statement
            // to tell the diff in schema, and only this got executed:
            /*
                alter table Movie
                    add column NewProp VARCHAR(255)
             *
             * */
            //
            // However, it does not work as expected all the times, for eg,
            // if I rename a column in HBM, it just adds a new column with new name
            // if I change the sql-type from VARCHAR(255) to VARCHAR(100), nothing is executed and the column type remains unchanged
            // if we add new indexes, it wont pickup (meaning for existing tables it wont create an index, but it'll create for new tables).
            // What about column delete
            // So we will need manual scripts for migration
            //
        }

        public bool ValidateSchema()
        {
            try
            {
                NHibernate.Cfg.Configuration cfg = _configuration;
                SchemaValidator schemaValidator = new SchemaValidator(cfg);
                schemaValidator.Validate();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex}");
                return false;
            }
        }

        public ISessionFactory GetSessionFactory()
        {
            // Note: Troubleshooting
            // At some point here, I was getting error which got resolved due to this:
            // https://stackoverflow.com/questions/35444487/how-to-use-sqlclient-in-asp-net-core
            //            Instead of referencing System.Data and System.Data.SqlClient you need to grab from Nuget:
            //            System.Data.Common and System.Data.SqlClient


            if (_sessionFactory != null)
            {
                return _sessionFactory;
            }

            NHibernate.Cfg.Configuration cfg = _configuration;
            _sessionFactory = cfg.BuildSessionFactory();

            return _sessionFactory;
        }

        public void ExecuteNonQuery(string sqlStatement)
        {
            _postgresInterface.ExecuteNonQuery(sqlStatement);
        }

    }
}
