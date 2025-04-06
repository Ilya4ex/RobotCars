using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace WpfApp1.Model
{
    public class VideoStreamControler 
    {
        public string AddressHost { get; set; } = "192.168.4.2";

        public string AddressRemote { get; set; } = "192.168.4.1";
        public int HostPort { get; set; } = 25001;
        public int RemotePort { get; set; } = 25000;
        private UdpClient? client;

        private IPEndPoint? localEndPoint;

        private IPEndPoint? remoteEndPoint;
        public TaskRunner UdpTaskRunner { get; }

        public delegate void UdpFrameHandler(byte[] frame);

        public event UdpFrameHandler? UdpGetFrame;

        public delegate void UdpStateHandler(bool state);

        public event UdpStateHandler? UdpGetState;

        private int UdpPacketLength = 1440+1;
       
        public VideoStreamControler()
        {            
            this.localEndPoint = new IPEndPoint(IPAddress.Parse(AddressHost), HostPort);
            this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(AddressRemote), RemotePort);
            //Connect();
            UdpTaskRunner = new TaskRunner(this.UdpCycle);            
        }
       
        protected virtual Task UDPFlush()
        {
            try
            {
                while (this.client?.Available > 0)
                {
                    _ = this.client.Receive(ref this.localEndPoint);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }
        }       
        public async Task UdpCycle(CancellationToken token)
        {
            var tempframe = new List<byte[]>();           
            await UDPFlush();
            while (!token.IsCancellationRequested)
            {
                var dataRead = await UdpReadData(token);
                if (dataRead.Length == UdpPacketLength && dataRead[0] > 20 && dataRead[1] == 255 && dataRead[2] == 216 && dataRead[3] == 255)
                {
                    tempframe.Clear();
                    tempframe.Add(dataRead);
                }
                else if (dataRead.Length == UdpPacketLength)
                {
                    tempframe.Add(dataRead);
                }
                if (dataRead.Length > 2 && dataRead.Length != UdpPacketLength && dataRead[0] > 20 && dataRead[dataRead.Length - 2] == 255 && dataRead[dataRead.Length - 1] == 217)
                {
                    tempframe.Add(dataRead);
                    await FindCorrectFrame(tempframe);
                    tempframe.Clear();
                    await UDPFlush();
                }
            }
        }
        public async Task UdpSendMessage(byte[] message)
        {
            try
            {
                if (this.client == null)
                {
                    return;
                }
                await this.client.SendAsync(message, message.Length, this.remoteEndPoint);
            }
            catch{ }
        }
        private Task FindCorrectFrame(List<byte[]> data)//CancellationToken token = default
        {
            var countPack = data[0][0] - 20;
            if (data.Count == countPack && data.Last()[0] == countPack+20)
            {
                var fullpack = new List<byte>();
                for (var i = 0;  i< countPack; i++)
                {
                    if (data[i][0] == i || data[i][0] == countPack + 20)
                    {
                        fullpack.AddRange(data[i][1..]);
                    }
                    else return Task.CompletedTask;
                }
                UdpGetFrame?.Invoke(fullpack.ToArray());
            }          
            return Task.CompletedTask;
        }
        private static void UDPListener()
        {
            Task.Run(async () =>
            {
                using (var udpClient = new UdpClient(11000))
                {
                    string loggingEvent = "";
                    while (true)
                    {
                        //IPEndPoint object will allow us to read datagrams sent from any source.
                        var receivedResults = await udpClient.ReceiveAsync();
                        loggingEvent += Encoding.ASCII.GetString(receivedResults.Buffer);
                    }
                }
            });
        }

        protected Task<byte[]> UdpReadData(CancellationToken token)//all scan in one freq add timeot
        {
            try
            {
                while (this.client != null && !token.IsCancellationRequested)
                {
                    if(this.client.Available > 0)
                    {
                        var dataRead = this.client.Receive(ref this.localEndPoint);
                         return Task.FromResult(dataRead);   
                    }
                }
                return Task.FromResult(Array.Empty<byte>());
            }
            catch (Exception ex)
            {
                return Task.FromResult(Array.Empty<byte>());
            }        
        }
        public Task Connect()//CancellationToken token = default
        {
            if (this.client != null) return Task.CompletedTask;
            try
            {
                this.client = new UdpClient(localEndPoint);
                this.client.Client.ReceiveBufferSize = 1460;
                this.UdpGetState?.Invoke(true);
            }
            catch //(Exception e)
            {
                UdpGetState?.Invoke(false);
            }
            return Task.CompletedTask;
        }       

        public Task Disconnect()
        {
            //this.alinxManager.OnGetSpectrum -= AlinxManager_OnOnGetSpectrum;
            return Task.CompletedTask;
        }
    }
}
