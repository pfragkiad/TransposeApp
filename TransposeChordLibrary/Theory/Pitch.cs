using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;

//https://www.liveabout.com/pitch-notation-and-octave-naming-2701389





public class Pitch
{
    internal int Index;

    private readonly MusicFactory _factory;


    IReadOnlyList<Pitch> Pitches => _factory.Pitches;

    public Pitch MinValue => Pitches[0];
    public Pitch MaxValue => Pitches[Pitches.Count - 1];

    internal Pitch(MusicFactory factory)
    {
        _factory = factory;
    }

    public Note Note { get; init; }

    //PitchClass (0-8)
    public int Class { get; init; }

    //https://en.wikipedia.org/wiki/Scientific_pitch_notation
    public int MidiNumber => Index + 21; //21 corresponds to low A

    public double Frequency => 27.5 * Math.Pow(2.0, Index / 12.0);

    public override string ToString() => $"{Note}{Class}";

    public string ToString(PitchNotation notation)
    {
        switch (notation)
        {
            case PitchNotation.Helmholtz:
                if (Class >= 3)
                    return $"{Note.ToString().ToLower()}{new string('\'', Class - 3)}";
                else //if (Class<=2) 
                    return $"{Note}{new string(',', 2 - Class)}";
                break;
            case PitchNotation.English:
                //https://www.dacapoalcoda.com/note-names
                if (Class >= 3)
                    return $"{Note.ToString().ToLower()}{new string('\'', Class - 3)}";
                else //if (Class<=2) 
                    return string.Concat(Enumerable.Repeat($"{Note}", 3 - Class));
            case PitchNotation.Solfege:
                return $"{Note.ToStringSolfege()}{(Class - 1 >= 1 ? Class - 1 : Class - 2)}";
            case PitchNotation.Midi:
                return $"#{MidiNumber}";
            default:
            case PitchNotation.Scientific:
                return $"{Note}{Class}";
        }
    }

    public Pitch? AddSemitones(int semiTones) =>
                Index + semiTones <= Pitches.Count - 1 && Index + semiTones >= 0 ?
                    Pitches[Index + semiTones] : null;
      
     #region Operator overloading


    public Pitch? Next { get => AddSemitones(1); }
    public Pitch? Previous { get => AddSemitones(-1); }

    #region Operator overloading
    public static Pitch? operator ++(Pitch pitch) => pitch.Next;
    public static Pitch? operator --(Pitch pitch) => pitch.Previous;

    public static Pitch? operator +(Pitch pitch, int semiTones) => pitch.AddSemitones(semiTones);
    public static Pitch? operator +(int semiTones, Pitch pitch) => pitch.AddSemitones(semiTones);

    public static Pitch? operator -(Pitch pitch, int semiTones) => pitch.AddSemitones(-semiTones);

    public static Pitch? operator -(int semiTones, Pitch pitch) => pitch.AddSemitones(-semiTones);

    public static int operator -(Pitch pitch1, Pitch pitch2) => pitch1.Index - pitch2.Index;

    public static bool operator <(Pitch pitch1, Pitch pitch2) => pitch1.Index < pitch2.Index;
    public static bool operator <=(Pitch pitch1, Pitch pitch2) => pitch1.Index <= pitch2.Index;
    public static bool operator >(Pitch pitch1, Pitch pitch2) => pitch1.Index > pitch2.Index;
    public static bool operator >=(Pitch pitch1, Pitch pitch2) => pitch1.Index >= pitch2.Index;

    #endregion


    #endregion

}
