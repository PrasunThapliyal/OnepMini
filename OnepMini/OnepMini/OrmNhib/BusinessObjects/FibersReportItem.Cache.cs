using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TopologyRestLibrary.V1.Etp.Reports
{
    public partial class FibersReportItem
    {
        //[System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual long Id { get; set; }

        //[System.Runtime.Serialization.IgnoreDataMember]
        [JsonIgnore]
        public virtual FibersReport FibersReport { get; set; }
    }
}
