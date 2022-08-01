using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueControl.Classes.Others
{
    internal class ColorConversions
    {
        public static string colorTemperatureToRGB(int kelvin)
        {
            double temp = kelvin / 100;
            double red;
            double green;
            double blue;

            if (temp <= 66)
            {
                red = 255;

                green = 99.4708025861 * Math.Log(temp) - 161.1195681661;

                if (temp <= 19)
                {

                    blue = 0;

                }
                else
                {

                    blue = temp - 10;
                    blue = 138.5177312231 * Math.Log(blue) - 305.0447927307;
                }
            }
            else
            {

                red = temp - 60;
                red = 329.698727446 * Math.Pow(red, -0.1332047592);

                green = temp - 60;
                green = 288.1221695283 * Math.Pow(green, -0.0755148492);

                blue = 255;

            }

            string ret = clamp(red, 0, 255).ToString() + ";" + clamp(green, 0, 255).ToString() + ";" + clamp(blue, 0, 255).ToString();
            return ret;

        }

        public static int clamp(double x, double min, double max)
        {
            if (x < min) { return Convert.ToInt32(min); }
            if (x > max) { return Convert.ToInt32(max); }

            return Convert.ToInt32(x);
        }

        public static string rgbToHex(string rgb)
        {
            string[] strings = rgb.Split(";");
            int r = Convert.ToInt32(strings[0]);
            int g = Convert.ToInt32(strings[1]);
            int b = Convert.ToInt32(strings[2]);

            System.Drawing.Color myColor = System.Drawing.Color.FromArgb(r, g, b);
            string hex = myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");

            return hex;
        }

        public static double midToKelvin(double mid)
        {
            double x = mid / 1000000;
            double y = 1 / x;
            return y;
        }


        public static string XYZtoRGB(double x, double y, double bri)
        {
            double z = 1.0 - x - y;

            double Y = bri / 255.0; // Brightness of lamp
            double X = Y / y * x;
            double Z = Y / y * z;
            double r = X * 1.612 - Y * 0.203 - Z * 0.302;
            double g = -X * 0.509 + Y * 1.412 + Z * 0.066;
            double b = X * 0.026 - Y * 0.072 + Z * 0.962;
            r = r <= 0.0031308 ? 12.92 * r : (1.0 + 0.055) * Math.Pow(r, 1.0 / 2.4) - 0.055;
            g = g <= 0.0031308 ? 12.92 * g : (1.0 + 0.055) * Math.Pow(g, 1.0 / 2.4) - 0.055;
            b = b <= 0.0031308 ? 12.92 * b : (1.0 + 0.055) * Math.Pow(b, 1.0 / 2.4) - 0.055;
            double maxValue1 = Math.Max(r, g);
            double maxValue = Math.Max(maxValue1, b);
            r /= maxValue;
            g /= maxValue;
            b /= maxValue;
            r = r * 255; if (r < 0) { r = 0; }
            g = g * 255; if (g < 0) { g = 0; }
            b = b * 255; if (b < 0) { b = 0; }

            string re = Math.Round(r).ToString();
            string ge = Math.Round(g).ToString();
            string be = Math.Round(b).ToString();

            if (re.Length < 2)
                re = "0" + re;
            if (ge.Length < 2)
                ge = "0" + ge;
            if (be.Length < 2)
                be = "0" + re;
            string rgb = re + ";" + ge + ";" + be;

            return rgb;
        }
    }
}
