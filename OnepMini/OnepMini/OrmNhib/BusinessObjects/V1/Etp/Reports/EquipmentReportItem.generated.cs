//-----------------------------------------------------------------------------
// <copyright file="EquipmentReportItem.generated.cs" company="Ciena Corporation"\>
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

    /// <summary>Describes the change for an equipment</summary>
    public partial class EquipmentReportItem
    {
        private OnepMini.V1.Common.NodeIdentity _node;
        private OnepMini.V1.Common.EquipmentSpec _equipmentSpec;
        private OnepMini.V1.Common.Location _location;
        private ChangeTypeTypes _changeType;
        private string _id;
        private decimal? _width;
        private bool? _canReslot;
        private IList<string> _eligibleShelves;
        private IList<OnepMini.V1.Etp.Reports.EligibleNE> _eligibleNEs;
        private string _equipmentGroupKey;
        private OnepMini.V1.Etp.Common.PlanningState _planningState;

    	/// <summary>
        /// Initializes a new instance of the <see cref="EquipmentReportItem"/> class.
        /// </summary>
        public EquipmentReportItem()
    	{
            _changeType = ChangeTypeTypes.Undefined;
            _eligibleShelves = new List<string>();
            _eligibleNEs = new List<OnepMini.V1.Etp.Reports.EligibleNE>();

        }

        [System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long OId { get; set; }

        /// <summary>ChangeTypeTypes</summary>
        public enum ChangeTypeTypes
        {
        #pragma warning disable 1591
             [EnumMember(Value = "Added")]
        	 Added, 
             [EnumMember(Value = "Removed")]
        	 Removed, 
             [EnumMember(Value = "Modified")]
        	 Modified, 
             [EnumMember(Value = "Undefined")]
        	 Undefined, 
        #pragma warning restore 1591
        }



        /// <summary>The node equipment belongs to</summary>
        [JsonProperty("node", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual OnepMini.V1.Common.NodeIdentity Node
        {
            get { return _node; }
            set 
            {
                if (_node != value)
                {
                    _node = value; 
                }
            }
        }

        /// <summary>Equipment spec.</summary>
        [JsonProperty("equipmentSpec", Required = Required.Always)]
        public virtual OnepMini.V1.Common.EquipmentSpec EquipmentSpec
        {
            get { return _equipmentSpec; }
            set 
            {
                if (_equipmentSpec != value)
                {
                    _equipmentSpec = value; 
                }
            }
        }

        /// <summary>Location of the equipment</summary>
        [JsonProperty("location", Required = Required.Always)]
        public virtual OnepMini.V1.Common.Location Location
        {
            get { return _location; }
            set 
            {
                if (_location != value)
                {
                    _location = value; 
                }
            }
        }

        /// <summary>The type of change</summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("changeType", Required = Required.Always)]
        public virtual ChangeTypeTypes ChangeType
        {
            get { return _changeType; }
            set 
            {
                if (_changeType != value)
                {
                    _changeType = value; 
                }
            }
        }

        /// <summary>Equipment Id</summary>
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

        /// <summary>Equipment width</summary>
        [JsonProperty("width", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual decimal? Width
        {
            get { return _width; }
            set 
            {
                if (_width != value)
                {
                    _width = value; 
                }
            }
        }

        /// <summary>Can reslot this equipment</summary>
        [JsonProperty("canReslot", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual bool? CanReslot
        {
            get { return _canReslot; }
            set 
            {
                if (_canReslot != value)
                {
                    _canReslot = value; 
                }
            }
        }

        /// <summary>Eligible Shelves</summary>
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

        /// <summary>Eligible NEs</summary>
        [JsonProperty("eligibleNEs", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual IList<OnepMini.V1.Etp.Reports.EligibleNE> EligibleNEs
        {
            get { return _eligibleNEs; }
            set 
            {
                if (_eligibleNEs != value)
                {
                    _eligibleNEs = value; 
                }
            }
        }

        /// <summary>Unique key for each parent-child(equipment and it's pluggable) equipment</summary>
        [JsonProperty("equipmentGroupKey", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual string EquipmentGroupKey
        {
            get { return _equipmentGroupKey; }
            set 
            {
                if (_equipmentGroupKey != value)
                {
                    _equipmentGroupKey = value; 
                }
            }
        }

        /// <summary>Planning state properties</summary>
        [JsonProperty("planningState", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public virtual OnepMini.V1.Etp.Common.PlanningState PlanningState
        {
            get { return _planningState; }
            set 
            {
                if (_planningState != value)
                {
                    _planningState = value; 
                }
            }
        }


    }
}