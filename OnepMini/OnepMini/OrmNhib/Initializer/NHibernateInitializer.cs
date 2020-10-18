using NHibernate;
using NHibernate.Cfg;
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

            var connectionString = "Host=localhost;Port=5432;Database=reportsmini;Username=postgres;Password=password;";
            var cfg = new NHibernate.Cfg.Configuration();
            
            cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionString, connectionString);
            cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
            cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, "OnepMini.OrmNhib.OnepNpgsqlDriver, OnepMini");
            cfg.SetProperty(NHibernate.Cfg.Environment.Dialect, "OnepMini.OrmNhib.OnepPostgresSqlDialect, OnepMini");
            cfg.SetProperty(NHibernate.Cfg.Environment.BatchSize, "0");
            cfg.SetProperty(NHibernate.Cfg.Environment.ShowSql, "true");
            cfg.SetProperty(NHibernate.Cfg.Environment.FormatSql, "true");
            cfg.AddAssembly("OnepMini");

            {
                foreach (var mapping in cfg.ClassMappings)
                {
                    System.Diagnostics.Debug.WriteLine(mapping.ClassName);
                }

                var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(cfg);
                schemaExport.SetOutputFile(@"db.Postgre.sql").Execute(
                    useStdOut: true, execute: false, justDrop: false);

                // Alternately, we can use SchemaUpdate.Execute, as is done in 1P
                try
                {
                    NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(cfg);
                    schemaUpdate.Execute(useStdOut: true, doUpdate: true);
                }
                catch (Npgsql.PostgresException ex)
                {
                    Debugger.Break();
                    if (ex.ErrorCode == 42710)
                    {
                        Console.WriteLine($"Ignoring exception: {ex}");
                    }
                    else
                    {
                        throw;
                    }
                }

                try
                {
                    SchemaValidator schemaValidator = new SchemaValidator(cfg);
                    schemaValidator.Validate();
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    Debug.WriteLine($"Exception: {ex}");
                }

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
