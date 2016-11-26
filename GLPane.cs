using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class GLPane : UserControl
    {
        public GLPane()
        {
            InitializeComponent();
        }

        // basic wrapper around gl.h
        public static class GL
        {
            public static int POINTS = 0x0;
            public static int LINES = 0x1;
            public static int LINE_LOOP = 0x2;
            public static int LINE_STRIP = 0x3;
            public static int TRIANGLES = 0x4;
            public static int TRIANGLE_STRIP = 0x5;
            public static int TRIANGLE_FAN = 0x6;
        }
    }
}
