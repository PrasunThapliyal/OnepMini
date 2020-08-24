using NHibernate;
using OnepMini.ETP.OnePlannerMigrations.Framework;
using OnepMini.OrmNhib.Initializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.ETP
{
    public class NetworkDBStorageInitializer : INetworkDBStorageInitializer
    {
        private readonly INHibernateInitializer _nHibernateInitializer;
        private readonly ISessionFactory _sessionFactory;
        private readonly INHMigrationRunner _nhMigrationRunner;

        public NetworkDBStorageInitializer(
            INHibernateInitializer nHibernateInitializer,
            ISessionFactory sessionFactory,
            INHMigrationRunner nhMigrationRunner)
        {
            this._nHibernateInitializer = nHibernateInitializer ?? throw new ArgumentNullException(nameof(nHibernateInitializer));
            this._sessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            this._nhMigrationRunner = nhMigrationRunner ?? throw new ArgumentNullException(nameof(nhMigrationRunner));
        }

        public void RunMigrations()
        {
            _nhMigrationRunner.RunMigrations();
        }
    }
}
