using NHibernate.Cfg;
using OnepMini.OrmNhib.Initializer;
using OnepMini.OrmNhib.Migrations;
using OnepMini.OrmNhib.Util;
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
        private readonly IUtils _utils;
        private List<NHMigrationsBase> _orderedListOfMigrations = new List<NHMigrationsBase>();

        public NHMigrationRunner(
            INHibernateInitializer nHibernateInitializer,
            IUtils utils)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._utils = utils ?? throw new ArgumentNullException(nameof(utils));
            this._configuration = _nHibernateInitializer.GetConfiguration();
            {
                var m = new InitialCreate(_configuration, _nHibernateInitializer, _utils);
                AddMigration(m);
            }
            {
                var m = new MigrateTo_0_0_17_1002(_configuration, _nHibernateInitializer, _utils);
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
            // And we have a record of migrations already run
            // TODO : This is how we decide whether to run Up or Down, and for how long
            // Let X be the current 1P backend version
            // Let Y be the last 1P backend version in NHMigrationsHistory table
            // Now
            // if X > Y,
            //          we call Up for Y+1, Y+2 until we reach X
            // If X < Y,
            //          we call Down from Y, Y-1, Y-2 until we reach X

            foreach (var m in _orderedListOfMigrations)
            {
                m.Up();
            }

            var isValid = _nHibernateInitializer.ValidateSchema();
            if (!isValid)
            {
                throw new ApplicationException("Migrations failed");
            }
        }

    }
}
