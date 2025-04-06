using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfApp1.Model;
using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Tracing;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.ViewModel
{
    public class ControlViewModel : Window, INotifyPropertyChanged
    {
        private VideoStreamControler videoStreamControler;
        private EngineController engineController;
        private Brush colorElipse;
        private double powerValue;
        private byte power;
        private string textLog;
        public ControlViewModel(VideoStreamControler videoStreamControler, EngineController engineController)
        {
            this.videoStreamControler = videoStreamControler;
            this.engineController = engineController;
            videoStreamControler.UdpGetState += VideoStreamControler_UdpGetState;
            engineController.UdpLogSend += EngineController_UdpLogSend;
            this.colorElipse = new SolidColorBrush(Color.FromArgb(0xFF, 179, 249, 249));
            this.powerValue = 25;
            this.power = 25;
            this.textLog = "ready";
            KeyBordDown = new DelegateCommand(KeyBordDownAction);
            KeyBordUP = new DelegateCommand(KeyBordUpAction);
            StartUdp = new DelegateCommand(StartUdpAction);
            SliderValueChange = new DelegateCommand(SliderValueChangeAction);
        }

        private void EngineController_UdpLogSend(string message)
        {
           TextLog = message;
        }

        private void VideoStreamControler_UdpGetState(bool state)
        {
            if (state)
            {
                ColorElipse = new SolidColorBrush(Color.FromArgb(0xFF, 156, 252, 143));
            }
            else
            {
                ColorElipse = new SolidColorBrush(Color.FromArgb(0xFF, 160, 75, 69));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public ICommand StartUdp { get; }
        public ICommand KeyBordDown { get; }
        public ICommand KeyBordUP { get; }
        public ICommand SliderValueChange { get; }
       
        public Brush ColorElipse
        {
            get { return colorElipse; }
            set
            {                
                if (Equals(value, this.colorElipse)) return;
                this.colorElipse = value;
                this.OnPropertyChanged();
            }
        }
        public double PowerValue
        {
            get { return powerValue; }
            set
            {
                if (Equals(value, this.powerValue)) return;
                this.powerValue = value;
                this.power = (byte)(int)powerValue;
                this.OnPropertyChanged();
            }
        }
        public string TextLog
        {
            get { return textLog; }
            set
            {
                if (Equals(value, this.textLog)) return;
                this.textLog = value;               
                this.OnPropertyChanged();
            }
        }
        private void KeyBordDownAction(object obj)
        {
            if (obj is KeyEventArgs args)
            {
                switch (args.Key)
                {
                    case Key.W:
                        engineController.StartMoveOnKey(power, 0x01);                        
                        break;
                    case Key.A:
                        engineController.StartMoveOnKey(power, 0x02);                        
                        break;
                    case Key.S:
                        engineController.StartMoveOnKey(power, 0x03);
                        
                        break;
                    case Key.D:
                        engineController.StartMoveOnKey(power, 0x04);
                        break;
                    default:
                        break;
                }
            }             
        }
        private void KeyBordUpAction(object obj)
        {
            if (obj is KeyEventArgs args)
            {
                switch (args.Key)
                {
                    case Key.W:
                        engineController.StopModeOnKey();                    
                        break;
                    case Key.A:
                        engineController.StopModeOnKey();                   
                        break;
                    case Key.S:
                        engineController.StopModeOnKey();                       
                        break;                      
                    case Key.D:
                        engineController.StopModeOnKey();                      
                        break;
                    default:
                        break;
                }
            }
        }
        private async void StartUdpAction(object obj)
        {            
            await videoStreamControler.Connect();
            if(!videoStreamControler.UdpTaskRunner.IsRunning)
            videoStreamControler.UdpTaskRunner.Start();           
            else
            {
                await videoStreamControler.UdpTaskRunner.StopAsync();
                //Task.Run(() => videoStreamControler.UdpTaskRunner.Start());
                videoStreamControler.UdpTaskRunner.Start();
            }
        }
        private void SliderValueChangeAction(object obj)
        {
            if (obj is RoutedPropertyChangedEventArgs<double> args)
            {
                var newVal = (int)Math.Round(args.NewValue, 0);
                PowerValue = (double)newVal;
            }
        }

    }
}
