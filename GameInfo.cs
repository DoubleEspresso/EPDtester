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
        public GameInfo(Position p, ChessBoard cb)
        {     
            InitializeComponent();
            this.pos = p;
            mvList.cb = cb;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
        private delegate bool DisplayMoveFunc();
        public bool UpdateGameMoves()
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayMoveFunc(UpdateGameMoves));
                return true;
            }
            mvList.setGameText(pos.Game);
            highlightMove(pos.ToMove() ^ 1, pos.Game.MoveIndex());
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
            if (idx < 1) return true;
            return mvList.highlightMove(c, idx);
        }
    }
}
