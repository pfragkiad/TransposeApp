using Transpose;

namespace WinTranspose
{
    public partial class Form1 : Form
    {
        ChordTransposer _transposer;


        //https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms
        public Form1(ChordTransposer transposer)
        {
            InitializeComponent();

            string songFile = @"C:\Users\north\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt";
            textBox1.Text = File.ReadAllText(songFile);

            _transposer = transposer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTransposedChords();
        }




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

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textBox1.Text = File.ReadAllText(files[0]);

                UpdateTransposedChords();

            }
        }

        private void textBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

  
    }
}