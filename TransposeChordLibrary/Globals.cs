global using static TransposeChordLibrary.Globals;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransposeChordLibrary;

public static class Globals
{
    public static int MyMod(int n, int m) => n - (int)Math.Floor((decimal)n / m) * m;

}
