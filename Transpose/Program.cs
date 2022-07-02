

using Transpose;

string songFile = @"C:\Users\north\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";
// @"C:\Users\PhD\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";

ChordTransposer t = new ChordTransposer();
t.Transpose(songFile, "kfetes.txt", 2);

