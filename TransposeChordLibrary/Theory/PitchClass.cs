using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;

public class PitchClass
{
    public int Class { get; init; }

    public string OctaveName => OctaveNames[Class];

    public string NotesPrefix => NotesPrefixes[Class];

    public static string[] OctaveNames = new string[]
    {
    "sub-contra", //0
    "contra",
    "great",
    "small",
    "1-line/2nd small",
    "2-line/3rd small",
    "3-line/4th small",
    "4-line/5th small",
    "5-line/6th small" //8
    };

    public static string[] NotesPrefixes = new string[]
    {
    "triple", //C0
    "double pedal",
    "pedal",
    "bass",
    "middle",
    "treble",
    "top/high",
    "double top/double high",
    "triple top/triple high" //C8

    };
}
