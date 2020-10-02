using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopologyRestLibrary.V1.Etp.Reports
{
    public partial class FibersReport
    {
        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual long Id { get; set; }
        
        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual string McpProjectId { get; set; }
    }
}
