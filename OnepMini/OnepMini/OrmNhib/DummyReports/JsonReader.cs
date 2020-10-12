namespace OnepMini.OrmNhib.DummyReports
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class JsonReader
    {
        public static T ReadJsonDataFile<T>(Assembly assembly, string manifestResourceName)
        {
            var resourceStream = assembly.GetManifestResourceStream(manifestResourceName);

            string content = string.Empty;

            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                content = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
