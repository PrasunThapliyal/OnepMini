//-----------------------------------------------------------------------------
// <copyright file="EquipmentReport.generated.cs" company="Ciena Corporation"\>
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright\>
//
// This file was generated by a tool. Do not make modifications to this file.
//
//-----------------------------------------------------------------------------

namespace OnepMini.V1.Etp.Reports
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>EquipmentReport</summary>
    public partial class EquipmentReport
    {
        private OnepMini.V1.Etp.Common.ReportingMetaInfo _reportingMetaInfo;
        private IList<OnepMini.V1.Etp.Reports.UrlParameter> _parameters;
        private IList<OnepMini.V1.Etp.Reports.EquipmentReportItem> _data;

    	/// <summary>
        /// Initializes a new instance of the <see cref="EquipmentReport"/> class.
        /// </summary>
        public EquipmentReport()
    	{
            _parameters = new List<OnepMini.V1.Etp.Reports.UrlParameter>();
            _data = new List<OnepMini.V1.Etp.Reports.EquipmentReportItem>();
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long OId { get; set; }


        /// <summary>reportingMetaInfo</summary>
        [JsonProperty("reportingMetaInfo", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual OnepMini.V1.Etp.Common.ReportingMetaInfo ReportingMetaInfo
        {
            get { return _reportingMetaInfo; }
            set 
            {
                if (_reportingMetaInfo != value)
                {
                    _reportingMetaInfo = value; 
                }
            }
        }

        /// <summary>Stringified list of Url parameters</summary>
        [JsonProperty("parameters", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual IList<OnepMini.V1.Etp.Reports.UrlParameter> Parameters
        {
            get { return _parameters; }
            set 
            {
                if (_parameters != value)
                {
                    _parameters = value; 
                }
            }
        }

        /// <summary>List of equipment</summary>
        [JsonProperty("data", Required = Required.Always)]
        public virtual IList<OnepMini.V1.Etp.Reports.EquipmentReportItem> Data
        {
            get { return _data; }
            set 
            {
                if (_data != value)
                {
                    _data = value; 
                }
            }
        }


    }
}