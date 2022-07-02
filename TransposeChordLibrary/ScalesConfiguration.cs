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


    public List<string> Notes { get; set; }

    public string TransposeNote(string note, int semitones, TransposeSettings settings = TransposeSettings.Normal)
    {
        if(note.EndsWith("b")) //get the sharp enharmonic
            note = Enharmonics.FirstOrDefault(e => e[1].Equals(note, StringComparison.OrdinalIgnoreCase))[0];

        int index = Notes.IndexOf(note);
        string newNote = Notes[MyMod(index + semitones, 12)];

        if (!newNote.EndsWith("b") && !newNote.EndsWith("#") ||
            settings == TransposeSettings.ForceSharps) return newNote;

        if(settings ==TransposeSettings.ForceFlats && newNote.EndsWith("#"))
            return Enharmonics.FirstOrDefault(e => e[0].Equals(newNote, StringComparison.OrdinalIgnoreCase))[1];

        //settings is normal => we want to return a note that is the base of an existing scale
        Scale? scale = Scales.FirstOrDefault(s => s.Name.Equals(newNote, StringComparison.OrdinalIgnoreCase));
        if (scale is not null) return newNote;
        
        if(newNote.EndsWith("b")) //should return the #
            return Enharmonics.FirstOrDefault(e => e[1].Equals(newNote, StringComparison.OrdinalIgnoreCase))[0];

        //else if (newNote.EndsWith("#")) //should return the b
        return Enharmonics.FirstOrDefault(e => e[0].Equals(newNote, StringComparison.OrdinalIgnoreCase))[1];
    }


    public List<List<string>> Enharmonics {get;set;}

    public static ScalesConfiguration ReadFromFile(string file = "scales.json")
    {
        IConfigurationBuilder b = new ConfigurationBuilder();
        //NUGET: Microsoft.Extensions.Configuration.Json
        b.AddJsonFile(file);
        IConfiguration config = b.Build();

        var section = config.GetSection("scales");
        var scales = new ScalesConfiguration();
        //NUGET: Microsoft.Extensions.Configuration.Binder
        config.GetSection(ScalesSection).Bind(scales);

        return scales;
    }
}
