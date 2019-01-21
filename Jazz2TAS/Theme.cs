using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Jazz2TAS
{
    [XmlType]
    public struct Theme
    {
        public static Theme DefaultTheme = new Theme()
        {
            BackgroundColor = "F0F0F0",
            TextColor = "000000",
            TableCurrentFrameBackgroundColor = "FF0000",
            TableTickBackgroundColor = "C0C0C0",
            TableGridColor = "A0A0A0",
            TableBackgroundColor = "FFFFFF",
            TableTextColor = "000000",
            TableSelectionBackgroundColor = "0078D7",
            TableSelectionTextColor = "FFFFFF",
            TableHeaderBackgroundColor = "F0F0F0",
            TableHeaderTextColor = "000000",
        };

        public static Theme DarkTheme = new Theme()
        {
            BackgroundColor = "202020",
            TextColor = "F0F0F0",
            TableCurrentFrameBackgroundColor = "800000",
            TableTickBackgroundColor = "404040",
            TableGridColor = "505050",
            TableBackgroundColor = "101010",
            TableTextColor = "F0F0F0",
            TableSelectionBackgroundColor = "303030",
            TableSelectionTextColor = "FFFFFF",
            TableHeaderBackgroundColor = "303030",
            TableHeaderTextColor = "F0F0F0",
        };

        [XmlElement]
        public string BackgroundColor { get; set; }
        [XmlElement]
        public string TextColor { get; set; }
        [XmlElement]
        public string TableCurrentFrameBackgroundColor { get; set; }
        [XmlElement]
        public string TableTickBackgroundColor { get; set; }
        [XmlElement]
        public string TableGridColor { get; set; }
        [XmlElement]
        public string TableBackgroundColor { get; set; }
        [XmlElement]
        public string TableTextColor { get; set; }
        [XmlElement]
        public string TableSelectionBackgroundColor { get; set; }
        [XmlElement]
        public string TableSelectionTextColor { get; set; }
        [XmlElement]
        public string TableHeaderBackgroundColor { get; set; }
        [XmlElement]
        public string TableHeaderTextColor { get; set; }

        public Color GetBackgroundColor() => Color.FromArgb(int.Parse(BackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTextColor() => Color.FromArgb(int.Parse(TextColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableCurrentFrameBackgroundColor() => Color.FromArgb(int.Parse(TableCurrentFrameBackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableTickBackgroundColor() => Color.FromArgb(int.Parse(TableTickBackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableGridColor() => Color.FromArgb(int.Parse(TableGridColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableBackgroundColor() => Color.FromArgb(int.Parse(TableBackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableTextColor() => Color.FromArgb(int.Parse(TableTextColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableSelectionBackgroundColor() => Color.FromArgb(int.Parse(TableSelectionBackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableSelectionTextColor() => Color.FromArgb(int.Parse(TableSelectionTextColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableHeaderBackgroundColor() => Color.FromArgb(int.Parse(TableHeaderBackgroundColor, NumberStyles.HexNumber) | (0xFF << 24));

        public Color GetTableHeaderTextColor() => Color.FromArgb(int.Parse(TableHeaderTextColor, NumberStyles.HexNumber) | (0xFF << 24));

        public void Save(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(Theme));
                serializer.Serialize(stream, this);
            }
        }
    }
}
