

using Transpose;
//using Music.Core;
using System.Text;
using TransposeChordLibrary;
using Microsoft.Extensions.DependencyInjection;
using TransposeChordLibrary.Theory;

string songFile = @"C:\Users\north\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";
// @"C:\Users\PhD\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";

//ChordTransposer t = new ChordTransposer();
//t.Transpose(songFile, "kfetes.txt", 2);


//var note = MusicNotes.FromString("Cb"); //MusicNotes.ESharp;
//Console.OutputEncoding = Encoding.UTF8;
//Console.WriteLine(note);
var app = App.GetMusicApp();
var m = app.Services.GetService<MusicFactory>()!;

//var note = m.GetNote("C"); //Note.Parse("C");
var note = m.GetNote(NoteName.C);
Console.WriteLine(note.ToStringFull());
Console.WriteLine( (note+2)==m.GetNote("D"));


Console.WriteLine(m.GetNote("D") >= m.GetNote("do #")); //true
Console.WriteLine(m.GetNote("D") - m.GetNote("do #")); //1



//show enharmonics
//note.EnharmonicNotes.ToList().ForEach(n=> Console.WriteLine(n));

note++;
Console.WriteLine(note.ToStringFull());

note-=2; //note+=2
Console.WriteLine(note.ToStringFull());

Console.WriteLine( (note-1).ToString());

//XUNit!

Pitch p1 = m.GetPitch("la4"); //=A5!
Pitch p2 = m.GetPitch("C");
Console.WriteLine(p2.Frequency);

Console.WriteLine($"{p2} -> {p2 + 12}");
Console.WriteLine($"{p2} -> {p2 - 12}");
p2++;
Console.WriteLine($"{p2} -> {p2 + 12}");

var cmajor = m.GetMajorScale(NoteName.D);
Console.WriteLine(cmajor.ToString(false));

Console.WriteLine(m.GetInterval("C#", 5, IntervalQuality.Perfect));


