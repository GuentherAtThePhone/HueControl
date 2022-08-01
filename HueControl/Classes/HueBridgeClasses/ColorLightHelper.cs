using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HueControl
{
    #region TurnOnOff
    public partial class TurnOnOff
    {
        [JsonProperty("on")]
        public bool On { get; set; }

        public TurnOnOff(bool on)
        {
            On = on;
        }
    }
    public partial class TurnOnOff
    {
        public static TurnOnOff FromJson(string json) => JsonConvert.DeserializeObject<TurnOnOff>(json, Converter.Settings);
    }
    public static class SerializeTurnOnOff
    {
        public static string ToJson(this TurnOnOff self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public class ConverterTurnOnOff
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
    #endregion

    #region ChangeColor
    public partial class ChangeColor
    {
        [JsonProperty("transitiontime")]
        public int Transitiontime { get; set; }
        [JsonProperty("hue")]
        public long? Hue { get; set; }
        [JsonProperty("xy")]
        public List<double>? Xy { get; set; }
        [JsonProperty("ct")]
        public long? Ct { get; set; }

        public ChangeColor(int transitiontime, long hue)
        {
            Transitiontime = transitiontime;
            Hue = hue;
        }
        public ChangeColor(int transitiontime, List<double> xy)
        {
            Transitiontime = transitiontime;
            Xy = xy;
        }
        public ChangeColor(long ct, int transitiontime)
        {
            Transitiontime = transitiontime;
            Ct = ct;
        }
    }
    public partial class ChangeColor
    {
        public static ChangeColor FromJson(string json) => JsonConvert.DeserializeObject<ChangeColor>(json, Converter.Settings);
    }
    public static class SerializeChangeColor
    {
        public static string ToJson(this ChangeColor self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public class ConverterChangeColor
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }

    #endregion

    #region ChangeBrightness
    public partial class ChangeBrightness
    {
        [JsonProperty("transitiontime")]
        public int Transitiontime { get; set; }
        [JsonProperty("bri")]
        public long Bri { get; set; }

        public ChangeBrightness(int transitiontime, long bri)
        {
            Transitiontime = transitiontime;
            Bri = bri;
        }
    }
    public partial class ChangeBrightness
    {
        public static ChangeBrightness FromJson(string json) => JsonConvert.DeserializeObject<ChangeBrightness>(json, Converter.Settings);
    }
    public static class SerializeChangeBrightness
    {
        public static string ToJson(this ChangeBrightness self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public class ConverterChangeBrightness
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
    #endregion

    #region ChangeSaturation
    public partial class ChangeSaturation
    {
        [JsonProperty("transitiontime")]
        public int Transitiontime { get; set; }
        [JsonProperty("sat")]
        public long Sat { get; set; }

        public ChangeSaturation(int transitiontime, long sat)
        {
            Transitiontime = transitiontime;
            Sat = sat;
        }
    }
    public partial class ChangeSaturation
    {
        public static ChangeSaturation FromJson(string json) => JsonConvert.DeserializeObject<ChangeSaturation>(json, Converter.Settings);
    }
    public static class SerializeChangeSaturation
    {
        public static string ToJson(this ChangeSaturation self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public class ConverterChangeSaturation
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
    #endregion

    #region ChangeName
    public partial class ChangeName
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public ChangeName(string name)
        {
            Name = name;
        }
    }
    public partial class ChangeName
    {
        public static ChangeName FromJson(string json) => JsonConvert.DeserializeObject<ChangeName>(json, Converter.Settings);
    }
    public static class SerializeChangeName
    {
        public static string ToJson(this ChangeSaturation self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    public class ConverterChangeName
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
    #endregion

}
