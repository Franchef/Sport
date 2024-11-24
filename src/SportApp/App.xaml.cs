using DatiPedanaUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SportApp.Interfaces;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace SportApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration Configuration { get; init; }
        public IServiceProvider ServiceProvider { get; init; }

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SportViewModel>();
            services.AddScoped<MainWindow>();
            services
                .AddDatiPedanaFeature()
                .AddScoped<IEntryPoint, EntryPoint<DatiPedanaUI.DatiPedanaUserControl>>();
        }

        private void OnStartup(object sender, EventArgs e)
        {
            this.MainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            ServiceProvider.GetRequiredService<MainWindow>().Close();
        }
    }

}
