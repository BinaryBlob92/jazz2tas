using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Jazz2TAS
{
    [XmlType]
    public class Level
    {
        [XmlAttribute]
        public string LevelName { get; set; }
        [XmlArray]
        public List<Inputs> Inputs { get; set; }

        public Level()
        {
            Inputs = new List<Inputs>();
        }

        public short[] GetJazz2Inputs()
        {
            short[] output = new short[0x7FFF];

            var orderedInputs = Inputs
                .OrderBy(x => x.Frame)
                .ToArray();

            for (int i = 0; i < orderedInputs.Length; i++)
            {
                int nextInputsFrame = i + 1 < orderedInputs.Length ? orderedInputs[i + 1].Frame : output.Length;
                int frame = orderedInputs[i].Frame;
                foreach (var inputs in orderedInputs[i].GetInputs(nextInputsFrame))
                {
                    output[frame++] |= inputs;
                }
            }

            return output;
        }
    }
}
