using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transpose;

public class Scale
{
    public string Name { get; set; }

    public List<string> Chords { get; set; }

    public override string ToString() => Name;
}

public class ScalesConfiguration
{
    public static string ScalesSection = "scalesConfiguration";

    public List<Scale> Scales { get; set; }

    public static ScalesConfiguration ReadFromFile(string file = "appsettings.json")
    {
        IConfigurationBuilder b = new ConfigurationBuilder();
        //NUGET: Microsoft.Extensions.Configuration.Json
        b.AddJsonFile("appsettings.json");
        IConfiguration config = b.Build();

        var section = config.GetSection("scales");
        var scales = new ScalesConfiguration();
        //NUGET: Microsoft.Extensions.Configuration.Binder
        config.GetSection(ScalesSection).Bind(scales);

        return scales;
    }
}
