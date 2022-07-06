using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Theory;

public class Scale
{
    private readonly MusicFactory _factory;

    public Scale(MusicFactory factory)
    {
        _factory = factory;
    }

    public List<Note> Notes { get; internal set; }

    public List<Note> Pitches { get; internal set; }


}
