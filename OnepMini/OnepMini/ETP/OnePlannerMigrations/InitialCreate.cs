using NHibernate.Cfg;
using OnepMini.OrmNhib.Initializer;
using OnepMini.ETP.OnePlannerMigrations.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.ETP.OnePlannerMigrations
{
    public class InitialCreate : NHMigrationsBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;

        public InitialCreate(
            NHibernate.Cfg.Configuration configuration,
            INHibernateInitializer nHibernateInitializer):
            base(configuration)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
        }

        public override void Up()
        {
            base.Up();

            if (!_nHibernateInitializer.DoesDatabaseExist())
            {
                // Create DB
                // TODO : Get 1P Backend version used in current ETP
                // TODO : Create a model snapshot file (*.SQL)
                // TODO : Update Migrations History table

                var currentOnepBackendVersion = "0.0.17.1001";

                _nHibernateInitializer.ExportSchemaFile(currentOnepBackendVersion);

                NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(Configuration);
                schemaUpdate.Execute(useStdOut: true, doUpdate: true);

            }
        }

        public override void Down()
        {
            base.Down();
        }
    }
}
