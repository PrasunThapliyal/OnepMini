
    create table nh_migrations_history (
        oid int8 not null,
       name varchar(255),
       onepBackendVersion varchar(255),
       previousOnepBackendVersion varchar(255),
       notes varchar(255),
       creationTime timestamp,
       primary key (oid)
    )