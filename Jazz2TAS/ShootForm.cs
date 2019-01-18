using System;
using System.Windows.Forms;

namespace Jazz2TAS
{
    public partial class ShootForm : Form
    {
        private int _Shoot;

        public int Shoot => _Shoot;

        public ShootForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxShoot.Text, out _Shoot))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(textBoxShoot.Text + " can't be interpreted as an integer.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
