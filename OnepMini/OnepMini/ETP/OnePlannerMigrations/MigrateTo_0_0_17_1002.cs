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
            FromVersion = "0.0.17.1001";
            ToVersion = "0.0.17.1002";
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

                // Get this from the Diff of snapshot of previous to current


                /*
                 *
    drop table if exists onep_validationresults cascade

    drop table if exists onep_validochpath cascade

    create table onep_validationresults (
        oid int8 not null,
       status int4,
       network int8,
       primary key (oid)
    )

    create table onep_validochpath (
        oid int8 not null,
       name varchar(255),
       pmd float8,
       network int8,
       validationResults int8,
       primary key (oid)
    )

    alter table onep_validationresults
        add constraint FK_41C56A95
        foreign key (network)
        references onep_network

    alter table onep_validochpath
        add constraint FK_DE0F4B7
        foreign key (network)
        references onep_network

    alter table onep_validochpath
        add constraint FK_F4AB2716
        foreign key (validationResults)
        references onep_validationresults

                 * */

                var sqlStatement = "drop table if exists onep_validationresults cascade";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "drop table if exists onep_validochpath cascade";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "create table onep_validationresults ( oid int8 not null, status int4, network int8, primary key(oid))";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "create table onep_validochpath ( oid int8 not null, name varchar(255), pmd float8, network int8, validationResults int8, primary key(oid) )";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "alter table onep_validationresults add constraint FK_41C56A95 foreign key(network) references onep_network";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "alter table onep_validochpath add constraint FK_DE0F4B7 foreign key(network) references onep_network";
                if (!string.IsNullOrEmpty(sqlStatement))
                {
                    _nHibernateInitializer.ExecuteNonQuery(sqlStatement);
                }
                sqlStatement = "alter table onep_validochpath add constraint FK_F4AB2716 foreign key(validationResults) references onep_validationresults";
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
