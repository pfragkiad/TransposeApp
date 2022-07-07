using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;


public enum ScaleType
{
    Major
}


public class Scale
{
    private readonly MusicFactory _factory;

    public Scale(MusicFactory factory)
    {
        _factory = factory;
    }

    public ScaleType ScaleType { get; set; }

    public List<Note> Notes { get; internal set; }

    public Scale Transpose(int semiTones)
    {
        var newScale = new Scale(_factory);
        newScale.ScaleType = ScaleType;
        newScale.Notes = Notes.Select(n => n += semiTones).ToList();
        newScale.UpdatePitches();
        return newScale;
    }

    public IReadOnlyList<Pitch> Pitches { get; internal set; }

    public void UpdatePitches() =>
      Pitches = _factory.Pitches.Where(p => Notes.Contains(p.Note)).ToList();


    public override string ToString() => ToString(false);

    public string ToString(bool useSolfege)
    {
        var allNoteNames = GetAllNoteNames(useSolfege);

        switch(allNoteNames.Count)
        {
            case 1:
                return string.Join(", ", allNoteNames[0]);
            case 2:
                return string.Join(", ", allNoteNames[0]) +
                    " ("  + string.Join(", ", allNoteNames[1]) + ")"   ;
            case 3:
                return string.Join(", ", allNoteNames[0]) +
                    " ("  + string.Join(", ", allNoteNames[1]) + ")" +
                     " (" + string.Join(", ", allNoteNames[2]) + ")";
            default:
                return "";
        }
    }

    /// <summary>
    /// Get one list of notenames per enharmonic note of the first note. Only valid sequences are returned.
    /// </summary>
    /// <param name="useSolfege"></param>
    /// <returns></returns>
    public List<List<string>> GetAllNoteNames(bool useSolfege = false)
    {

        Note startNote = Notes[0];

        var enharmonicNotes = startNote.GetEnharmonicNoteNames(useSolfege);

        List< List<string>> scaleNoteNames = new();

        foreach (var startNoteName in enharmonicNotes)
        {
            //if (startNoteName == "C#") Debugger.Break();

            List<string> scenarioNotes = new List<string>();
            string currentChordNote = startNoteName;
            scenarioNotes.Add(currentChordNote);

            for(int iNote=1;iNote<Notes.Count-1;iNote++)
            {
                currentChordNote = _factory.GetΝextIntervalNoteName(currentChordNote, Notes[iNote]);
                //ignore invalid chord sequences
                if (currentChordNote is null) goto NextEnharmonicNote;
                scenarioNotes.Add(currentChordNote);
            }
            scenarioNotes.Add(scenarioNotes[0]); //the octave note is always the same
            scaleNoteNames.Add(scenarioNotes);

        NextEnharmonicNote:;
        }

        return scaleNoteNames;
    }
}
