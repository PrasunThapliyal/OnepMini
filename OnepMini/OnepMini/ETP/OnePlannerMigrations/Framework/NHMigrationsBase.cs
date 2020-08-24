using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.ETP.OnePlannerMigrations.Framework
{
    public class NHMigrationsBase
    {
        public Configuration Configuration { get; }

        public NHMigrationsBase(NHibernate.Cfg.Configuration configuration)
        {
            Configuration = configuration;
        }

        public virtual void Up()
        {

        }
        public virtual void Down()
        {

        }
    }
}
