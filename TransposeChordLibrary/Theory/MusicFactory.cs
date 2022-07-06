using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;

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
                note.SharpNameSolfege = $"{previousNote.UnalteredNameSolfege}#";
            }
            if (nextNote.IsNatural)
            {
                note.FlatName = $"{nextNote}b";
                note.FlatNameSolfege = $"{previousNote.UnalteredNameSolfege}b";
            }

            var previous2Note = Notes[MyMod(i - 2, 12)];
            var next2Note = Notes[MyMod(i + 2, 12)];
            if (previous2Note.IsNatural)
            {
                note.DoubleSharpName = $"{previous2Note}x";
                note.DoubleSharpNameSolfege = $"{previous2Note.UnalteredNameSolfege}x";
            }
            if (next2Note.IsNatural)
            {
                note.DoubleFlatName = $"{next2Note}bb";
                note.DoubleFlatNameSolfege = $"{next2Note.UnalteredNameSolfege}bb";
            }

            _logger.LogDebug("Added {n}, Solfege: {n2}", note.ToStringFull(), note.ToStringFull(true));
        }
    }

    public Note? GetNote(string s)
    {
        //remove whitespace
        s = Regex.Replace(s, @"\s+", "");

        var found = Notes.FirstOrDefault(n =>
           (n.UnalteredName?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.SharpName?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.FlatName?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleSharpName?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleFlatName?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false));
        if (found is not null) return found;

        //else search based on Solfege name
        return Notes.FirstOrDefault(n =>
           (n.UnalteredNameSolfege?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.SharpNameSolfege?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.FlatNameSolfege?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleSharpNameSolfege?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false) ||
           (n.DoubleFlatNameSolfege?.Equals(s, StringComparison.OrdinalIgnoreCase) ?? false));
    }

    public Note GetNote(Notes n) => Notes[(int)n];
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

    public Pitch? GetPitch(string s)
    {
        //remove whitespace
        s = Regex.Replace(s, @"\s+", "");
        return Pitches.FirstOrDefault(p =>
            p.ToString(PitchNotation.Scientific).Equals(s, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.Solfege).Equals(s, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.Helmholtz).Equals(s, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.English).Equals(s, StringComparison.OrdinalIgnoreCase) ||
            p.ToString(PitchNotation.Midi).Equals(s, StringComparison.OrdinalIgnoreCase));
    }

    public Pitch? GetPitch(Notes n, int classIndex)
        => classIndex>=0 && classIndex<=8 ?
        Pitches.FirstOrDefault(p => p.Note == Notes[(int)n] && p.Class == classIndex) : null;


    #endregion


    #region Scales

    public List<Scale> Scales { get; private set; }

    private void BuildScales()
    {
        Scales = new List<Scale>();

        //build C major scale
        var cMajor = new Scale(this);
        //  cMajor.Notes.AddRange( Notes. );

    }

    #endregion
}
