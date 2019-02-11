using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Jazz2TAS
{
    public partial class SequenceForm : Form
    {
        private Inputs _Inputs;
        private BindingList<Inputs> _SequenceInputs;

        public SequenceForm(Inputs inputs)
        {
            InitializeComponent();

            _Inputs = inputs;
            _SequenceInputs = new BindingList<Inputs>();

            if (inputs.Sequence != null)
            {
                textBoxLength.Text = inputs.Sequence.Length.ToString();
                textBoxRepeats.Text = inputs.Sequence.Repeats.ToString();

                foreach (var sequenceInputs in inputs.Sequence.Inputs)
                    _SequenceInputs.Add(sequenceInputs);
            }

            dataGridViewInputs.AutoGenerateColumns = false;
            dataGridViewInputs.DataSource = _SequenceInputs;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxLength.Text, out int length))
            {
                MessageBox.Show("'" + textBoxLength.Text + "' can't be interpreted as a number.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(textBoxRepeats.Text, out int repeats))
            {
                MessageBox.Show("'" + textBoxRepeats.Text + "' can't be interpreted as a number.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int maxFrame = _SequenceInputs
                .Select(x => x.Frame)
                .DefaultIfEmpty()
                .Max();

            if (maxFrame >= length)
            {
                if (MessageBox.Show("Inputs are specified at frames greater than the length. Do you want to change the length to " + (maxFrame + 1) + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    length = maxFrame + 1;
                }
                else
                {
                    return;
                }
            }

            if (_Inputs.Sequence == null)
            {
                _Inputs.Sequence = new InputSequence();
                _Inputs.Sequence.Inputs.AddRange(_SequenceInputs);
                _Inputs.Sequence.Length = length;
                _Inputs.Sequence.Repeats = repeats;
            }
            else
            {
                _Inputs.Sequence.Inputs.Clear();
                _Inputs.Sequence.Inputs.AddRange(_SequenceInputs);
                _Inputs.Sequence.Length = length;
                _Inputs.Sequence.Repeats = repeats;
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _Inputs.Sequence = null;
            Close();
        }
    }
}
