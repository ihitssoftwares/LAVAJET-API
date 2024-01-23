using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PPR.Lite.Infrastructure.Helpers
{
 public static    class JsonHelper
    {
        public static string ToJson<T>(T obj, JsonSerializerSettings settings = null)
        {
            if (settings != null)
                return JsonConvert.SerializeObject(obj, settings);
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static SqlDbType ToJson(object groupBy)
        {
            throw new NotImplementedException();
        }
    }
}
