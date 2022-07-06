namespace WinTranspose
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSong = new System.Windows.Forms.TextBox();
            this.txtTransposedSong = new System.Windows.Forms.TextBox();
            this.radioButtonNormal = new System.Windows.Forms.RadioButton();
            this.radioButtonSharps = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonFlats = new System.Windows.Forms.RadioButton();
            this.txtSongPath = new System.Windows.Forms.TextBox();
            this.browseSongDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(25, 24);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(60, 23);
            this.numericUpDown1.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(25, 72);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSong);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtTransposedSong);
            this.splitContainer1.Size = new System.Drawing.Size(891, 467);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 3;
            // 
            // txtSong
            // 
            this.txtSong.AllowDrop = true;
            this.txtSong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSong.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSong.Location = new System.Drawing.Point(0, 0);
            this.txtSong.Multiline = true;
            this.txtSong.Name = "txtSong";
            this.txtSong.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSong.Size = new System.Drawing.Size(420, 467);
            this.txtSong.TabIndex = 1;
            this.txtSong.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBox1_DragDrop);
            this.txtSong.DragOver += new System.Windows.Forms.DragEventHandler(this.textBox1_DragOver);
            // 
            // txtTransposedSong
            // 
            this.txtTransposedSong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTransposedSong.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtTransposedSong.Location = new System.Drawing.Point(0, 0);
            this.txtTransposedSong.Multiline = true;
            this.txtTransposedSong.Name = "txtTransposedSong";
            this.txtTransposedSong.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTransposedSong.Size = new System.Drawing.Size(467, 467);
            this.txtTransposedSong.TabIndex = 2;
            // 
            // radioButtonNormal
            // 
            this.radioButtonNormal.AutoSize = true;
            this.radioButtonNormal.Checked = true;
            this.radioButtonNormal.Location = new System.Drawing.Point(18, 38);
            this.radioButtonNormal.Name = "radioButtonNormal";
            this.radioButtonNormal.Size = new System.Drawing.Size(65, 19);
            this.radioButtonNormal.TabIndex = 4;
            this.radioButtonNormal.TabStop = true;
            this.radioButtonNormal.Text = "Normal";
            this.radioButtonNormal.UseVisualStyleBackColor = true;
            this.radioButtonNormal.CheckedChanged += new System.EventHandler(this.radioButtonNormal_CheckedChanged);
            // 
            // radioButtonSharps
            // 
            this.radioButtonSharps.AutoSize = true;
            this.radioButtonSharps.Location = new System.Drawing.Point(18, 73);
            this.radioButtonSharps.Name = "radioButtonSharps";
            this.radioButtonSharps.Size = new System.Drawing.Size(92, 19);
            this.radioButtonSharps.TabIndex = 4;
            this.radioButtonSharps.Text = "Force Sharps";
            this.radioButtonSharps.UseVisualStyleBackColor = true;
            this.radioButtonSharps.CheckedChanged += new System.EventHandler(this.radioButtonSharps_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonNormal);
            this.groupBox1.Controls.Add(this.radioButtonFlats);
            this.groupBox1.Controls.Add(this.radioButtonSharps);
            this.groupBox1.Location = new System.Drawing.Point(931, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 187);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // radioButtonFlats
            // 
            this.radioButtonFlats.AutoSize = true;
            this.radioButtonFlats.Location = new System.Drawing.Point(18, 111);
            this.radioButtonFlats.Name = "radioButtonFlats";
            this.radioButtonFlats.Size = new System.Drawing.Size(81, 19);
            this.radioButtonFlats.TabIndex = 4;
            this.radioButtonFlats.Text = "Force Flats";
            this.radioButtonFlats.UseVisualStyleBackColor = true;
            this.radioButtonFlats.CheckedChanged += new System.EventHandler(this.radioButtonFlats_CheckedChanged);
            // 
            // txtSongPath
            // 
            this.txtSongPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSongPath.Location = new System.Drawing.Point(184, 24);
            this.txtSongPath.Name = "txtSongPath";
            this.txtSongPath.Size = new System.Drawing.Size(651, 23);
            this.txtSongPath.TabIndex = 6;
            // 
            // browseSongDialog
            // 
            this.browseSongDialog.DefaultExt = "txt";
            this.browseSongDialog.Filter = "Text files|*.txt|All files|*.*";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(841, 22);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 551);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSongPath);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NumericUpDown numericUpDown1;
        private SplitContainer splitContainer1;
        private TextBox txtSong;
        private TextBox txtTransposedSong;
        private RadioButton radioButtonNormal;
        private RadioButton radioButtonSharps;
        private GroupBox groupBox1;
        private RadioButton radioButtonFlats;
        private TextBox txtSongPath;
        private OpenFileDialog browseSongDialog;
        private Button btnBrowse;
    }
}