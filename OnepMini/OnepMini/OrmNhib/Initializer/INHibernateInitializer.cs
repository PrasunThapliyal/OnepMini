using NHibernate;
using NHibernate.Cfg;

namespace OnepMini.OrmNhib.Initializer
{
    public interface INHibernateInitializer
    {
        Configuration GetConfiguration();
        ISessionFactory GetSessionFactory(Configuration cfg);
    }
}