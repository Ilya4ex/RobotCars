using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp1.Model
{
    public class EngineController
    {
        private VideoStreamControler videoStreamControler;     
        private DispatcherTimer timer;
        private int intrvalTimMs = 60;
        private byte powerEngine;
        private byte directionrEngine;

        public EngineController(VideoStreamControler videoStreamControler) 
        { 
            this.videoStreamControler = videoStreamControler;
            this.timer = CreateTimer();          
        }

        public delegate void UdpLogHandler(string message);

        public event UdpLogHandler? UdpLogSend;

        public void StartMoveOnKey(byte power, byte direction)
        {
            powerEngine = power;
            directionrEngine = direction;
            //timer = CreateTimer();
            UdpLogSend?.Invoke("go "+directionrEngine+", power " + (double)powerEngine * 100 / 50 + " %"); //
            videoStreamControler.UdpSendMessage(new byte[] { directionrEngine, powerEngine });
            timer.Start();
        }
        public void StopModeOnKey() 
        {
            timer.IsEnabled = false;
            //timer.Stop();            
            UdpLogSend?.Invoke("off");
            timer.Stop();
        }
        private void TimCollback(object? sender, EventArgs e)
        {
            UdpLogSend?.Invoke("go "+directionrEngine+", power " + (double)powerEngine * 100 / 50 + " %");
            videoStreamControler.UdpSendMessage(new byte[] { directionrEngine, powerEngine });
            //Console.WriteLine("Direction");
        }
        
        private DispatcherTimer CreateTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimCollback);
            timer.Interval = TimeSpan.FromMilliseconds(intrvalTimMs);            
            return timer; 
        }
    }
}
