//-----------------------------------------------------------------------------
// <copyright file="OchReport.generated.cs" company="Ciena Corporation"\>
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright\>
//
// This file was generated by a tool. Do not make modifications to this file.
//
//-----------------------------------------------------------------------------

namespace TopologyRestLibrary.V1.Etp.Reports
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

    /// <summary>OchReport</summary>
    public partial class OchReport
    {
        private IList<TopologyRestLibrary.V1.Etp.Reports.OchReportData> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="OchReport"/> class.
        /// </summary>
        public OchReport()
        {
            _data = new List<TopologyRestLibrary.V1.Etp.Reports.OchReportData>();
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long OId { get; set; }


        /// <summary>data</summary>
        [JsonProperty("data", Required = Required.Always)]
        public virtual IList<TopologyRestLibrary.V1.Etp.Reports.OchReportData> Data
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