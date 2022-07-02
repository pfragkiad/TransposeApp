using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Transpose;

public class ChordTransposer
{
    ScalesConfiguration? allScales;

    public ChordTransposer()
    {
        allScales = ScalesConfiguration.ReadFromFile();
    }

    const string chordPattern = @"\[(?<scale>[A-G](b|#)?(m|dim|maj|min|sus|aug)?\d{0,2})\]";
    const string fullChordPattern = @"\b(?<note>[A-G])(?<accidentals>(b|#)?)(?<chord>(m|dim|maj|min|sus|aug)?)(?<number>\d{0,2})\b";

    public void Transpose(string fileIn, string fileOut, int transpose)
    {
        string[] lines = File.ReadAllLines(fileIn);

        int iLine = 0;

        transpose = mymod(transpose, 12);
        int mymod(int n, int m) => n - (int)Math.Floor((decimal)n / m) * m;

        Scale? oldScale = null, newScale = null;
        HashSet<string> allChords = new HashSet<string>();

        using (StreamWriter writer = new StreamWriter(fileOut))
        {

            foreach (string line in lines)
            {
                iLine++;
                if (iLine == 1)
                {
                    //read scale
                    var m = Regex.Match(line, chordPattern);
                    if (m.Success)
                    {
                        string title = line.Substring(0, line.IndexOf('[') - 1).Trim();
                        Console.WriteLine($"Title: {title}");

                        string scale = m.Groups["scale"].Value;

                        oldScale = allScales.Scales.FirstOrDefault(s => s.Name.Equals(scale, StringComparison.OrdinalIgnoreCase));
                        if (oldScale is not null)
                        {
                            int oldScaleIndex = -1;
                            oldScaleIndex = allScales.Scales.IndexOf(oldScale);
                            newScale = allScales.Scales[mymod(oldScaleIndex + transpose,12)];
                        }

                        Console.WriteLine($"Scale: {scale}->{newScale.Name}");
                    }
                }

                var matches = Regex.Matches(line, fullChordPattern);
                if (matches.Any())
                {
                    List<(string OldChord, string NewChord, int Index, int Length)> lineChords = new();

                    foreach (Match m in matches)
                    {
                        string oldChord = m.Value;
                        allChords.Add(oldChord);

                        string oldNote = m.Groups["note"].Value;
                        int oldNoteIndex = oldScale.Chords.IndexOf(oldNote);

                        Scale? currentScale = newScale;

                        if(oldNoteIndex==-1)
                        {
                            foreach(var scale in allScales.Scales) //search for scale 
                            {
                                if(scale.Chords.Contains(oldNote))
                                {
                                    oldNoteIndex = scale.Chords.IndexOf(oldNote);
                                    currentScale = allScales.Scales[mymod(allScales.Scales.IndexOf(scale)+transpose,12)];

                                    break;
                                }
                            }
                        }

                        string newNote = currentScale.Chords[oldNoteIndex];
                        string newChord = $"{newNote}{m.Groups["accidentals"].Value}{m.Groups["chord"].Value}{m.Groups["number"].Value}";

                        Console.WriteLine($"  {oldChord}->{newChord} matched @ line {iLine}, position {m.Index}");

                        lineChords.Add((oldChord, newChord, m.Index, m.Length));
                    }

                    string newLine = line;
                    for (int i = lineChords.Count - 1; i >= 0; i--)
                    //foreach(var c in lineChords)
                    {
                        var c = lineChords[i];
                        newLine = newLine.Remove(c.Index, c.Length).Insert(c.Index, c.NewChord);
                        if (iLine == 1) continue; //we do not remove spaces at the title

                        int chordLengthDifference = c.NewChord.Length - c.OldChord.Length;
                        if (chordLengthDifference < 0)//add spaces to avoid altering the other chords positions
                            newLine = newLine.Insert(c.Index + c.NewChord.Length, new string(' ', -chordLengthDifference));
                        else //remove spaces (only if it does not collide with other chord!)
                            if(c.Index + c.NewChord.Length < newLine.Length 
                            //&& 
                            //newLine.Substring(c.Index+c.NewChord.Length,chordLengthDifference).Count(c=>c==' ') 
                            //== newLine.Substring(c.Index + c.NewChord.Length, chordLengthDifference).Length
                            )

                            newLine = newLine.Remove(c.Index + c.NewChord.Length, chordLengthDifference);
                    }
                    writer.WriteLine(newLine);
                }
                else
                {
                    Console.WriteLine($"Line #{iLine} does not contain chords");
                    writer.WriteLine(line); //just copy the line to the output

                }

                //string[] tokens = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                //bool matches = false;
                //foreach (string token in tokens)
                //{
                //    (bool localMatches, string oldChord, string newChord) = ProcessChord(oldScale, newScale, allChords, token);
                //    matches = matches || localMatches;
                //    if (localMatches)
                //        Console.WriteLine($"  {oldChord}->{newChord} matched @ line {iLine}");
                //}
                //if (!matches)
                //{
                //    Console.WriteLine($"Line #{iLine} does not contain chords");
                //    writer.WriteLine(line); //just copy the line to the output
                //}
            }
        }

        //Console.WriteLine("Captured chords:");
        //foreach (string chord in allChords.OrderBy(c => c))
        //{
        //    (bool localMatches, string oldChord, string newChord) = ProcessChord(oldScale, newScale, allChords, chord);
        //    Console.WriteLine($"{chord}->{newChord}");
        //}

    }


    private static (bool, string, string) ProcessChord(Scale? oldScale, Scale? newScale, HashSet<string> allChords, string token)
    {
        var m = Regex.Match(token, fullChordPattern);
        if (m.Success)
        {
            string oldChord = m.Value;
            allChords.Add(oldChord);

            string oldNote = m.Groups["note"].Value;
            int oldNoteIndex = oldScale.Chords.IndexOf(oldNote);
            string newNote = newScale.Chords[oldNoteIndex];
            string newChord = $"{newNote}{m.Groups["accidentals"].Value}{m.Groups["chord"].Value}{m.Groups["number"].Value}";

            return (true, oldChord, newChord);
        }

        return (false, "", "");
    }
}
