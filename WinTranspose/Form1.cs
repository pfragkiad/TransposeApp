using Transpose;

namespace WinTranspose
{
    public partial class Form1 : Form
    {
        //https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms
        public Form1(ChordTransposer transposer)
        {
            InitializeComponent();

            string songFile = @"C:\Users\north\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";
            textBox1.Text = File.ReadAllText(songFile);

            _transposer = transposer;
        }

        ChordTransposer _transposer;



        private void UpdateTransposedChords()
        {
            int transpose = (int)numericUpDown1.Value;
            string content = textBox1.Text;

            if (radioButtonFlats.Checked)
                textBox2.Text = _transposer.Transpose(content, transpose, TransposeSettings.ForceFlats);
            else if (radioButtonSharps.Checked)
                textBox2.Text = _transposer.Transpose(content, transpose, TransposeSettings.ForceSharps);
            else
                textBox2.Text = _transposer.Transpose(content, transpose, TransposeSettings.Normal);

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

            UpdateTransposedChords();
        }
        private void radioButtonSharps_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonSharps.Checked) return;
            UpdateTransposedChords();

        }


        private void radioButtonNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonNormal.Checked) return;
            UpdateTransposedChords();


        }

        private void radioButtonFlats_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonFlats.Checked) return;
            UpdateTransposedChords();


        }
    }
}