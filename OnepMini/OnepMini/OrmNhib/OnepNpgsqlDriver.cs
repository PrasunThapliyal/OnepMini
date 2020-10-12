
namespace OnepMini.OrmNhib
{
	using NHibernate.Driver;
	using NHibernate.SqlTypes;
	using Npgsql;
	using NpgsqlTypes;
	using System.Data;
	using System.Data.Common;

	public class OnepNpgsqlDriver : NpgsqlDriver
	{
		protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
		{
			if (sqlType.DbType == DbType.DateTimeOffset)
            {
				var param = dbParam as NpgsqlParameter;
				param.ParameterName = FormatNameForParameter(name);
				param.NpgsqlDbType = NpgsqlDbType.TimeTz;
			}
			else
			{
				base.InitializeParameter(dbParam, name, sqlType);
			}
		}
	}
}
