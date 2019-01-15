using System;
using System.Windows.Forms;

namespace Jazz2TAS
{
    public partial class OffsetForm : Form
    {
        private int _Offset;

        public int Offset => _Offset;

        public OffsetForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxOffset.Text, out _Offset))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(textBoxOffset.Text + " can't be interpreted as an integer.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
