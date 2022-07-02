using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Transpose;

public enum TransposeSettings
{
    Normal,
    ForceFlats,
    ForceSharps
}

public class ChordTransposer
{
    ScalesConfiguration? allScales;

    public ChordTransposer()
    {
        allScales = ScalesConfiguration.ReadFromFile();
    }

    const string scalePattern = @"\[(?<note>[A-G](b|#)?)(?<minor>m?)\]";
    const string fullChordPattern = @"\b(?<note>[A-G])(?<accidentals>(b|#)?)(?<chord>(m|dim|maj|min|sus|aug)?)(?<number>\d{0,2})\b";

    public void Transpose(string fileIn, string fileOut, int transpose, TransposeSettings transposeSettings = TransposeSettings.Normal)
    {
        File.WriteAllText(
            fileOut,
            Transpose(File.ReadAllText(fileIn), transpose, transposeSettings));

    }


    public string Transpose(string content, int transpose, TransposeSettings transposeSettings = TransposeSettings.Normal)
    {
        string[] lines = content.Split("\r\n");// File.ReadAllLines(fileIn);

        int iLine = 0;

        transpose = MyMod(transpose, 12);

        Scale? oldMajorScale = null, newScale = null;
        bool scaleHasSharps = false;
        bool scaleHasFlats = false;
        HashSet<string> allChords = new HashSet<string>();

        StringBuilder writer = new StringBuilder();


        foreach (string line in lines)
        {
            iLine++;
            if (iLine == 1)
            {
                //read scale
                var m = Regex.Match(line, scalePattern);
                if (m.Success)
                {
                    string title = line.Substring(0, line.IndexOf('[') - 1).Trim();
                    Console.WriteLine($"Title: {title}");

                    string note = m.Groups["note"].Value;
                    string majorNote = note;

                    if (m.Groups["minor"].Value != "")
                        majorNote = allScales.TransposeNote(note, 3);
                    else majorNote = allScales.TransposeNote(note, 0); //get the corresponding note that exists in scales!

                    //that's the major scale in all cases
                    oldMajorScale = allScales.Scales.FirstOrDefault(s => s.Name.Equals(majorNote, StringComparison.OrdinalIgnoreCase));
                    if (oldMajorScale is not null)
                    {
                        int oldScaleIndex = -1;
                        oldScaleIndex = allScales.Scales.IndexOf(oldMajorScale);
                        newScale = allScales.Scales[MyMod(oldScaleIndex + transpose, 12)];

                        scaleHasSharps = newScale.Chords.Any(c => c.EndsWith("#"));
                        scaleHasFlats = newScale.Chords.Any(c => c.EndsWith("b"));
                    }

                    Console.WriteLine($"Scale: {note}{m.Groups["minor"].Value}->{allScales.TransposeNote(note, 3,transposeSettings)}{m.Groups["minor"].Value}");
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
                    int oldNoteIndex = oldMajorScale.Chords.IndexOf(oldNote);

                    Scale? currentScale = newScale;

                    if (oldNoteIndex == -1)
                    {
                        foreach (var scale in allScales.Scales) //search for scale 
                        {
                            if (scale.Chords.Contains(oldNote))
                            {
                                oldNoteIndex = scale.Chords.IndexOf(oldNote);
                                currentScale = allScales.Scales[MyMod(allScales.Scales.IndexOf(scale) + transpose, 12)];

                                break;
                            }
                        }
                    }

                    string newNote = currentScale.Chords[oldNoteIndex];
                    if (newNote.EndsWith("#") &&
                        (transposeSettings == TransposeSettings.ForceFlats
                        || transposeSettings == TransposeSettings.Normal && scaleHasFlats && !scaleHasSharps))
                    {
                        //get the flat enharmonic
                        var enharmonic = allScales.Enharmonics.FirstOrDefault(e => e[0] == newNote);
                        if (enharmonic is not null)
                            newNote = enharmonic[1];
                    }
                    else if (newNote.EndsWith("b") &&
                        (transposeSettings == TransposeSettings.ForceSharps
                        || transposeSettings == TransposeSettings.Normal && scaleHasSharps && !scaleHasFlats))
                    {
                        //get the sharp enharmonic
                        var enharmonic = allScales.Enharmonics.FirstOrDefault(e => e[1] == newNote);
                        if (enharmonic is not null)
                            newNote = enharmonic[0];
                    }

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
                        if (c.Index + c.NewChord.Length < newLine.Length
                        //&& 
                        //newLine.Substring(c.Index+c.NewChord.Length,chordLengthDifference).Count(c=>c==' ') 
                        //== newLine.Substring(c.Index + c.NewChord.Length, chordLengthDifference).Length
                        )

                        newLine = newLine.Remove(c.Index + c.NewChord.Length, chordLengthDifference);
                }
                writer.AppendLine(newLine);
            }
            else
            {
                Console.WriteLine($"Line #{iLine} does not contain chords");
                writer.AppendLine(line); //just copy the line to the output

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


        //Console.WriteLine("Captured chords:");
        //foreach (string chord in allChords.OrderBy(c => c))
        //{
        //    (bool localMatches, string oldChord, string newChord) = ProcessChord(oldScale, newScale, allChords, chord);
        //    Console.WriteLine($"{chord}->{newChord}");
        //}

        return writer.ToString();
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
