using System;
using System.Windows.Forms;

namespace Jazz2TAS
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm(args));
        }
    }
}
