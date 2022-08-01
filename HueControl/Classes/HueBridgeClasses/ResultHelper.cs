using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueControl
{
    public partial class ResultHelper
    {
        [JsonProperty("success")]
        public Success Success { get; set; }
    }

    public partial class Success
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public partial class ResultHelper
    {
        public static ResultHelper[]? FromJson(string json) => JsonConvert.DeserializeObject<ResultHelper[]>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ResultHelper[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
