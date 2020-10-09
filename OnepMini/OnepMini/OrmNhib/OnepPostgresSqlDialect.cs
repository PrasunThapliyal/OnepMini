using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            RegisterColumnType(System.Data.DbType.DateTimeOffset, "timetz");
        }

        public override string GetAddForeignKeyConstraintString(
            string constraintName,
            string[] foreignKey,
            string referencedTable,
            string[] primaryKey,
            bool referencesPrimaryKey)
        {
            // If the constraint name is of format $"{tableName}_FK_{columnName}
            // Then we can use this to extract the tableName
            // Which we can further use to create a 'CREATE INDEX' statement

            var tableName = (!constraintName.Contains("_FK_")) ? string.Empty : Regex.Replace(constraintName, @"_FK_.*", "");

            var defaultForeignKeyConstraintString = base.GetAddForeignKeyConstraintString(constraintName, foreignKey, referencedTable, primaryKey, referencesPrimaryKey);
            if (string.IsNullOrEmpty(tableName))
            {
                return defaultForeignKeyConstraintString;
            }

            var createIndexString = $"create index {constraintName}_idx on {tableName} ( {string.Join(", ", foreignKey)} );";
            var foreignKeyConstraintStringAndCreateIndexString =
                $"{defaultForeignKeyConstraintString}; {createIndexString}";

            return foreignKeyConstraintStringAndCreateIndexString;
        }
    }
}
