using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnepMini.OrmNhib.Util
{
    public class Utils : IUtils
    {
        public string GetCurrent1PBackendVersion()
        {
            var version = "0.0.17.1002";
            return version;
        }

        public string GetLastMigrated1PBackendVersion()
        {
            var version = "0.0.17.1001";
            return version;
        }
    }
}
