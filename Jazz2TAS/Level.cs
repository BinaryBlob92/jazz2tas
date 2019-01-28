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

        public short[] GetJazz2InputsBytes()
        {
            short[] output = new short[0x7FFF];
            short index = 0;
            short previousInputs = 0;
            foreach (var inputs in Inputs.OrderBy(x => x.Frame))
            {
                while (index < inputs.Frame)
                    output[index++] = previousInputs;

                var newInputs = inputs.GetInputs();
                for (short i = 0; i<newInputs.Length; i++)
                {
                    previousInputs = newInputs[i];
                    output[(index + i)] |= previousInputs;
                }
            }

            while (index + 1 < output.Length)
            {
                output[index++] |= previousInputs;
            }
            return output;
        }
    }
}
