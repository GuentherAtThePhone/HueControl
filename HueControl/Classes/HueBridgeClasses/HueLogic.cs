using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Rssdp;

namespace HueControl.Classes.HueBridgeClasses
{
    internal class HueLogic
    {

        public static string BridgeIP;

        public static string Usercode;

        private const string APIAddressTemplate = "http://{0}/api";
        private const string BodyTemplate = "{{\"devicetype\":\"{0}\"}}";

        public const string LightsUrlTemplate = "http://{0}/api/{1}/{2}";

        public const string ControlLightUrlTemplate = "http://{0}/api/{1}/{2}/{3}/{4}";

        public const string ControlNameUrlTemplate = "http://{0}/api/{1}/{2}/{3}";

        public void FindBridgeIP()
        {
            SsdpDeviceLocator deviceLocator = new SsdpDeviceLocator(GetLocalIPAddress().ToString());
            var foundDevices = deviceLocator.SearchAsync().Result.ToList();

            foreach (var device in foundDevices)
            {
                if (device.ResponseHeaders.ToString().Contains("IpBridge"))
                {
                    BridgeIP = device.DescriptionLocation.Host;
                    return;
                }
                BridgeIP = "no bridge found";
            }
        }

        private IPAddress GetLocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return null;

            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public static string ConnectBridge(string username)
        {
            return PostRequestToBridge(
                 string.Format(APIAddressTemplate, BridgeIP),
                 string.Format(BodyTemplate, username));
        }

        private static string PostRequestToBridge(string uri, string data, string contentType = "application/json", string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            //search for easier way
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                var result = ResultHelper.FromJson(json);
                Usercode = result.First().Success.Username;

                return json;
            }
        }

        public static string GetRequestToBridge(string fullUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        public static void PutRequestToBridge(string fullUri, string data, string contentType = "application/json", string method = "PUT")
        {
            using (var client = new WebClient())
            {
                client.UploadData(fullUri, method, Encoding.UTF8.GetBytes(data));
            }
        }

    }
}
