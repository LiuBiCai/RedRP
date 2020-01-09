using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
            }
            Console.WriteLine("共找到" + HostDict.Count + "台PS4,有" + readyCount + "在运行,有" + standByCount + "在待机，有" + RunFIFA20 + "台在运行FIFA");
            textBoxInfo.Text = "共找到" + HostDict.Count + "台PS4,有" + readyCount + "台在运行，有" + standByCount + "台在待机，有" + RunFIFA20 + "台在运行FIFA";
        }
        void receive()
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
                //Console.WriteLine(response);
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
        void send()
        {

            byte[] data = Encoding.ASCII.GetBytes(srch_fmt);
            IPAddress broadcatAddress = NetworkUtils.GetBroadcastIp();
            EndPoint endPoint = new IPEndPoint(broadcatAddress, myProt);
            int result = serverSocket.SendTo(data, endPoint);



        }
    }
}
