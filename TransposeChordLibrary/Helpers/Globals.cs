global using static TransposeChordLibrary.Helpers.Globals;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary.Helpers;

public static class Globals
{
    public static int MyMod(int n, int m) => n - (int)Math.Floor((decimal)n / m) * m;

    private static char[] accidentals = new char[] { '#', 'b' };

    public static string? StripAccidentals(string? s) => s?.TrimEnd(accidentals).Trim();


}
