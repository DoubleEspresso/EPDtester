using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class LogViewer : Form
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        public class LogTextBox : RichTextBox
        {
            System.Windows.Forms.Timer refresh = null;
            long last_displayed = 0;
            private int WM_SETFOCUS = 0x0007;
            private UInt32 EM_SETSEL = 0x00B1;
            int selCurrent = 0;
            int selOrigin = 0;
            int selStart = 0;
            int selEnd = 0;
            int selTrough = 0;
            int selPeak = 0;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

            public bool clicked = false;
            string filename = null;
        }

        private void openLogFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Log.DirectoryName);
            }
            catch (Exception) { }
        }
    }
}
