using Org.BouncyCastle.Security;
using Ps4RemotePlay.Protocol.Crypto;
using Ps4RemotePlay.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedRP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private static int myProt = 987;   //端口 
        static Socket serverSocket;

        const string DISCOVERY_PROTOCOL_VERSION = "00020020";
        const string srch_fmt = "SRCH * HTTP/1.1\r\ndevice-discovery-protocol-version:" + DISCOVERY_PROTOCOL_VERSION + "\r\n";

        Dictionary<string, DiscoveryHost> HostDict;
        private void Discory_Click(object sender, EventArgs e)
        {
            textBoxInfo.Text = "";
            HostDict = new Dictionary<string, DiscoveryHost>();
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);

            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 0));  //绑定IP地址：端口                

            serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);


            Thread thread = new Thread(receive);
            thread.Start();
            send();
            Thread.Sleep(500);
            int readyCount = 0;
            int standByCount = 0;
            int RunFIFA20 = 0;
            foreach (var DiscoveryHost in HostDict)
            {
                if (DiscoveryHost.Value.HostState == HostState.Ready)
                    readyCount++;
                else if (DiscoveryHost.Value.HostState == HostState.StandBy)
                    standByCount++;
                if (DiscoveryHost.Value.RunningAppName.Contains("FIFA"))
                    RunFIFA20++;
                textBoxInfo.AppendText(DiscoveryHost.Value.HostID + "," + DiscoveryHost.Value.IP+","+DiscoveryHost.Value.HostState+"\r\n");
            }

            Console.WriteLine("共找到" + HostDict.Count + "台PS4,有" + readyCount + "在运行,有" + standByCount + "在待机，有" + RunFIFA20 + "台在运行FIFA");
            textBoxInfo.Text += "共找到" + HostDict.Count + "台PS4,有" + readyCount + "台在运行，有" + standByCount + "台在待机，有" + RunFIFA20 + "台在运行FIFA";
        }

        string PairIP = "";
        public void receive()
        {
            while (true&&!this.IsDisposed)
            {
                byte[] buffer = new byte[512];

                SocketAddress socketAddress = new SocketAddress(AddressFamily.InterNetwork);
                EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                int receiveNum = serverSocket.ReceiveFrom(buffer, ref endPoint);
                // Console.WriteLine(endPoint.ToString());
                if (receiveNum < 0)
                {
                    Console.WriteLine("Discovery thread failed to read from socket");
                    break;
                }
                if (receiveNum == 0)
                    continue;

                string response = Encoding.ASCII.GetString(buffer, 0, receiveNum);
                Console.WriteLine(response);
                if(response.Contains("RES2"))
                {
                    PairIP= endPoint.ToString().Split(':')[0];
                    continue;
                }


                DiscoveryHost discoveryHost = new DiscoveryHost(response);
                discoveryHost.IP = endPoint.ToString().Split(':')[0];
                
                if (!HostDict.ContainsKey(discoveryHost.IP))
                {
                    HostDict.Add(discoveryHost.IP, discoveryHost);
                }
                else
                {
                    HostDict[discoveryHost.IP] = discoveryHost;
                }

                // discovery_srch_response_parse(buffer);
            }

        }
        public void send()
        {

            byte[] data = Encoding.ASCII.GetBytes(srch_fmt);
            IPAddress broadcatAddress = NetworkUtils.GetBroadcastIp();
            EndPoint endPoint = new IPEndPoint(broadcatAddress, myProt);
            int result = serverSocket.SendTo(data, endPoint);



        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendwakeup("94998949", "192.168.3.6");
        }

        void sendwakeup(string registKey, string ip)
        {
            IPAddress broadcatAddress = NetworkUtils.GetBroadcastIp();
            EndPoint point = new IPEndPoint(broadcatAddress, 987);

            string wakeup =
                "WAKEUP * HTTP/1.1\n" +
                "client-type:vr\n" +
                "auth-type:R\n" +
                "model:w\n" +
                "app-type:r\n" +
                "user-credential:{0}\n" +
                "device-discovery-protocol-version:{1}\n\0";

            var request = string.Format(wakeup, ToUInt32(registKey, false), "00020020");
            byte[] buffer = Encoding.ASCII.GetBytes(request);
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
            serverSocket.SendTo(buffer, point);
        }
        public byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }
        public uint ToUInt32(string key, bool littleEndian)
        {
            byte[] value = HexStringToByteArray(key);
            if (littleEndian)
            {
                return value[0]
                       + ((uint)value[0 + 1] << 8)
                       + ((uint)value[0 + 2] << 16)
                       + ((uint)value[0 + 3] << 24);
            }
            else
            {
                return value[0 + 3]
                       + ((uint)value[0 + 2] << 8)
                       + ((uint)value[0 + 1] << 16)
                       + ((uint)value[0 + 0] << 24);
            }
        }

        Dictionary<string, string> reason = new Dictionary<string, string>() 
        {
            { "80108b09","APPLICATION_REASON_REGIST_FAILED" },
            { "80108b02","APPLICATION_REASON_INVALID_PSN_ID" },
            { "80108b10","APPLICATION_REASON_IN_USE" },
            { "80108b15","APPLICATION_REASON_CRASH" },
            { "80108b11","APPLICATION_REASON_RP_VERSION" },
            { "80108bff","APPLICATION_REASON_UNKNOWN" },
        };
            


        private void PairingBtn_Click(object sender, EventArgs e)
        {
            var pin = int.Parse(textBoxPin.Text);
      
            var userId = Convert.ToBase64String(BitConverter.GetBytes(long.Parse(textBoxUserId.Text)));
           

            //Search 
            byte[] data = Encoding.ASCII.GetBytes("SRC2");
            IPAddress broadcatAddress = NetworkUtils.GetBroadcastIp();
            EndPoint endPoint = new IPEndPoint(broadcatAddress, 9295);
            int result = serverSocket.SendTo(data, endPoint);

            Thread.Sleep(5000); 

            //CHECK PASS
            Session session = CryptoService.GetSessionForPin(pin);

            Dictionary<string, string> registrationHeaders = new Dictionary<string, string>();
            registrationHeaders.Add("Client-Type", "dabfa2ec873de5839bee8d3f4c0239c4282c07c25c6077a2931afcf0adc0d34f");
            registrationHeaders.Add("Np-AccountId", userId);
            byte[] payload = session.Encrypt(ByteUtil.HttpHeadersToByteArray(registrationHeaders));
           
            SecureRandom random = new SecureRandom();
            byte[] buffer = new byte[480];
            random.NextBytes(buffer);
            buffer[0] = 0x41;
            buffer[0x18D] = 0x41;
            byte[] paddedPayload = ByteUtil.ConcatenateArrays(buffer, payload);

            //CHECK PASS
            byte[] nonceDerivative = session.GetNonceDerivative();

            byte[] lastNonce = new byte[8];
            Array.Copy(nonceDerivative, 8, lastNonce, 0, 8);
            byte[] finalPaddedPayload;
            using (var ms = new MemoryStream())
            {
                ms.SetLength(paddedPayload.Length);
                ms.Write(paddedPayload, 0, paddedPayload.Length);
                ms.Seek(0xC7, SeekOrigin.Begin);
                ms.Write(lastNonce, 0, 8);
                ms.Seek(0x191, SeekOrigin.Begin);
                ms.Write(nonceDerivative, 0, 8);
                finalPaddedPayload = ms.ToArray();
            }
            //var ip = IPAddress.Parse(textBoxIP.Text);
            textBoxInfo.AppendText(PairIP);
            var ip = IPAddress.Parse(PairIP);
            IPEndPoint ipEndPoint = new IPEndPoint(ip, 9295);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipEndPoint);

            string requestData = "POST /sie/ps4/rp/sess/rgst HTTP/1.1\r\n HTTP/1.1\r\n" +
                                   $"HOST: {ip}\r\n" +
                                   "User-Agent: remoteplay Windows\r\n" +
                                   "Connection: close\r\n" +
                                   $"Content-Length: {finalPaddedPayload.Length}\r\n" +
                                   "RP-Version: 10.0\r\n" +
                                   "\r\n";

            socket.Send(ByteUtil.ConcatenateArrays(Encoding.UTF8.GetBytes(requestData),
                finalPaddedPayload));
            byte[] receiveBuffer = new byte[8192];
            int readBytes = socket.Receive(receiveBuffer);
            byte[] response = new byte[readBytes];
            Buffer.BlockCopy(receiveBuffer, 0, response, 0, response.Length);
            string httpResponse = Encoding.ASCII.GetString(receiveBuffer, 0, readBytes);
            Console.WriteLine(httpResponse);
            HttpStatusCode statusCode = HttpUtils.GetStatusCode(httpResponse);
            if (statusCode == HttpStatusCode.OK)
            {
                byte[] responseData = HttpUtils.GetBodyPayload(response);
                byte[] decryptedData = session.Decrypt(responseData);
                string registerHeaderInfoComplete = Encoding.UTF8.GetString(decryptedData);
                Console.WriteLine(registerHeaderInfoComplete);
                Dictionary<string, string> httpHeaders = ByteUtil.ByteArrayToHttpHeader(decryptedData);
                httpHeaders.TryGetValue("AP-Ssid", out var apSsid);
                httpHeaders.TryGetValue("AP-Bssid", out var apBssid);
                httpHeaders.TryGetValue("AP-Key", out var apKey);
                httpHeaders.TryGetValue("AP-Name", out var name);
                httpHeaders.TryGetValue("PS4-Mac", out var mac);
                httpHeaders.TryGetValue("PS4-RegistKey", out var registrationKey);
                httpHeaders.TryGetValue("PS4-Nickname", out var nickname);
                httpHeaders.TryGetValue("RP-KeyType", out var rpKeyType);
                httpHeaders.TryGetValue("RP-Key", out var rpKey);
                string registKey = Encoding.UTF8.GetString(HexUtil.Unhexlify(registrationKey));//6463643837666333
                string morning = Convert.ToBase64String(HexUtil.Unhexlify(rpKey));//9167a531dca0f4befc9383e7d79c85c9
                Console.WriteLine(registKey);
                Console.WriteLine(morning);
                textBoxInfo.AppendText("registKey: "+registKey+"\r\n");
                textBoxInfo.AppendText("morning: " + registKey + "\r\n");
                textBoxInfo.AppendText("mac: " + mac + "\r\n");
              

            }
            else
            {
                textBoxInfo.AppendText(httpResponse);
                var match = Regex.Match(httpResponse, "Reason: (?<value>.*?)\r\n\r\n");
                var resonCode = match.Groups["value"].Value;
                textBoxInfo.AppendText(reason[resonCode] + "\r\n");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
