using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Jazz2TAS
{
    [XmlType]
    public class InputSequence
    {
        [XmlElement]
        public string Name { get; set; }
        [XmlElement]
        public int Length { get; set; }
        [XmlElement]
        public int Repeats { get; set; }
        [XmlArray]
        public List<Inputs> Inputs { get; set; }

        public InputSequence()
        {
            Repeats = 1;
            Inputs = new List<Inputs>();
        }

        public short[] GetInputs()
        {
            short[] output = new short[Length * Repeats];

            var orderedInputs = Inputs
                .OrderBy(x => x.Frame)
                .ToArray();

            for (int i = 0; i < orderedInputs.Length; i++)
            {
                int nextInputsFrame = i + 1 < orderedInputs.Length ? orderedInputs[i + 1].Frame : Length;
                int frame = orderedInputs[i].Frame;
                foreach (var inputs in orderedInputs[i].GetInputs(nextInputsFrame))
                {
                    output[frame++] |= inputs;
                }
            }

            for (int i = 1; i < Repeats; i++)
                Array.Copy(output, 0, output, Length * i, Length);

            return output;
        }

        public int GetCalculatedHash()
        {
            int hash = 0;
            hash += Length << 16;
            hash += Repeats;

            foreach (var inputs in Inputs)
                hash += inputs.GetCalculatedHash();

            return hash;
        }
    }
}
