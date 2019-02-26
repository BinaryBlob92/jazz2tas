using System;
using System.Windows.Forms;

namespace Jazz2TAS
{
    public partial class InsertForm : Form
    {
        private int _Insert;

        public int Insert => _Insert;

        public InsertForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxInsert.Text, out _Insert))
            {
                if (int.Parse(textBoxInsert.Text) > 0) {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Please specify a number of rows greater than zero.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(textBoxInsert.Text + " can't be interpreted as an integer.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
