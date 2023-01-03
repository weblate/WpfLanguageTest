using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WpfLanguageTest.I18n;

namespace WpfLanguageTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {  
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; private set; }

        #region IHost services

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<II18nService, I18nService>();

            }).Build();

        #endregion

        public App() : base()
        {
            Services = _host.Services;
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var language = Services.GetRequiredService<II18nService>();
            await language.ApplyLanguageAsync("EN");
            //Application.Current.Dispatcher.Invoke(() =>  language.ApplyLanguageAsync("EN", true));

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }
}
