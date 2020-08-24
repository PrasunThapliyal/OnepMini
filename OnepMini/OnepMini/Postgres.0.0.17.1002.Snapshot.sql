
    drop table if exists onep_amptp cascade

    drop table if exists onep_network cascade

    drop table if exists onep_terminationpoint cascade

    drop table if exists onep_topologicallink cascade

    drop table if exists onep_fibertl cascade

    drop table if exists onep_validationresults cascade

    drop table if exists onep_validochpath cascade

    drop table if exists nh_migrations_history cascade

    drop table if exists hibernate_unique_key cascade

    create table onep_amptp (
        oid int8 not null,
       targetGain float8,
       network int8,
       primary key (oid)
    )

    create table onep_network (
        oid int8 not null,
       name varchar(255),
       mcpProjectId varchar(255),
       primary key (oid)
    )

    create table onep_terminationpoint (
        oid int8 not null,
       PTP int2,
       name varchar(255),
       notes varchar(255),
       network int8,
       OnepAmpRole int8,
       primary key (oid)
    )

    create table onep_topologicallink (
        oid int8 not null,
       Discriminator int4,
       name varchar(255),
       length float8,
       uniMate int8,
       aEndTP int8,
       zEndTP int8,
       network int8,
       primary key (oid)
    )

    create table onep_fibertl (
        oid int8 not null,
       loss float8,
       network int8,
       primary key (oid)
    )

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

    create table nh_migrations_history (
        oid int8 not null,
       name varchar(255),
       onepBackendVersion varchar(255),
       previousOnepBackendVersion varchar(255),
       notes varchar(255),
       creationTime timestamp,
       primary key (oid)
    )

    alter table onep_amptp
        add constraint FK_298A3252
        foreign key (network)
        references onep_network

    alter table onep_terminationpoint
        add constraint FK_47A14A86
        foreign key (network)
        references onep_network

    alter table onep_terminationpoint
        add constraint FK_353543FA
        foreign key (OnepAmpRole)
        references onep_amptp

    alter table onep_topologicallink
        add constraint FK_E6468C8D
        foreign key (uniMate)
        references onep_topologicallink

    alter table onep_topologicallink
        add constraint FK_C07BFEF1
        foreign key (aEndTP)
        references onep_terminationpoint

    alter table onep_topologicallink
        add constraint FK_33EE3BAB
        foreign key (zEndTP)
        references onep_terminationpoint

    alter table onep_topologicallink
        add constraint FK_1588FBB9
        foreign key (network)
        references onep_network

    alter table onep_fibertl
        add constraint FK_DBFE8370
        foreign key (oid)
        references onep_topologicallink

    alter table onep_fibertl
        add constraint FK_D775C7F5
        foreign key (network)
        references onep_network

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

    create table hibernate_unique_key (
         next_hi int8
    )

    insert into hibernate_unique_key values ( 1 )
