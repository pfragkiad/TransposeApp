using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using Transpose;

namespace WinTranspose
{
    public partial class Form1 : Form
    {
        ChordTransposer _transposer;
        private readonly IConfiguration _configuration;


        //https://stackoverflow.com/questions/70475830/how-to-use-dependency-injection-in-winforms
        public Form1(
            ChordTransposer transposer,
            IConfiguration configuration)
        {
            InitializeComponent();

            var settings = Properties.Settings.Default;

            string songFile = string.IsNullOrWhiteSpace(settings.LastSongFile) || !File.Exists(settings.LastSongFile) ?
                @"C:\Users\north\OneDrive\songs\Καραφώτης Κώστας - Γίνε μαζί μου παιδί.txt" : settings.LastSongFile;
            
            txtSongPath.Text = songFile;
            txtSong.Text = File.ReadAllText(songFile);

            numericUpDown1.Value = settings.LastTranspose;
            numericUpDown1.ValueChanged += (o, e) => UpdateTransposedChords();

            _transposer = transposer;
            _configuration = configuration;
          
            UpdateTransposedChords();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var settings = Properties.Settings.Default;
            settings.LastSongFile = txtSongPath.Text;
            settings.LastTranspose = numericUpDown1.Value;
            settings.Save();
        }


        private void UpdateTransposedChords()
        {
            int transpose = (int)numericUpDown1.Value;
            string content = txtSong.Text;

            if (radioButtonFlats.Checked)
                txtTransposedSong.Text = _transposer.Transpose(content, transpose, TransposeSettings.ForceFlats);
            else if (radioButtonSharps.Checked)
                txtTransposedSong.Text = _transposer.Transpose(content, transpose, TransposeSettings.ForceSharps);
            else
                txtTransposedSong.Text = _transposer.Transpose(content, transpose, TransposeSettings.Normal);

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
                txtSongPath.Text = files[0];
                txtSong.Text = File.ReadAllText(files[0]);

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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            browseSongDialog.InitialDirectory = _configuration["baseSongPath"];
            var reply = browseSongDialog.ShowDialog();
            if (reply != DialogResult.OK) return;

            txtSongPath.Text = browseSongDialog.FileName;
            txtSong.Text = File.ReadAllText(browseSongDialog.FileName);

            UpdateTransposedChords();
        }
    }
}