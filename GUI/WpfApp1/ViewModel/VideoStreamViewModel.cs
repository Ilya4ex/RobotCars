using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfApp1.Model;
using System.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Tracing;
using System.Windows.Input;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Windows.Controls;

namespace WpfApp1.ViewModel
{

    public class VideoStreamViewModel : Window, INotifyPropertyChanged
    {
        private BitmapImage? imageSourse;

        private VideoStreamControler videoStreamControler;

        public VideoStreamViewModel(VideoStreamControler videoStreamControler)
        {
            this.videoStreamControler = videoStreamControler;
            this.videoStreamControler.UdpGetFrame += this.UdpGetFrameAction;
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
        private void UdpGetFrameAction(byte[] frame)
        {
            try
            {
                BitmapImage bi = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(frame))
                {
                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                    bi.Freeze();                                     
                    Dispatcher.BeginInvoke(new ThreadStart(delegate { ImageSourse = bi; }));              
                   
                }          
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource");
            }           
        }       
        public BitmapImage? ImageSourse
        {
            get => this.imageSourse;
            set
            {
                if (Equals(value, this.imageSourse))
                {
                    return;
                }
                this.imageSourse = value;               
                this.OnPropertyChanged();
            }
        }        
    }
}
