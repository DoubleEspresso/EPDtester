using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class GameInfo : Form
    {
        Position pos = null;
        public GameInfo(Position pos)
        {
            InitializeComponent();
            this.pos = pos;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
        public bool displayMove(string m, int c, int idx)
        {
            return mvList.appendMove(m, c, idx); // just pass to child control
        }
    }
}
