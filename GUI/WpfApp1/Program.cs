using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1
{

 
public class Program
    {
        [STAThread]
        public static void Main()
        {
            // создаем хост приложения
            var host = Host.CreateDefaultBuilder()
                // внедряем сервисы
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<VideoStreamControler>();//AddTransient создает экземпляр для каждого где нужен
                    services.AddSingleton<MainWindowViewModel>();  
                    services.AddSingleton<EngineController>();
                    services.AddSingleton<VideoStreamViewModel>();
                    services.AddSingleton<ControlViewModel>();                   
                    services.AddSingleton<MainWindow>();
                })
                .Build();
            // получаем сервис - объект класса App
            var app = host.Services.GetService<App>();
            // запускаем приложения
            app?.Run();
        }
    }
}
