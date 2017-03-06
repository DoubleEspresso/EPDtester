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
        ChessBoard board = null;
        public GameInfo(Position p, ChessBoard cb)
        {     
            InitializeComponent();
            this.pos = p;
            mvList.cb = cb;
            board = cb;
            //chessPlot.setPoints(new List<double>() { 0 });
        }
        public void SetPosition(Position p) { this.pos = p; }
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
        public void setPlotValues(List<double> vals)
        {
            //chessPlot.setPoints(vals);
        }
        public void ClearAnalysisPane()
        {
            engineAnalysisControl1.Clear();
        }
        // fixme
        private void engineAnalysisItem_Click(object sender, EventArgs e)
        {
            // todo :: should "disable" the option if no engine present..
            //if (!board.hasEngine()) return;
            //board.mode = ChessBoard.Mode.ANALYSIS;

            //// note: take engine out of "game" mode so it does not try to stop
            //// on best-move parsing (causes illegal engine moves/crashes)
            //// todo : if we toggle this option, we need to re-set the callback.
            //board.ChessEngine.Parser.CallbackOnBestmove = null;
             
            //// update for "analysis control" game info view
            //engineAnalysisControl1.Initialize(board.ChessEngine, board);

            //// start analyzing right away
            //board.RefreshBoard();
        }
    }
}
