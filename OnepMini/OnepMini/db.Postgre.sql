
    drop table if exists fibers_report cascade

    drop table if exists fibers_report_item cascade

    drop table if exists hibernate_unique_key cascade

    create table fibers_report (
        oid int8 not null,
       mcpProjectId text,
       primary key (oid)
    )

    create table fibers_report_item (
        oid int8 not null,
       name varchar(255),
       fiberid varchar(255),
       fiberPairString varchar(255),
       fiberType varchar(255),
       onepSiteByAEndSite varchar(255),
       onepSiteByZEndSite varchar(255),
       fiberLength float8,
       totalLoss float8,
       fiberLossSourceType varchar(255),
       fiberLoss float8,
       lossPerKm float8,
       marginDbPerSpan float8,
       marginDbPerKm float8,
       marginSourceType varchar(255),
       headPPL float8,
       tailPPL float8,
       pmdType varchar(255),
       pmdCoefficient float8,
       pmdMean float8,
       fibersReport int8,
       primary key (oid)
    )

    create index fibers_report_mcpProjectId_idx on fibers_report (mcpProjectId)

    alter table fibers_report_item 
        add constraint FK_679998FF 
        foreign key (fibersReport) 
        references fibers_report

    create table hibernate_unique_key (
         next_hi int8 
    )

    insert into hibernate_unique_key values ( 1 )
