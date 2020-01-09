using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace RedRP
{
    public class DiscoveryHost
    {
        
        public HostState HostState { get; set; }
        public string SystemVersion { get; set; }
        public string ProtocolVersion { get; set; }
        public string RequestPort { get; set; }
        public string HostName { get; set; }
        public string HostType { get; set; }
        public string HostID { get; set; }
        public string RunningAppTitleID { get; set; }
        public string RunningAppName { get; set; }
        public string IP { get; set; }
        public DiscoveryHost(string data)
        {
            Dictionary<string, string> dict = SplitHttpResponse(data);
            HostID = dict.ContainsKey("host-id") ?  dict["host-id"] : "";
            HostType = dict.ContainsKey("host-type") ? dict["host-type"] : "";
            HostName = dict.ContainsKey("host-name") ? dict["host-name"] : "";
            RequestPort = dict.ContainsKey("host-request-port") ? dict["host-request-port"] : "";
            ProtocolVersion = dict.ContainsKey("device-discovery-protocol-version") ? dict["device-discovery-protocol-version"] : "";
            SystemVersion = dict.ContainsKey("system-version") ? dict["system-version"] : "";
            RunningAppTitleID = dict.ContainsKey("running-app-titleid") ? dict["running-app-titleid"] : "";
            RunningAppName = dict.ContainsKey("running-app-name") ? dict["running-app-name"] : "";
            if (dict["host-state"] == "200")
                HostState = HostState.Ready;
            else if (dict["host-state"] == "620")
                HostState = HostState.StandBy;
            else
                HostState = HostState.Unknown;

        }


        private Dictionary<string, string> SplitHttpResponse(string data)
        {
            string[] splitData = data.Split('\n');
            string firstLine = splitData[0];
            string code = Regex.Split(firstLine, @"\s+")[1];

            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            for (int i = 1; i < splitData.Length; i++)
            {
                string pair = splitData[i];
                string[] keyValue = pair.Split(':');
                if (keyValue.Length == 2)
                {
                    httpHeaders.Add(keyValue[0], keyValue[1]);
                }
            }
            httpHeaders.Add("host-state", code);
            return httpHeaders;


        }
    }
  
    public enum HostState
    {
        Ready,
        StandBy,
        Unknown
    }
}
