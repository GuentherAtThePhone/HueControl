using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueControl
{
    public partial class GroupHelper
    {
        public string ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("lights")]
        public List<int> Lights { get; set; }
        [JsonProperty("sensors")]
        public List<int> Sensors { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("state")]
        public State State { get; set; }
        [JsonProperty("recycle")]
        public bool Recycle { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("action")]
        public Action Action { get; set; }
    }

    public partial class State
    {
        [JsonProperty("all_on")]
        public bool All_On { get; set; }
        [JsonProperty("any_on")]
        public bool Any_On { get; set; }
    }
    public partial class Action
    {
        [JsonProperty("on")]
        public bool On { get; set; }
        [JsonProperty("bri")]
        public long Bri { get; set; }
        [JsonProperty("hue")]
        public long Hue { get; set; }
        [JsonProperty("sat")]
        public long Sat { get; set; }
        [JsonProperty("effect")]
        public string Effect { get; set; }
        [JsonProperty("xy")]
        public List<double> Xy { get; set; }
        [JsonProperty("ct")]
        public long Ct { get; set; }
        [JsonProperty("alert")]
        public string Alert { get; set; }
        [JsonProperty("colormode")]
        public string Colormode { get; set; }
    }

    public partial class GroupHelper
    {
        public static List<GroupHelper> FromJson(string json)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, GroupHelper>>(json, ConverterLight.Settings);
            var result = new List<GroupHelper>();
            foreach (var item in dict)
            {
                item.Value.ID = item.Key;

                result.Add(item.Value);
            }
            return result;
        }
    }

    public static class SerializeGroup
    {
        public static string ToJson(this GroupHelper self) => JsonConvert.SerializeObject(self, ConverterLight.Settings);
    }
    public class ConverterGroup
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}