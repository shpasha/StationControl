using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    class Program
    {
        static Socket server = null;
        const int BUFFER_SIZE = 2048;
        static byte[] buffer = new byte[BUFFER_SIZE];
        static int port = 21;
        static Dictionary<string, Func<Dictionary<string, string>, Socket, bool>> methods;
        static void Main(string[] args)
        {
            methods = new Dictionary<string, Func<Dictionary<string, string>, Socket, bool>>();
            methods["getScreenshot"] = getScreenshot;
            methods["shutdown"] = shutdown;
            methods["reboot"] = reboot;

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(500);
            server.BeginAccept(AcceptCallback, null);

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Console.WriteLine("Server started at " + ipAddress.ToString() + ":" + port.ToString());

            Console.ReadLine();
        }

        static bool getScreenshot(Dictionary<string, string> data, Socket socket)
        {
            Bitmap printScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(printScreen as Image);
            g.CopyFromScreen(0, 0, 0, 0, printScreen.Size);
            MemoryStream ms = new MemoryStream();
            printScreen.Save(ms, ImageFormat.Jpeg);
            string base64 = Convert.ToBase64String(ms.ToArray());
            data["method"] = "getScreenshot";
            data["img"] = base64;
            string msg = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(msg));
            return true;
        }

        static bool shutdown(Dictionary<string, string> data, Socket socket)
        {

            data["method"] = "shutdownResult";
            data["result"] = "true"; ;
            string msg = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(msg));

            Process.Start("shutdown", "/s /t 0");
            return true;
        }

        static bool reboot(Dictionary<string, string> data, Socket socket)
        {

            data["method"] = "rebootResult";
            data["result"] = "false"; 
            string msg = JsonConvert.SerializeObject(data);
            socket.Send(Encoding.UTF8.GetBytes(msg));

            Process.Start("shutdown", "/r /t 0");
            return true;
        }

        static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = server.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            server.BeginAccept(AcceptCallback, null);
        }

        static string s = "";
        static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received = 0;
            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException e)
            {
                current.Close();                      
            }
            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string message = Encoding.ASCII.GetString(recBuf);
            try {
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
                methods[data["method"]](data, current);
                Console.WriteLine(message);
            } catch (Exception e)
            {
                
            }
            current.Shutdown(SocketShutdown.Send);
            current.Close();
        }
    }
}
