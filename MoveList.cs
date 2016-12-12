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
    public partial class MoveList : UserControl
    {
        public ChessBoard cb = null;
        public MoveList()
        {
            InitializeComponent();
            mList.Clear();
        }
        public bool highlightMove(int c, int idx)
        {
            string idxString = Convert.ToString((int)Math.Floor((double)idx/2)) + ".";
            int sidx = mList.Text.IndexOf(idxString) + idxString.Length; int eidx = -1;
            if (sidx < 0) return false;
            int count = 0;
            while ((sidx + count < mList.Text.Length) && mList.Text[sidx + count] != ' ') ++count;
            if (c == 0) eidx = sidx + count;
            else
            {
                sidx += count + 1; // move 1 past the ' ' character
                count = 0;
                while ((sidx + count < mList.Text.Length) && mList.Text[sidx + count] != ' ') ++count;
                eidx = sidx + count;
            }
            if (sidx < 0 || sidx > mList.Text.Length) return false; // do not update the highlighted text
            if (eidx <= sidx || eidx > mList.Text.Length ) return false;
            mList.SelectionStart = sidx;
            mList.SelectionLength = eidx - sidx;
            return true;
        }
        public bool setGameText(Position.ChessGame g)
        {
            mList.Clear();
            mList.Text += g.Moves();
            return true;
        }
        private void OnMouse_click(object sender, MouseEventArgs e)
        {
            if (cb == null) return;
            int p = mList.GetCharIndexFromPosition(new Point(e.X, e.Y));
            switch (e.Button)
            {
                case MouseButtons.Right:
                    ContextMenu cm = new ContextMenu();
                    cm.MenuItems.Add("test mv (does nothing)", SelectVariation);
                    cm.Show(this, new Point(e.X, e.Y));
                    break;
                case MouseButtons.Left: // update to clicked move
                    // note : dbg hack to "set" position based on clicking the move list at a point
                    // needs debugging/smoothing, requires access to the chessgame/position classes.
                    int dst_idx = p;
                    if (dst_idx >= mList.Text.Length) return;
                    while (mList.Text[dst_idx] != ' ') ++dst_idx;
                    dst_idx = mList.Text.Substring(0, dst_idx).Count(f => f == ' ') + 1; // number of moves made to get to mouse position
                    int diff = dst_idx - cb.CurrentMoveIdx();
                    if (diff > 0) cb.setFutureBoard(diff);
                    else cb.setPreviousBoard(-diff);
                    //MessageBox.Show(string.Format("left-click @({0},{1}), number of moves to this point ({2})", e.X, e.Y, nb_mvs));
                    break;
                default:
                    break;
            }
        }
        private void SelectVariation(object sender, EventArgs args)
        {
            // event to select subvariation on right-click
        }
    }
}
