using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Station
    {
        private Socket socket;
        private IPEndPoint endPoint;
        private const int BUFFER_SIZE = 2048;
        private byte[] buffer = new byte[BUFFER_SIZE];
        private AutoResetEvent recieveEvent = new AutoResetEvent(false);
        private string buff = "";
        private Dictionary<string, string> recievedData;


        public Station(string hostName, int port, string netpingIp)
        {
            this.hostName = hostName;
            this.port = port;
            this.netpingIp = netpingIp;

            endPoint = getIPEndPointFromHostName();
        }

        private IPEndPoint getIPEndPointFromHostName()
        {
            try {
                var addresses = System.Net.Dns.GetHostAddresses(hostName);
                if (addresses.Length == 0)
                {
                    throw new ArgumentException(
                        "Unable to retrieve address from specified host name.",
                        "hostName"
                    );
                }
                return new IPEndPoint(addresses[0], port);
            } catch(Exception e)
            {
                return null;
            }
        }
        private Task<Socket> connectToServerAsync()
        {
            return Task.Run(() =>
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = getIPEndPointFromHostName();
                if (ipEndPoint == null) return null;
                IAsyncResult result = client.BeginConnect(ipEndPoint, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(3000, true);

                if (client.Connected)
                {
                    client.EndConnect(result);
                    return client;
                }
                else
                {
                    client.Close();
                    return null;
                }
            });
        }
        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            try
            {
                int received = current.EndReceive(AR);
                if (received > 0)
                {
                    buff += Encoding.UTF8.GetString(buffer, 0, received);
                    current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
                }
                else
                {
                    recievedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(buff);
                    IPEndPoint end = current.RemoteEndPoint as IPEndPoint;
                    current.Close();
                    buff = "";
                    recieveEvent.Set();
                }
            }
            catch (Exception ex)
            {
                buff = "";
            }
        }

        public async Task<string> getNetpingStatusAsync()
        {
            try {
                var client = new WebClient { Credentials = new NetworkCredential("sysadm", "04summer") };
                string state = await client.DownloadStringTaskAsync("http://" + netpingIp + "/relay.cgi?r1");
                if (state == "relay_result('ok', 1, 1);")
                {
                    return "on";
                }
                else if (state == "relay_result('ok', 0, 0);")
                {
                    return "off";
                }
            } catch (Exception e)
            {
                return "error";
            }
            return "error";
        }    
        public async Task<bool> pingAsync()
        {
            isPingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = await pinger.SendPingAsync(hostName, 2000);
                isPingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {

            }
            return isPingable;
        }

        private Task<Image> getImage()
        {
            return Task.Run(() =>
            {
                recieveEvent.WaitOne();
                var bytes = Convert.FromBase64String(recievedData["img"]);
                MemoryStream ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            });

        }
        public async Task<Image> getScreenshotAsync()
        {
            socket = await connectToServerAsync();
            if (socket == null) return null;
            var data = new Dictionary<string, string>();
            data["method"] = "getScreenshot";
            string json = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(json));
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            return await getImage();
        }

        public async Task<string> setNetping()
        {
            try
            {
                var client = new WebClient { Credentials = new NetworkCredential("sysadm", "04summer") };
                string result = await client.DownloadStringTaskAsync("http://" + netpingIp + "/relay.cgi?r1=f");
                if (result == "relay_result('ok');")
                {
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
            }
            return "error";
        }

        public async Task<bool> reboot()
        {
            socket = await connectToServerAsync();
            if (socket == null) return false;
            var data = new Dictionary<string, string>();
            data["method"] = "reboot";
            string json = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(json));
            return true;
        }

        public async Task<bool> shutdown()
        {
            socket = await connectToServerAsync();
            if (socket == null) return false;
            var data = new Dictionary<string, string>();
            data["method"] = "shutdown";
            string json = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(json));
            return true;
        }

        public string hostName { get; set; }
        public string netpingIp { get; set; }
        public int port { get; set; }
        public int ip { get; set; }
        public bool isPingable { get; set; }
    }
}
