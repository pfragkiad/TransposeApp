using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transpose;

namespace WinTranspose
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            ServiceProvider = CreateHostBuilder().Build().Services;

            //  Application.Run(new Form1());
            Application.Run(ServiceProvider.GetService<Form1>());
        }


        public static IServiceProvider ServiceProvider { get; private set; }
        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => {
                    services
                    .AddTransient<Form1>()
                    .AddSingleton<ChordTransposer>();
                });
        }
    }
}