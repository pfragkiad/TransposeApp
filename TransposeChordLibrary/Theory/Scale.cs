using System;
using System.Collections.Generic;
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


    public override string ToString() => ToString(false,true);


    public string ToString(bool useSolfege, bool preferSharps)
    {

        int countNonNaturalSharps = Notes.Where(n => !n.IsNatural && n.SharpName is not null).Count();
        int countNonNaturalFlats = Notes.Where(n => !n.IsNatural && n.FlatName is not null).Count();
        return ScaleType.ToString() + " " + string.Join(", ", Notes.Select(n => n.ToString(countNonNaturalSharps >= countNonNaturalFlats, useSolfege)));

    }
}
