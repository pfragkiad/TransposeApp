using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Transpose;




string songFile = @"C:\Users\PhD\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";

ChordTransposer t = new ChordTransposer();
t.Transpose(songFile, "kfetes.txt", 2);

