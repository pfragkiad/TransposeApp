using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransposeChordLibrary.Theory;

namespace TransposeChordLibrary;

public static class App
{
    public static IHost GetMusicApp()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services
                .AddSingleton<MusicFactory>()
                ;
            }).UseSerilog((context, configuration) =>
            {
                configuration.MinimumLevel.Debug();
                configuration.WriteTo.Console();
            });
        return builder.Build();
    }
}
