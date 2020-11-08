//-----------------------------------------------------------------------------
// <copyright file="PlanningPhase.generated.cs" company="Ciena Corporation"\>
//     Copyright (c) Ciena Corporation. All rights reserved.
// </copyright\>
//
// This file was generated by a tool. Do not make modifications to this file.
//
//-----------------------------------------------------------------------------

namespace OnepMini.V1.Etp.Common
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

    /// <summary>Planning Phase</summary>
    public partial class PlanningPhase
    {
        private string _id;
        private string _name;
        private string _order;

    	/// <summary>
        /// Initializes a new instance of the <see cref="PlanningPhase"/> class.
        /// </summary>
        public PlanningPhase()
    	{
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long OId { get; set; }


        /// <summary>PlanningPhase Identifier</summary>
        [JsonProperty("id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Id
        {
            get { return _id; }
            set 
            {
                if (_id != value)
                {
                    _id = value; 
                }
            }
        }

        /// <summary>PlanningPhase Name</summary>
        [JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Name
        {
            get { return _name; }
            set 
            {
                if (_name != value)
                {
                    _name = value; 
                }
            }
        }

        /// <summary>PlanningPhase Order (baseline:1 , Phase1:2)</summary>
        [JsonProperty("order", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Order
        {
            get { return _order; }
            set 
            {
                if (_order != value)
                {
                    _order = value; 
                }
            }
        }


    }
}