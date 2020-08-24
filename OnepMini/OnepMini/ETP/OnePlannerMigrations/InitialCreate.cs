using NHibernate.Cfg;
using OnepMini.OrmNhib.Initializer;
using OnepMini.ETP.OnePlannerMigrations.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnepMini.OrmNhib.Util;

namespace OnepMini.ETP.OnePlannerMigrations
{
    public class InitialCreate : NHMigrationsBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly IUtils _utils;

        public InitialCreate(
            NHibernate.Cfg.Configuration configuration,
            INHibernateInitializer nHibernateInitializer,
            IUtils utils) :
            base(configuration)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._utils = utils ?? throw new ArgumentNullException(nameof(utils));
            FromVersion = string.Empty;
            ToVersion = utils.GetCurrent1PBackendVersion();
        }

        public override void Up()
        {
            base.Up();

            if (!_nHibernateInitializer.DoesDatabaseExist())
            {
                // Create DB
                // TODO : Get 1P Backend version used in current ETP
                // TODO : Create a model snapshot file (*.SQL)
                // TODO : This migration code
                // TODO : Validate Schema() should not be called from here .. it should be called after the last migration is executed
                // TODO : Update Migrations History table

                var currentOnepBackendVersion = ToVersion; // Eg "0.0.17.1001"

                _nHibernateInitializer.ExportSchemaFile(currentOnepBackendVersion);

                _nHibernateInitializer.CreateDatabase();

                NHibernate.Tool.hbm2ddl.SchemaUpdate schemaUpdate = new NHibernate.Tool.hbm2ddl.SchemaUpdate(Configuration);
                schemaUpdate.Execute(useStdOut: true, doUpdate: true);
            }
        }

        public override void Down()
        {
            base.Down();

            _nHibernateInitializer.
        }
    }
}
