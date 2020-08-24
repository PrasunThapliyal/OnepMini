using OnepMini.ETP.OnePlannerMigrations.Framework;
using OnepMini.OrmNhib.Initializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OnepMini.ETP.OnePlannerMigrations
{
    public class MigrateTo : NHMigrationsBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;

        public MigrateTo(
            NHibernate.Cfg.Configuration configuration,
            INHibernateInitializer nHibernateInitializer)
            :base(configuration)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
        }

        public override void Up()
        {
            base.Up();

            if (_nHibernateInitializer.DoesDatabaseExist())
            {
                // Create DB
                // TODO : Get 1P Backend version used in current ETP
                // TODO : Create a model snapshot file (*.SQL)
                // TODO : Update Migrations History table

                var currentOnepBackendVersion = "0.0.17.1009";

                _nHibernateInitializer.ExportSchemaFile(currentOnepBackendVersion);

                NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(Configuration);
                schemaUpdate.Execute(LogAutoMigration, doUpdate: true);

            }
        }

        private static void LogAutoMigration(string sql)
        {
            using (var file = new FileStream(@"update.sql", FileMode.Append))
            {
                using (var sw = new StreamWriter(file))
                {
                    sw.Write(sql);
                }
            }
        }

        public override void Down()
        {
            base.Down();
        }
    }
}
