using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorHelper;
using HueControl.Classes.Others;
using Newtonsoft.Json;

namespace HueControl
{
    public partial class LightHelper
    {
        public string ID { get; set; }
        [JsonProperty("state")]
        public State State { get; set; }
        [JsonProperty("swupdate")]
        public Swupdate Swupdate { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("modelid")]
        public string Modelid { get; set; }
        [JsonProperty("manufacturername")]
        public string Manufacturername { get; set; }
        [JsonProperty("capabilities")]
        public Capabilities Capabilities { get; set; }
        [JsonProperty("uniqueid")]
        public string Uniqueid { get; set; }
        [JsonProperty("swversion")]
        public string Swversion { get; set; }
        [JsonProperty("swconfigid")]
        public string Swconfigid { get; set; }
        [JsonProperty("productid")]
        public string Productid { get; set; }
    }
    public partial class Swupdate
    {
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("lastinstall")]
        public string Lastinstall { get; set; }
    }
    public partial class State
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
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("reachable")]
        public bool Reachable { get; set; }
        public string Rcolour { get; set; }
        public string? Backcolour { get; set; }
    }
    public partial class Capabilities
    {
        [JsonProperty("streaming")]
        public Streaming Streaming { get; set; }
        [JsonProperty("control")]
        public Control Control { get; set; }
    }
    public partial class Control
    {
        [JsonProperty("ct")]
        public Ct Ct { get; set; }
    }
    public partial class Ct
    {
        [JsonProperty("min")]
        public long Min { get; set; }
        [JsonProperty("max")]
        public long Max { get; set; }
    }
    public partial class Streaming
    {
        [JsonProperty("renderer")]
        public bool Renderer { get; set; }
        [JsonProperty("proxy")]
        public bool Proxy { get; set; }
    }
    public partial class LightHelper
    {
        public static List<LightHelper> FromJson(string json)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, LightHelper>>(json, ConverterLight.Settings);
            var result = new List<LightHelper>();
            foreach (var item in dict)
            {
                item.Value.ID = item.Key;

                if (item.Value.State.Reachable)
                {
                    item.Value.State.Rcolour = "Green";
                }
                else
                {
                    item.Value.State.Rcolour = "Red";
                }

                result.Add(item.Value);

                switch (item.Value.Type)
                {

                    case "Color temperature light":

                        double kelvin = ColorConversions.midToKelvin(item.Value.State.Ct);
                        string rgb = ColorConversions.colorTemperatureToRGB(Convert.ToInt32(kelvin));
                        string hex = "#" + ColorConversions.rgbToHex(rgb);
                        item.Value.State.Backcolour = hex;

                        break;

                    // Simple on/off plug
                    case "On/Off plug-in unit":
                        item.Value.State.Backcolour = "#FF838383";
                        break;

                    // lights like the Ikea Color bulbs with XY-Color Control
                    case "Color light":

                        double X = item.Value.State.Xy[0];
                        double Y = item.Value.State.Xy[1];

                        string rgB = ColorConversions.XYZtoRGB(X, Y, item.Value.State.Bri);
                        string Hex = "#" + ColorConversions.rgbToHex(rgB);
                        item.Value.State.Backcolour = Hex;

                        break;

                    // lights like the Hue Spot with Hue-Color Control
                    case "Extended color light":

                        double hue = item.Value.State.Hue;

                        double cons = 360.0 / 65535.0;

                        double degree = cons * hue;

                        double brightness = (100.0 / 254.0) * item.Value.State.Bri;

                        HSV hsv = new HSV(Convert.ToInt32(degree), Convert.ToByte(100), Convert.ToByte(brightness));
                        HEX heX = ColorHelper.ColorConverter.HsvToHex(hsv);

                        string hEx = "#" + heX.ToString();
                        item.Value.State.Backcolour = hEx;

                        break;

                    default:
                        break;
                }

            }
            return result;
        }
    }
    public static class SerializeLight
    {
        public static string ToJson(this LightHelper self) => JsonConvert.SerializeObject(self, ConverterLight.Settings);
    }
    public class ConverterLight
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}