using OnepMini.ETP.OnePlannerMigrations.Framework;
using OnepMini.OrmNhib.Initializer;
using OnepMini.OrmNhib.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace OnepMini.ETP.OnePlannerMigrations
{
    public class MigrateTo_0_0_17_1002 : NHMigrationsBase
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly IUtils _utils;

        public MigrateTo_0_0_17_1002(
            NHibernate.Cfg.Configuration configuration,
            INHibernateInitializer nHibernateInitializer,
            IUtils utils)
            :base(configuration)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._utils = utils ?? throw new ArgumentNullException(nameof(utils));
            FromVersion = _utils.GetLastMigrated1PBackendVersion();
            ToVersion = _utils.GetCurrent1PBackendVersion();
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

                _nHibernateInitializer.ExportSchemaFile(ToVersion);

                var sqlStatement = ""; // Get this from the Diff of snapshot of previous to current
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
            }
        }

        private void LogAutoMigration(string sql)
        {
            using (var file = new FileStream($"Postgres.{ToVersion}.AutoMigrateTo.sql", FileMode.Append))
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
