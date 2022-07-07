using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;


public enum IntervalQuality
{
    Major,
    Perfect,
    Minor,
    Diminished,
    Augmented
}


public class MusicFactory
{
    private readonly ILogger<MusicFactory> _logger;
    private readonly IServiceProvider _provider;

    public MusicFactory(ILogger<MusicFactory> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
        BuildNotes();
        BuildPitches();
        BuildScales();
    }

    private static string RemoveWhiteSpace(string s) => Regex.Replace(s, @"\s+", "");


    public IReadOnlyList<Note> Notes { get; private set; }

    #region Notes
    private void BuildNotes()
    {
        //https://www.allaboutmusictheory.com/piano-keyboard/enharmonic/
        //string[] sNotes = new[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        //notes = sNotes.Select(n => n.Length == 1 ? new Note { UnalteredName = n } : new Note { SharpName = n }).ToList();

        string[] sNotes = new[] { "C", null, "D", null, "E", "F", null, "G", null, "A", null, "B" };
        string[] sNotesSolfege = new[] { "do", null, "re", null, "mi", "fa", null, "sol", null, "la", null, "si" };

        int noteIndex = 0;
        Notes = Enumerable.Range(0, 12).
            Select(i =>
                new Note(this) { UnalteredName = sNotes[i], UnalteredNameSolfege = sNotesSolfege[i], Index = noteIndex++ }
                ).ToList();

        _logger.LogDebug("Adding notes...");
        //foreach (Note n in notes)
        for (int i = 0; i < Notes.Count; i++)
        {
            var note = Notes[i];

            //check for SharpName
            var previousNote = Notes[MyMod(i - 1, 12)];
            var nextNote = Notes[MyMod(i + 1, 12)];
            //if (note.IsNatural && previousNote.IsNatural) note.SharpName = $"{previousNote}#";
            //if (note.IsNatural && nextNote.IsNatural) note.FlatName = $"{previousNote}b";
            if (previousNote.IsNatural)
            {
                note.SharpName = $"{previousNote}#";
                note.SharpNameSolfege = $"{previousNote.UnalteredNameSolfege} #";
            }
            if (nextNote.IsNatural)
            {
                note.FlatName = $"{nextNote}b";
                note.FlatNameSolfege = $"{nextNote.UnalteredNameSolfege} b";
            }

            var previous2Note = Notes[MyMod(i - 2, 12)];
            var next2Note = Notes[MyMod(i + 2, 12)];
            if (previous2Note.IsNatural)
            {
                note.DoubleSharpName = $"{previous2Note}x";
                note.DoubleSharpNameSolfege = $"{previous2Note.UnalteredNameSolfege} x";
            }
            if (next2Note.IsNatural)
            {
                note.DoubleFlatName = $"{next2Note}bb";
                note.DoubleFlatNameSolfege = $"{next2Note.UnalteredNameSolfege} bb";
            }

            _logger.LogDebug("Added {n}, Solfege: {n2}", note.ToStringFull(), note.ToStringFull(true));
        }
    }

    public Note? GetNote(string note)
    {
        //remove whitespace
        note = RemoveWhiteSpace(note);

        var found = Notes.FirstOrDefault(n =>
           (n.UnalteredName?.Equals(note, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.SharpName?.Equals(note, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.FlatName?.Equals(note, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleSharpName?.Equals(note, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleFlatName?.Equals(note, StringComparison.OrdinalIgnoreCase) ?? false));
        if (found is not null) return found;

        //else search based on Solfege name
        return Notes.FirstOrDefault(n =>
           RemoveWhiteSpace(n.UnalteredNameSolfege ?? "").Equals(note, StringComparison.OrdinalIgnoreCase) ||
           RemoveWhiteSpace(n.SharpNameSolfege ?? "").Equals(note, StringComparison.OrdinalIgnoreCase) ||
           RemoveWhiteSpace(n.FlatNameSolfege ?? "").Equals(note, StringComparison.OrdinalIgnoreCase) ||
           RemoveWhiteSpace(n.DoubleSharpNameSolfege ?? "").Equals(note, StringComparison.OrdinalIgnoreCase) ||
           RemoveWhiteSpace(n.DoubleFlatNameSolfege ?? "").Equals(note, StringComparison.OrdinalIgnoreCase));
    }

    public Note GetNote(NoteName n) => Notes[(int)n];
    #endregion

    #region Pitches

    public IReadOnlyList<Pitch> Pitches { get; private set; }


    private void BuildPitches()
    {
        var LowA = new Pitch(this) { Note = GetNote("A")!, Class = 0, Index = 0 };

        _logger.LogDebug("Adding pitches...");

        var pitches = new List<Pitch>();
        pitches.Add(LowA);
        var currentPitch = LowA;
        for (int i = 1; i <= 7 * 12 + 3; i++)
        {
            Note nextNote = currentPitch.Note.Next;
            int nextClass = currentPitch.Note.Next > currentPitch.Note ?
                currentPitch.Class : currentPitch.Class + 1;
            Pitch nextPitch = new Pitch(this) { Note = nextNote, Class = nextClass, Index = i };
            pitches.Add(nextPitch);

            _logger.LogDebug("Added {p1} (S: {p2}, H: {p3}, E: {p4}, M: {p5})",
                nextPitch.ToString(PitchNotation.Scientific), nextPitch.ToString(PitchNotation.Solfege),
                nextPitch.ToString(PitchNotation.Helmholtz), nextPitch.ToString(PitchNotation.English),
                nextPitch.ToString(PitchNotation.Midi));

            currentPitch = nextPitch;
        }

        Pitches = pitches;
    }

    public Pitch? GetPitch(string pitch)
    {
        //remove whitespace
        pitch = RemoveWhiteSpace(pitch);
        return Pitches.FirstOrDefault(p =>
            p.ToString(PitchNotation.Scientific).Equals(pitch, StringComparison.OrdinalIgnoreCase) ||
            RemoveWhiteSpace(p.ToString(PitchNotation.Solfege)).Equals(pitch, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.Helmholtz).Equals(pitch, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.English).Equals(pitch, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.Midi).Equals(pitch, StringComparison.OrdinalIgnoreCase));
    }

    public Pitch? GetPitch(NoteName n, int classIndex)
        => classIndex >= 0 && classIndex <= 8 ?
        Pitches.FirstOrDefault(p => p.Note == Notes[(int)n] && p.Class == classIndex) : null;


    #endregion


    #region Scales

    public List<Scale> Scales { get; private set; }

    public List<string> CMajorNoteNames => "C D E F G A B C".Split(' ').ToList();
    public List<string> CMajorNoteNamesSolfege => "do re mi fa sol la si do".Split(' ').ToList();

    //Returns a 2nd Interval.
    public string? GetΝextIntervalNoteName(string currentNoteName, Note nextNote)
    {

        string currentNoteWithoutAccidentals = StripAccidentals(currentNoteName)!;
        bool useSolfege = CMajorNoteNamesSolfege.Contains(currentNoteWithoutAccidentals);

        if (!useSolfege)
        {
            string nextNoteWithoutAccidentals = CMajorNoteNames[
                CMajorNoteNames.IndexOf(currentNoteWithoutAccidentals) + 1];

            if (nextNote.UnalteredName == nextNoteWithoutAccidentals)
                return nextNote.UnalteredName;

            if (StripAccidentals(nextNote.SharpName) == nextNoteWithoutAccidentals)
                return nextNote.SharpName;

            if (StripAccidentals(nextNote.FlatName) == nextNoteWithoutAccidentals)
                return nextNote.FlatName;

            if (StripAccidentals(nextNote.DoubleSharpName) == nextNoteWithoutAccidentals)
                return nextNote.DoubleSharpName;

            if (StripAccidentals(nextNote.DoubleFlatName) == nextNoteWithoutAccidentals)
                return nextNote.DoubleFlatName;
        }
        else
        {
            string nextNoteWithoutAccidentals = CMajorNoteNamesSolfege[
                CMajorNoteNamesSolfege.IndexOf(currentNoteWithoutAccidentals) + 1];

            if (nextNote.UnalteredNameSolfege == nextNoteWithoutAccidentals)
                return nextNote.UnalteredNameSolfege;

            if (StripAccidentals(nextNote.SharpNameSolfege) == nextNoteWithoutAccidentals)
                return nextNote.SharpNameSolfege;

            if (StripAccidentals(nextNote.FlatNameSolfege) == nextNoteWithoutAccidentals)
                return nextNote.FlatNameSolfege;

            if (StripAccidentals(nextNote.DoubleSharpNameSolfege) == nextNoteWithoutAccidentals)
                return nextNote.DoubleSharpNameSolfege;

            if (StripAccidentals(nextNote.DoubleFlatNameSolfege) == nextNoteWithoutAccidentals)
                return nextNote.DoubleFlatNameSolfege;
        }

        return null;

    }


    private void BuildScales()
    {
        Scales = new List<Scale>();

        _logger.LogDebug("Adding major scales...");

        //build C major scale
        var cMajor = new Scale(this);
        cMajor.Notes = CMajorNoteNames.Select(s => GetNote(s)).ToList();
        cMajor.UpdatePitches();
        cMajor.ScaleType = ScaleType.Major;
        Scales.Add(cMajor);

        //build all others by transposition
        for (int iSemiTone = 1; iSemiTone <= 11; iSemiTone++)
            Scales.Add(cMajor.Transpose(iSemiTone));


        foreach (var scale in Scales)
            _logger.LogDebug("Added {scale}", scale);
    }

    public Scale? GetMajorScale(NoteName name) =>
        Scales.FirstOrDefault(sc => sc.Notes[0] == GetNote(name));

    public Scale? GetMajorScale(string note) =>
        Scales.FirstOrDefault(sc => sc.Notes[0] == GetNote(note));

    public string GetInterval(string startNote, int number, IntervalQuality quality)
    {
        //get the default (major interval)
        var scale = Scales.FirstOrDefault(s => s.Notes[0] == GetNote(startNote));

        int index = MyMod(number - 1, 8);
        var note = scale.Notes[index];

        var notes = scale.GetAllNoteNames().Where(nn => nn[0] == startNote).FirstOrDefault();
        string majorPerfectName = notes![index];

        //https://www.youtube.com/watch?v=8RPggfJ5bjQ&ab_channel=BradHarrisonMusic
        if (quality == IntervalQuality.Major || quality == IntervalQuality.Perfect)
            return majorPerfectName;
        else if (quality == IntervalQuality.Minor  )
        {
            if (majorPerfectName.EndsWith("x")) return StripAccidentals(majorPerfectName) + "#";
            else if (majorPerfectName.EndsWith("#")) return StripAccidentals(majorPerfectName);
            else return majorPerfectName + "b";
        }
        return "";
    }

    #endregion
}
