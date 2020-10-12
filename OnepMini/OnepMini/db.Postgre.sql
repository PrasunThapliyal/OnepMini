
    drop table if exists fibers_report cascade

    drop table if exists fibers_report_my_list_of_strings1 cascade

    drop table if exists fibers_report_my_list_of_strings2 cascade

    drop table if exists fibers_report_item cascade

    drop table if exists och_report cascade

    drop table if exists och_report_data cascade

    drop table if exists optical_channels_report_item cascade

    drop table if exists optical_channels_report_item_failure_reason_ids cascade

    drop table if exists reporting_root cascade

    drop table if exists hibernate_unique_key cascade

    create table fibers_report (
        oid int8 not null,
       acct_type int4 not null,
       primary key (oid)
    )

    create table fibers_report_my_list_of_strings1 (
        fibersReport int8 not null,
       myListOfStrings1 varchar(255)
    )

    create table fibers_report_my_list_of_strings2 (
        fibersReport int8 not null,
       myListOfStrings2 varchar(255)
    )

    create table fibers_report_item (
        oid int8 not null,
       name varchar(255),
       fiberId varchar(255),
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
       fibersReport int8 not null,
       primary key (oid)
    )

    create table och_report (
        oid int8 not null,
       primary key (oid)
    )

    create table och_report_data (
        oid int8 not null,
       id varchar(255),
       type int4 not null,
       attributes int8 unique,
       ochReport int8 not null,
       primary key (oid)
    )

    create table optical_channels_report_item (
        oid int8 not null,
       ochPair varchar(255),
       ochPairStatus varchar(255),
       ochID varchar(255),
       ochStatus varchar(255),
       receiverOSNRMargin varchar(255),
       photonicSncId varchar(255),
       photonicSncName varchar(255),
       sourceSite varchar(255),
       destinationSite varchar(255),
       sourceNetworkElement varchar(255),
       destinationNetworkElement varchar(255),
       frequency varchar(255),
       wavelength varchar(255),
       lowFrequencyGuardBand varchar(255),
       highFrequencyGuardBand varchar(255),
       minimumSpectralOccupancy varchar(255),
       baudRate varchar(255),
       netBias varchar(255),
       sourceAdjTxType varchar(255),
       destinationAdjTxType varchar(255),
       sourceDWDMInterface varchar(255),
       destinationDWDMInterface varchar(255),
       sourcePEC varchar(255),
       destinationPEC varchar(255),
       modulationClass varchar(255),
       pathLength varchar(255),
       pmdMean varchar(255),
       amplifiedSpanCount varchar(255),
       fiberLatency varchar(255),
       selectedSourceCMDType varchar(255),
       selectedDestinationCMDType varchar(255),
       selectedSourceAddDrop varchar(255),
       selectedDestinationAddDrop varchar(255),
       selectedSourceCMDAID varchar(255),
       selectedDestinationCMDAID varchar(255),
       selectedSourceCMDPort varchar(255),
       selectedDestinationCMDPort varchar(255),
       preferredLineCard varchar(255),
       receiverDispersionMin varchar(255),
       receiverDispersionMax varchar(255),
       lowDispersionMargin varchar(255),
       highDispersionMargin varchar(255),
       optimumDispersionShift varchar(255),
       dispersionShiftFiberType varchar(255),
       txPreCompMode varchar(255),
       txPreCompModeValue varchar(255),
       rmsPDL varchar(255),
       adjustedReceiverOSNR varchar(255),
       simulatedOSNR varchar(255),
       receiverPower varchar(255),
       receiverPowerMinMargin varchar(255),
       receiverPowerMaxMargin varchar(255),
       transmitterPowerReduction varchar(255),
       protected varchar(255),
       path varchar(255),
       dispersion varchar(255),
       e2pn varchar(255),
       fastValidationStatus varchar(255),
       measuredCd varchar(255),
       measuredPmd varchar(255),
       omsCount varchar(255),
       otsCount varchar(255),
       traversedOms varchar(255),
       verificationPath varchar(255),
       modellingStatus varchar(255),
       measuredLatency varchar(255),
       measuredPreFECBER varchar(255),
       measuredQ varchar(255),
       primary key (oid)
    )

    create table optical_channels_report_item_failure_reason_ids (
        opticalChannelsReportItem int8 not null,
       failureReasonIds varchar(255)
    )

    create table reporting_root (
        oid int8 not null,
       projectId varchar(255),
       creationDate timetz,
       lastAccessedDate timetz,
       fibersReport int8 unique,
       ochReport int8 unique,
       primary key (oid)
    )

    alter table fibers_report_my_list_of_strings1 
        add constraint fibers_report_my_list_of_strings1_FK_fibersReport 
        foreign key (fibersReport) 
        references fibers_report; create index fibers_report_my_list_of_strings1_FK_fibersReport_idx 
        on fibers_report_my_list_of_strings1 ( fibersReport );

    alter table fibers_report_my_list_of_strings2 
        add constraint fibers_report_my_list_of_strings2_FK_fibersReport 
        foreign key (fibersReport) 
        references fibers_report; create index fibers_report_my_list_of_strings2_FK_fibersReport_idx 
        on fibers_report_my_list_of_strings2 ( fibersReport );

    alter table fibers_report_item 
        add constraint fibers_report_item_FK_fibersReport 
        foreign key (fibersReport) 
        references fibers_report; create index fibers_report_item_FK_fibersReport_idx 
        on fibers_report_item ( fibersReport );

    alter table och_report_data 
        add constraint FK_44E486B3 
        foreign key (attributes) 
        references optical_channels_report_item

    alter table och_report_data 
        add constraint och_report_data_FK_ochReport 
        foreign key (ochReport) 
        references och_report; create index och_report_data_FK_ochReport_idx 
        on och_report_data ( ochReport );

    alter table optical_channels_report_item_failure_reason_ids 
        add constraint optical_channels_report_item_failure_reason_ids_FK_opticalChannelsReportItem 
        foreign key (opticalChannelsReportItem) 
        references optical_channels_report_item; create index optical_channels_report_item_failure_reason_ids_FK_opticalChannelsReportItem_idx 
        on optical_channels_report_item_failure_reason_ids ( opticalChannelsReportItem );

    alter table reporting_root 
        add constraint FK_4F5D32D1 
        foreign key (fibersReport) 
        references fibers_report

    alter table reporting_root 
        add constraint FK_53F3029B 
        foreign key (ochReport) 
        references och_report

    create table hibernate_unique_key (
         next_hi int8 
    )

    insert into hibernate_unique_key values ( 1 )
