
    drop table if exists reporting_meta_info cascade

    drop table if exists csamp_provisioning_report cascade

    drop table if exists csamp_provisioning_report_item cascade

    drop table if exists csamp_provisioning_report_item_estimated_output_power_table cascade

    drop table if exists fibers_report cascade

    drop table if exists fibers_report_item cascade

    drop table if exists och_report cascade

    drop table if exists och_report_data cascade

    drop table if exists optical_channels_report_item cascade

    drop table if exists optical_channels_report_item_failure_reason_ids cascade

    drop table if exists reporting_root cascade

    drop table if exists url_parameter cascade

    drop table if exists hibernate_unique_key cascade

    create table reporting_meta_info (
        oid int8 not null,
       pageSize int8,
       pageNumber int8,
       totalRecordsInReport int8,
       totalRecordsInDB int8,
       primary key (oid)
    )

    create table csamp_provisioning_report (
        oid int8 not null,
       reportingMetaInfo int8 unique,
       reportingRoot int8,
       primary key (oid)
    )

    create table csamp_provisioning_report_item (
        oid int8 not null,
       productType varchar(255),
       name varchar(255),
       tid varchar(255),
       id varchar(255),
       ampId varchar(255),
       siteName varchar(255),
       ampDirection varchar(255),
       ampOrder varchar(255),
       ampPackage varchar(255),
       ampType varchar(255),
       shelfID varchar(255),
       slot varchar(255),
       port varchar(255),
       peakPowerMode varchar(255),
       targetPeakPower varchar(255),
       channelPowerMode varchar(255),
       targetChannelPower varchar(255),
       estimatedPeakPower varchar(255),
       estimatedMaxChannelPower varchar(255),
       estimatedMinChannelPower varchar(255),
       gainOffsetMode varchar(255),
       targetGainOffset varchar(255),
       gainMode varchar(255),
       targetGain varchar(255),
       estimatedGain varchar(255),
       maxGainAtZeroTilt varchar(255),
       gainTiltMode varchar(255),
       targetGainTilt varchar(255),
       estimatedGainTilt varchar(255),
       maxTotalOutputPower varchar(255),
       estimatedTotalOutputPower varchar(255),
       targetPfib varchar(255),
       voaMode varchar(255),
       voaAttenuationMode varchar(255),
       voaAttenuation varchar(255),
       switchableAmpProvisioningMode varchar(255),
       switchableAmpGainMode varchar(255),
       switchableAmpTopOffsetMode varchar(255),
       switchableAmpTopOffset varchar(255),
       maxAllowedTotalPumpPower varchar(255),
       targetSpanLoss varchar(255),
       ramanGainMode varchar(255),
       ramanFiberType varchar(255),
       ramanGainSetting varchar(255),
       estimateRamanGain varchar(255),
       isLineAmp boolean,
       pfg varchar(255),
       oscPfibMode varchar(255),
       oscPfibTarget varchar(255),
       oscFECMode varchar(255),
       oscFEC varchar(255),
       linkPilotOutputEstimatedPower varchar(255),
       fiberTypeMode varchar(255),
       fiberType varchar(255),
       oscPfibEstimate varchar(255),
       ingressRepairMarginMode varchar(255),
       ingressRepairMargin varchar(255),
       fiberTypeOrLabel varchar(255),
       edfaPfibA varchar(255),
       edfaPfibB varchar(255),
       ramanPfibA varchar(255),
       ramanPfibB varchar(255),
       edfaInFiberType varchar(255),
       fiberAEff varchar(255),
       fiberDispersionCoeff varchar(255),
       fiberAttenuationCoeff varchar(255),
       oscPlacementMode varchar(255),
       oscSFP varchar(255),
       omsName varchar(255),
       estimatedTotalPfib varchar(255),
       targetTotalPfib varchar(255),
       workingBandType varchar(255),
       parentEquipmentBandType varchar(255),
       correspondingWorkingBandId varchar(255),
       cSAmpProvisioningReport int8,
       primary key (oid)
    )

    create table csamp_provisioning_report_item_estimated_output_power_table (
        OId int8 not null,
       AttributeValue float8,
       AttributeName varchar(255) not null,
       primary key (OId, AttributeName)
    )

    create table fibers_report (
        oid int8 not null,
       reportingMetaInfo int8 unique,
       reportingRoot int8,
       primary key (oid)
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
       fibersReport int8,
       primary key (oid)
    )

    create table och_report (
        oid int8 not null,
       reportingMetaInfo int8 unique,
       reportingRoot int8,
       primary key (oid)
    )

    create table och_report_data (
        oid int8 not null,
       id varchar(255),
       type int4 not null,
       attributes int8 unique,
       ochReport int8,
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
       primary key (oid)
    )

    create table url_parameter (
        oid int8 not null,
       name varchar(255),
       value varchar(255),
       cSAmpProvisioningReport int8,
       fibersReport int8,
       ochReport int8,
       primary key (oid)
    )

    alter table csamp_provisioning_report 
        add constraint FK_A18E77C3 
        foreign key (reportingMetaInfo) 
        references reporting_meta_info

    alter table csamp_provisioning_report 
        add constraint csamp_provisioning_report_FK_reportingRoot 
        foreign key (reportingRoot) 
        references reporting_root; 

	create index if not exists csamp_provisioning_report_FK_reportingRoot_idx 
        on csamp_provisioning_report ( reportingRoot );

    alter table csamp_provisioning_report_item 
        add constraint csamp_provisioning_report_item_FK_cSAmpProvisioningReport 
        foreign key (cSAmpProvisioningReport) 
        references csamp_provisioning_report; 

	create index if not exists csamp_provisioning_report_item_FK_cSAmpProvisioningReport_idx 
        on csamp_provisioning_report_item ( cSAmpProvisioningReport );

    alter table csamp_provisioning_report_item_estimated_output_power_table 
        add constraint FK_2EA3CBA 
        foreign key (OId) 
        references csamp_provisioning_report_item

    alter table fibers_report 
        add constraint FK_7D15EF10 
        foreign key (reportingMetaInfo) 
        references reporting_meta_info

    alter table fibers_report 
        add constraint fibers_report_FK_reportingRoot 
        foreign key (reportingRoot) 
        references reporting_root; 

	create index if not exists fibers_report_FK_reportingRoot_idx 
        on fibers_report ( reportingRoot );

    alter table fibers_report_item 
        add constraint fibers_report_item_FK_fibersReport 
        foreign key (fibersReport) 
        references fibers_report; 

	create index if not exists fibers_report_item_FK_fibersReport_idx 
        on fibers_report_item ( fibersReport );

    alter table och_report 
        add constraint FK_D9DDF894 
        foreign key (reportingMetaInfo) 
        references reporting_meta_info

    alter table och_report 
        add constraint och_report_FK_reportingRoot 
        foreign key (reportingRoot) 
        references reporting_root; 

	create index if not exists och_report_FK_reportingRoot_idx 
        on och_report ( reportingRoot );

    alter table och_report_data 
        add constraint FK_44E486B3 
        foreign key (attributes) 
        references optical_channels_report_item

    alter table och_report_data 
        add constraint och_report_data_FK_ochReport 
        foreign key (ochReport) 
        references och_report; 

	create index if not exists och_report_data_FK_ochReport_idx 
        on och_report_data ( ochReport );

    alter table optical_channels_report_item_failure_reason_ids 
        add constraint optical_channels_report_item_failure_reason_ids_FK_opticalChannelsReportItem 
        foreign key (opticalChannelsReportItem) 
        references optical_channels_report_item; 

	create index if not exists optical_channels_report_item_failure_reason_ids_FK_opticalChannelsReportItem_idx 
        on optical_channels_report_item_failure_reason_ids ( opticalChannelsReportItem );

    alter table url_parameter 
        add constraint url_parameter_FK_cSAmpProvisioningReport 
        foreign key (cSAmpProvisioningReport) 
        references csamp_provisioning_report; 

	create index if not exists url_parameter_FK_cSAmpProvisioningReport_idx 
        on url_parameter ( cSAmpProvisioningReport );

    alter table url_parameter 
        add constraint url_parameter_FK_fibersReport 
        foreign key (fibersReport) 
        references fibers_report; 

	create index if not exists url_parameter_FK_fibersReport_idx 
        on url_parameter ( fibersReport );

    alter table url_parameter 
        add constraint url_parameter_FK_ochReport 
        foreign key (ochReport) 
        references och_report; 

	create index if not exists url_parameter_FK_ochReport_idx 
        on url_parameter ( ochReport );

    create table hibernate_unique_key (
         next_hi int8 
    )

    insert into hibernate_unique_key values ( 1 )
