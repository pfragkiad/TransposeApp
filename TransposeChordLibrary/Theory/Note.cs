using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;

public enum NoteName
{
    C, BSharp = C, DDoubleFlat = C,
    CSharp, DFlat = CSharp, BDoubleSharp = CSharp,
    D, CDoubleSharp = D, EDoubleFlat = D,
    DSharp, EFlat = DSharp, FDoubleFlat = DSharp,
    E, FFlat = E, DDoubleSharp = E,
    F, ESharp = F, GDoubleFlat = F,
    FSharp, GFlat = FSharp, EDoubleSharp = FSharp,
    G, FDoubleSharp = G, ADoubleFlat = G,
    GSharp, AFlat = GSharp,
    A, GDoubleSharp = A, BDoubleFlat = A,
    B, CFlat = B, ADoubleSharp = B
}


public class Note
{
    internal Note(MusicFactory musicFactory)
    {
        _musicFactory = musicFactory;
    }

    IReadOnlyList<Note> Notes => _musicFactory.Notes;


    internal int Index;

    public string? UnalteredName { get; internal set; }
    public string? UnalteredNameSolfege { get; internal set; }

    public bool IsNatural { get => !string.IsNullOrWhiteSpace(UnalteredName); }

    private readonly MusicFactory _musicFactory;


    #region Enharmonic notes

    public string? SharpName { get; internal set; }
    public string? SharpNameSolfege { get; internal set; }
    public string? FlatNameSolfege { get; internal set; }
    public string? FlatName { get; internal set; }

    public string? DoubleSharpName { get; internal set; }
    public string? DoubleSharpNameSolfege { get; internal set; }
    public string? DoubleFlatName { get; internal set; }

    public string? DoubleFlatNameSolfege { get; internal set; }
    #endregion


    #region ToString
    public override string ToString() => UnalteredName ?? SharpName ?? FlatName ?? "<Invalid>";

    public string ToStringSolfege() => UnalteredNameSolfege ?? SharpNameSolfege ?? FlatNameSolfege ?? "<Invalid>";

    public string ToString(bool preferSharp, bool useSolfege) =>
        !useSolfege ?
        UnalteredName ?? (preferSharp ? SharpName ?? FlatName : FlatName ?? SharpName) ?? "<Invalid>" :
        UnalteredNameSolfege ?? (preferSharp ? SharpNameSolfege ?? FlatNameSolfege : FlatNameSolfege ?? SharpNameSolfege) ?? "<Invalid>";

    #endregion


    public string[] GetEnharmonicNoteNames(bool useSolfege = false) =>
           !useSolfege ?
            new string[] { UnalteredName, SharpName, FlatName, DoubleSharpName, DoubleFlatName }
                .Where(n => !string.IsNullOrWhiteSpace(n)).ToArray() :
            new string[] { UnalteredNameSolfege, SharpNameSolfege, FlatNameSolfege, DoubleSharpNameSolfege, DoubleFlatNameSolfege }
                .Where(n => !string.IsNullOrWhiteSpace(n)).ToArray();


    public string ToStringFull(bool useSolfege = false)
    {
        var enharmonicNotes = GetEnharmonicNoteNames(useSolfege);
        return IsNatural ?
                 $"{(!useSolfege ? UnalteredName : UnalteredNameSolfege)} ({string.Join(", ", enharmonicNotes)})" :
                 $"{(!useSolfege ? SharpName : SharpNameSolfege)} ({string.Join(", ", enharmonicNotes)})";
    }

    public Note AddSemitones(int semiTones) => Notes[MyMod(Index + semiTones, 12)];

    public Note Next { get => AddSemitones(1); }
    public Note Previous { get => AddSemitones(-1); }


    #region Operator overloading
    public static Note operator ++(Note note) => note.Next;
    public static Note operator --(Note note) => note.Previous;



    public static Note operator +(Note note, int semiTones) => note.AddSemitones(semiTones);
    public static Note operator +(int semiTones, Note note) => note.AddSemitones(semiTones);
    public static Note operator -(Note note, int semiTones) => note.AddSemitones(-semiTones);
    public static Note operator -(int semiTones, Note note) => note.AddSemitones(-semiTones);

    public static int operator -(Note note1, Note note2) => MyMod(note1.Index - note2.Index, 12);

    public static bool operator <(Note note1, Note note2) => note1.Index < note2.Index;
    public static bool operator <=(Note note1, Note note2) => note1.Index <= note2.Index;
    public static bool operator >(Note note1, Note note2) => note1.Index > note2.Index;
    public static bool operator >=(Note note1, Note note2) => note1.Index >= note2.Index;

    #endregion

}


