using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OnepMini.OrmNhib.Initializer
{

    public class NHibernateInitializer : INHibernateInitializer
    {
        private NHibernate.Cfg.Configuration _configuration = null;
        private ISessionFactory _sessionFactory = null;

        public NHibernate.Cfg.Configuration GetConfiguration()
        {
            if (_configuration != null)
            {
                return _configuration;
            }

            var cfg = new NHibernate.Cfg.Configuration();
            {
                cfg.Configure(@"OrmNhib/NHibernateConfig/hibernate.cfg.xml");

                foreach (var mapping in cfg.ClassMappings)
                {
                    string x = $"(1) {mapping.ClassName}, (2) {mapping.Discriminator}, (3) {mapping.DiscriminatorValue}, (4) {mapping.IsDiscriminatorValueNotNull}";
                    System.Diagnostics.Debug.WriteLine(x);
                }

                var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
                schemaExport.SetOutputFile(@"db.Postgre.sql").Execute(
                    useStdOut: true, execute: true, justDrop: false);

                // Alternately, we can use SchemaUpdate.Execute, as in done in 1P
                NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg);
                schemaUpdate.Execute(useStdOut: true, doUpdate: true);

                try
                {
                    SchemaValidator schemaValidator = new SchemaValidator(cfg);
                    schemaValidator.Validate();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Exception: {ex}");
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
                // So we will need manual scripts for migration
                //
            }

            _configuration = cfg;
            return _configuration;
        }

        public ISessionFactory GetSessionFactory(NHibernate.Cfg.Configuration cfg)
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

            _sessionFactory = cfg.BuildSessionFactory();

            return _sessionFactory;
        }
    }
}
