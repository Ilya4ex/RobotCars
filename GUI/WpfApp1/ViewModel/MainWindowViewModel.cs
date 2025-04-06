using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private VideoStreamViewModel videoStreamViewModel;
        private ControlViewModel controlViewModel;     
        public MainWindowViewModel(VideoStreamControler videoStreamControler, VideoStreamViewModel videoStreamViewModel, ControlViewModel controlViewModel)
        {
            this.videoStreamViewModel = videoStreamViewModel;
            this.controlViewModel = controlViewModel;           
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
        public VideoStreamViewModel VideoStreamViewModel
        {
            get => this.videoStreamViewModel;
            set
            {
                if (this.SetField(ref this.videoStreamViewModel, value))
                {
                    this.videoStreamViewModel = value;
                }
            }
        }
        public ControlViewModel ControlViewModel
        {
            get => this.controlViewModel;
            set
            {
                if (this.SetField(ref this.controlViewModel, value))
                {
                    this.controlViewModel = value;
                }
            }
        }
    }
}
