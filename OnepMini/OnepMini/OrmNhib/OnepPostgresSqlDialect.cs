using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.OrmNhib
{
    public class OnepPostgresSqlDialect : PostgreSQL83Dialect
    {
        public OnepPostgresSqlDialect()
        {
            RegisterColumnType(System.Data.DbType.UInt16, "oid");
            RegisterColumnType(System.Data.DbType.UInt32, "oid");
            RegisterColumnType(System.Data.DbType.SByte, "int2");
            RegisterColumnType(System.Data.DbType.Decimal, "float8");
        }
    }
}
