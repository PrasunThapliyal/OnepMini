using NHibernate.Cfg;
using OnepMini.OrmNhib.Initializer;
using OnepMini.OrmNhib.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.ETP.OnePlannerMigrations.Framework
{
    public class NHMigrationRunner : INHMigrationRunner
    {
        private readonly Configuration _configuration;
        private readonly INHibernateInitializer _nHibernateInitializer;

        private List<NHMigrationsBase> _orderedListOfMigrations = new List<NHMigrationsBase>();

        public NHMigrationRunner(
            INHibernateInitializer nHibernateInitializer)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));

            this._configuration = _nHibernateInitializer.GetConfiguration();
            {
                var m = new InitialCreate(_configuration, _nHibernateInitializer);
                AddMigration(m);
            }
            {
                var m = new MigrateTo(_configuration, _nHibernateInitializer);
                AddMigration(m);
            }
        }

        private void AddMigration(NHMigrationsBase migration)
        {
            _orderedListOfMigrations.Add(migration);
        }

        public void RunMigrations()
        {
            // This is an ordered sequence or migrations

            foreach (var m in _orderedListOfMigrations)
            {
                m.Up();
            }
        }

    }
}
