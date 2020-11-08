
    drop table if exists equipment_spec cascade

    drop table if exists location cascade

    drop table if exists location_address cascade

    drop table if exists node_identity cascade

    drop table if exists planning_phase cascade

    drop table if exists planning_state cascade

    drop table if exists reporting_meta_info cascade

    drop table if exists eligible_ne cascade

    drop table if exists eligible_ne_eligible_shelves cascade

    drop table if exists equipment_report cascade

    drop table if exists equipment_report_item cascade

    drop table if exists equipment_report_item_eligible_shelves cascade

    drop table if exists reporting_root cascade

    drop table if exists url_parameter cascade

    drop table if exists hibernate_unique_key cascade

    create table equipment_spec (
        oid int8 not null,
       partNumber varchar(255),
       description varchar(255),
       shortDescription varchar(255),
       releaseMin varchar(255),
       aidType varchar(255),
       groupId varchar(255),
       modellingStatus int4 not null,
       serialNumber varchar(255),
       manufacturingDate varchar(255),
       age varchar(255),
       isExcludedFromDefaultProfile boolean,
       primary key (oid)
    )

    create table location (
        oid int8 not null,
       mgmtAccess int4 not null,
       primary key (oid)
    )

    create table location_address (
        oid int8 not null,
       key int4 not null,
       "value" varchar(255),
       pattern varchar(255),
       location int8,
       primary key (oid)
    )

    create table node_identity (
        oid int8 not null,
       clli varchar(255),
       nodeId int8,
       tid varchar(255),
       siteName varchar(255),
       ncId varchar(255),
       productType varchar(255),
       siteId varchar(255),
       siteAddress varchar(255),
       primary key (oid)
    )

    create table planning_phase (
        oid int8 not null,
       id varchar(255),
       name varchar(255),
       "order" varchar(255),
       primary key (oid)
    )

    create table planning_state (
        oid int8 not null,
       planningStatus varchar(255),
       planningSource varchar(255),
       planningTimestamp varchar(255),
       planningPhase int8 unique,
       primary key (oid)
    )

    create table reporting_meta_info (
        oid int8 not null,
       pageSize int8,
       pageNumber int8,
       totalRecordsInReport int8,
       totalRecordsInDB int8,
       primary key (oid)
    )

    create table eligible_ne (
        oid int8 not null,
       tid varchar(255),
       equipmentReportItem int8,
       primary key (oid)
    )

    create table eligible_ne_eligible_shelves (
        eligibleNE int8 not null,
       eligibleShelves varchar(255)
    )

    create table equipment_report (
        oid int8 not null,
       reportingMetaInfo int8 unique,
       reportingRoot int8,
       primary key (oid)
    )

    create table equipment_report_item (
        oid int8 not null,
       changeType int4 not null,
       id varchar(255),
       width float8,
       canReslot boolean,
       equipmentGroupKey varchar(255),
       node int8 unique,
       equipmentSpec int8 unique,
       location int8 unique,
       planningState int8 unique,
       equipmentReport int8,
       primary key (oid)
    )

    create table equipment_report_item_eligible_shelves (
        equipmentReportItem int8 not null,
       eligibleShelves varchar(255)
    )

    create table reporting_root (
        oid int8 not null,
       projectId varchar(255),
       creationDate timetz,
       lastAccessedDate timetz,
       primary key (oid)
    )

    create table url_parameter (
        oid int8 not null,
       name varchar(255),
       "value" varchar(255),
       equipmentReport int8,
       primary key (oid)
    )

    alter table location_address 
        add constraint location_address_FK_location 
        foreign key (location) 
        references location; 

	create index if not exists location_address_FK_location_idx 
        on location_address ( location );

    alter table planning_state 
        add constraint FK_A2E090BC 
        foreign key (planningPhase) 
        references planning_phase

    alter table eligible_ne 
        add constraint eligible_ne_FK_equipmentReportItem 
        foreign key (equipmentReportItem) 
        references equipment_report_item; 

	create index if not exists eligible_ne_FK_equipmentReportItem_idx 
        on eligible_ne ( equipmentReportItem );

    alter table eligible_ne_eligible_shelves 
        add constraint eligible_ne_eligible_shelves_FK_eligibleNE 
        foreign key (eligibleNE) 
        references eligible_ne; 

	create index if not exists eligible_ne_eligible_shelves_FK_eligibleNE_idx 
        on eligible_ne_eligible_shelves ( eligibleNE );

    alter table equipment_report 
        add constraint FK_BBDE1C38 
        foreign key (reportingMetaInfo) 
        references reporting_meta_info

    alter table equipment_report 
        add constraint equipment_report_FK_reportingRoot 
        foreign key (reportingRoot) 
        references reporting_root; 

	create index if not exists equipment_report_FK_reportingRoot_idx 
        on equipment_report ( reportingRoot );

    alter table equipment_report_item 
        add constraint FK_ABAC7329 
        foreign key (node) 
        references node_identity

    alter table equipment_report_item 
        add constraint FK_5D3545E9 
        foreign key (equipmentSpec) 
        references equipment_spec

    alter table equipment_report_item 
        add constraint FK_9498FCFC 
        foreign key (location) 
        references location

    alter table equipment_report_item 
        add constraint FK_72DDF499 
        foreign key (planningState) 
        references planning_state

    alter table equipment_report_item 
        add constraint equipment_report_item_FK_equipmentReport 
        foreign key (equipmentReport) 
        references equipment_report; 

	create index if not exists equipment_report_item_FK_equipmentReport_idx 
        on equipment_report_item ( equipmentReport );

    alter table equipment_report_item_eligible_shelves 
        add constraint equipment_report_item_eligible_shelves_FK_equipmentReportItem 
        foreign key (equipmentReportItem) 
        references equipment_report_item; 

	create index if not exists equipment_report_item_eligible_shelves_FK_equipmentReportItem_idx 
        on equipment_report_item_eligible_shelves ( equipmentReportItem );

    alter table url_parameter 
        add constraint url_parameter_FK_equipmentReport 
        foreign key (equipmentReport) 
        references equipment_report; 

	create index if not exists url_parameter_FK_equipmentReport_idx 
        on url_parameter ( equipmentReport );

    create table hibernate_unique_key (
         next_hi int8 
    )

    insert into hibernate_unique_key values ( 1 )
