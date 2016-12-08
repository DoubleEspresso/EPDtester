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
        Position2 pos = null;
        public GameInfo(Position2 p)
        {     
            InitializeComponent();
            this.pos = p;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
        private delegate bool DisplayMoveFunc(int c);
        public bool AddMoveToUI(int c)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayMoveFunc(AddMoveToUI), new object[] { c });
                return true;
            }
            // note : two cases to consider
            // 1. we have added a variation to an existing position
            // 2. this is a new move
            string san_mv = "";
            if (!pos.Game.hasChildren())
            {
                san_mv = pos.Game.SanMove();
            }
            else
            {
                MessageBox.Show("..uh oh! time to crash.");
                return false;
            }
            // todo : indicate on the UI if this is to be an additional variation
            // .. maybe italics/bold(?)
            int idx = pos.Game.MoveIndex();
            mvList.appendMove(san_mv, c, idx);
            mvList.highlightMove(c, idx);
            return true;
        }
        private delegate bool HighlightMoveFunc(int c, int idx);
        public bool highlightMove(int c, int idx)
        {
            if (InvokeRequired)
            {
                Invoke(new HighlightMoveFunc(highlightMove), new object[] { c, idx });
                return true;
            }
            if (idx <= -1) return true;
            return mvList.highlightMove(c, idx);
        }
    }
}
