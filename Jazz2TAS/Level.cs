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

        public byte[] GetJazz2InputsBytes()
        {
            byte[] output = new byte[0xFFFF];
            int index = 0;
            int previousInputs = 0;
            foreach (var inputs in Inputs.OrderBy(x => x.Frame))
            {
                while (index / 2 < inputs.Frame)
                {
                    output[index++] = (byte)previousInputs;
                    output[index++] = (byte)(previousInputs >> 8);
                }
                previousInputs = inputs.ToJazz2Inputs();
            }

            while (index + 1 < output.Length)
            {
                output[index++] = (byte)previousInputs;
                output[index++] = (byte)(previousInputs >> 8);
            }
            return output;
        }
    }
}
