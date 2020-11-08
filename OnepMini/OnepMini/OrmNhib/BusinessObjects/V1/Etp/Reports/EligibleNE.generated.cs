//-----------------------------------------------------------------------------
// <copyright file="EligibleNE.generated.cs" company="Ciena Corporation"\>
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

    /// <summary>EligibleNE</summary>
    public partial class EligibleNE
    {
        private string _tid;
        private IList<string> _eligibleShelves;

    	/// <summary>
        /// Initializes a new instance of the <see cref="EligibleNE"/> class.
        /// </summary>
        public EligibleNE()
    	{
            _eligibleShelves = new List<string>();
        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long OId { get; set; }


        /// <summary>NE tid</summary>
        [JsonProperty("tid", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Tid
        {
            get { return _tid; }
            set 
            {
                if (_tid != value)
                {
                    _tid = value; 
                }
            }
        }

        /// <summary>Eligible shelves for NE</summary>
        [JsonProperty("eligibleShelves", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual IList<string> EligibleShelves
        {
            get { return _eligibleShelves; }
            set 
            {
                if (_eligibleShelves != value)
                {
                    _eligibleShelves = value; 
                }
            }
        }


    }
}