using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Jazz2TAS
{
    public partial class SequenceForm : Form
    {
        private InputSequence _InputSequence;

        public Inputs Inputs { get; set; }
        public InputSequence InputSequence
        {
            get
            {
                return _InputSequence;
            }
            set
            {
                _InputSequence = value;
                textBoxName.DataBindings.Clear();
                textBoxLength.DataBindings.Clear();
                textBoxRepeats.DataBindings.Clear();
                textBoxName.DataBindings.Add("Text", _InputSequence, "Name");
                textBoxLength.DataBindings.Add("Text", _InputSequence, "Length");
                textBoxRepeats.DataBindings.Add("Text", _InputSequence, "Repeats");
                dataGridViewInputs.DataSource = new BindingList<Inputs>(_InputSequence.Inputs);
            }
        }

        public SequenceForm(Inputs inputs)
        {
            InitializeComponent();
            dataGridViewInputs.AutoGenerateColumns = false;

            Inputs = inputs;
            InputSequence = Inputs.Sequence == null ? new InputSequence() : (InputSequence)Inputs.Sequence.Clone();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int maxFrame = _InputSequence.Inputs
                .Select(x => x.Frame)
                .DefaultIfEmpty()
                .Max();

            if (maxFrame >= _InputSequence.Length)
            {
                if (MessageBox.Show("Inputs are specified at frames greater than the length. Do you want to change the length to " + (maxFrame + 1) + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _InputSequence.Length = maxFrame + 1;
                }
                else
                {
                    return;
                }
            }

            Inputs.Sequence = _InputSequence;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Inputs.Sequence = null;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Input sequence|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!InputSequence.Save(dialog.FileName))
                        MessageBox.Show("Failed to save input sequence.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Input sequence|*.xml";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InputSequence inputSequence;
                    if (InputSequence.Load(dialog.FileName, out inputSequence))
                        InputSequence = inputSequence;
                    else
                        MessageBox.Show("Failed to load input sequence.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
