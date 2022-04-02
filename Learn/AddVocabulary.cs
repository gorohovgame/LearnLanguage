using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Learn
{
    public partial class gorm2 : Form
    {
        public string Name => fName.Text.Trim();
        public string Description => fDescription.Text.Trim();
        public string Language => fLanguage.Text.Trim();
        public string FileName => fFile.Text.Trim();
        public gorm2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Name == String.Empty || Description == String.Empty || Language == String.Empty  || !File.Exists(FileName))
            {
                MessageBox.Show("All fields are not filled!",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog();

        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse CSV Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fFile.Text = openFileDialog1.FileName;

            }
        }

        private void fFile_MouseClick(object sender, MouseEventArgs e)
        {
            if (fFile.Text == "")
                OpenFileDialog();
        }
    }
}
