using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class ChessBoard : Form
    {
        public class InteractionData
        {
            // moving piece variables
            public int from = -1;
            public int color = -1;
            public int piecetype = -1;
            public GL.Texture ActivePiece = null;
            public void reset()
            {
                from = -1; color = -1; piecetype = -1;
                ActivePiece = null;
            }
            public bool valid()
            {
                return ((from >= 0 && from < 64) &&
                       (color == 0 || color == 1) &&
                       (piecetype >= 0 && piecetype < 6) &&
                       ActivePiece != null);
            }
        }
        GameInfo gi = null;
        InteractionData id = new InteractionData();
        List<List<List<GL.Texture>>> PieceTextures = null;
        List<GL.Texture> squares = null;
        Position pos = null;
        Point MousePos = new Point(0, 0);
        public ChessBoard()
        {
            InitializeComponent();
            Initialize();
            boardPane.PaintGL += Render;
            this.MouseWheel += new MouseEventHandler(OnMouse_scroll);
        }
        void Initialize()
        {
            // todo : handle other extensions for pieces/squares (only gif/png are supported now).
            string squaredir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\graphics\boards"));
            string piecedir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\graphics\pieces"));
            string[,] texpiece = new string[2, 6]
            {
                { "wp.png", "wn.png", "wb.png", "wr.png", "wq.png", "wk.png" },
                { "bp.png", "bn.png", "bb.png", "br.png", "bq.png", "bk.png" }
            };
            string[] texsquare = new string[2] { "light.gif", "dark.gif" };

            // square textures
            if (squares == null) squares = new List<GL.Texture>();
            squares.Add(new GL.Texture(Path.Combine(squaredir, texsquare[0])));
            squares.Add(new GL.Texture(Path.Combine(squaredir, texsquare[1])));
            // piece textures
            PieceTextures = new List<List<List<GL.Texture>>>(); // [color][piece][idx]
            PieceTextures.Add(new List<List<GL.Texture>>()); // white textures
            PieceTextures.Add(new List<List<GL.Texture>>()); // black textures
            for (int c = 0; c < 2; ++c)
            {
                for (int p = 0; p < 6; ++p)
                {
                    List<GL.Texture> textures = new List<GL.Texture>();
                    for (int i = 0; i < 8; ++i) textures.Add(new GL.Texture(Path.Combine(piecedir, texpiece[c, p])));
                    PieceTextures[c].Add(textures);
                }
            }

            // the chess position (default is starting position)
            if (pos == null) pos = new Position(Position.StartFen);

            SetAspectRatio(Width, Height);
        }
        void Render()
        {
            GL.MatrixMode(GL.PROJECTION);
            GL.LoadIdentity();
            GL.Ortho(0, boardPane.Width, boardPane.Height, 0, 0, 1);
            GL.MatrixMode(GL.MODELVIEW);
            GL.Viewport(0, 0, boardPane.Width, boardPane.Height);
            GL.Clear(GL.DEPTH_BUFFER_BIT | GL.COLOR_BUFFER_BIT);
            GL.LoadIdentity();
            GL.ClearColor(0f, 0f, 0f, 1f);
            RenderBoard();
        }
        private void ResizeFinished(object sender, EventArgs e)
        {
            SetAspectRatio(Width, Height);
        }
        private void SetAspectRatio(int w, int h)
        {
            float mind = (float)Math.Min(w, h);
            float nw = (w >= mind ? mind : w);
            float nh = (h >= mind ? mind : h);
            Width = (int)nw; Height = (int)nh;
        }
        public void RenderBoard()
        {
            int w = boardPane.Width; int h = boardPane.Height;
            float dX = (float)w / 8f;
            float dY = (float)h / 8f;
            float oX = 0; // (this.Width - w) / 2f;
            float oY = 0; // (this.Height - h) / 2f;
            GL.Texture t;
            GL.Enable(GL.TEXTURE_2D);

            for (int r = 0; r < 8; ++r)
            {
                for (int c = 0; c < 8; ++c) // starts with A1 square
                {
                    if (r % 2 == 0)
                    {
                        t = (c % 2 == 0 ? squares[1] : squares[0]);
                    }
                    else t = (c % 2 == 0 ? squares[0] : squares[1]);
                    t.Bind();
                    r = 7 - r; // start from "white" row

                    GL.Begin(GL.QUADS);
                    GL.TexCoord2f(0, 0); GL.Vertex2d(oX + c * dX, oY + r * dY);
                    GL.TexCoord2f(1, 0); GL.Vertex2d(oX + (c + 1) * dX, oY + r * dY);
                    GL.TexCoord2f(1, 1); GL.Vertex2d(oX + (c + 1) * dX, oY + (r + 1) * dY);
                    GL.TexCoord2f(0, 1); GL.Vertex2d(oX + c * dX, oY + (r + 1) * dY);
                    GL.End();
                }
            }

            // right-click single square - highlighting
            //GLGraphics.renderPreviousSquareHighlights();
            //if (doSquareHighlight)
            //{
            //    Vec2 tmp = squareFromMouse(startDragPos);
            //    int r = (int)(7 - tmp.x);
            //    int c = (int)tmp.y;
            //    GLGraphics.highlightSquare(new Vec2(oX, oY), new Vec2(dX, dY), r, c, 0);
            //    GLGraphics.storeSquare(new Vec2(oX, oY), new Vec2(dX, dY), r, c, 0);
            //}

            // render pieces after all squares are rendered .. else dragging pieces sometimes renders squares over the dragging piece
            for (int r = 0; r < 8; ++r)
            {
                for (int c = 0; c < 8; ++c)
                {
                    //if (draggedPiece(r, c)) continue;
                    renderPiece(dX, dY, r, c);
                }
            }

            //GLGraphics.renderPreviousArrows();
            //if (mouseRightClick)
            //{
            //    Vec2 tmp = squareFromMouse(startDragPos);
            //    int r = (int)(7 - tmp.x);
            //    int c = (int)tmp.y;
            //    Vec2 start = new Vec2((oX + c * dX) + 0.63 * dX, (oY + r * dY) + 0.5 * dY);
            //    Vec2 tmp2 = squareFromMouse(MousePos);
            //    if (!tmp.equals(tmp2))
            //    {
            //        r = (int)(7 - tmp2.x);
            //        c = (int)tmp2.y;
            //        if (r > 7 || r < 0) r = (r > 7 ? 7 : 0); // keep it on the board
            //        if (c > 7 || c < 0) c = (c > 7 ? 7 : 0);
            //        Vec2 end = new Vec2((oX + c * dX) + 0.63 * dX, (oY + r * dY) + 0.5 * dY);
            //        GLGraphics.storeArrowData(start, end, (float)(0.25 * dX));
            //        GLGraphics.renderArrow(start, end, (float)(0.25 * dX));
            //    }

            //}
            // render dragging piece last, so it renders *over* all other textures.
            renderDraggingPiece(dX, dY);
        }
        private void renderPiece(double x, double y, int r, int c)
        {
            int s = 8 * r + c;
            float oX = 0;// (float)((Width - 8 * x) / 2f);
            float oY = 0; // (float)((Height - 8 * y) / 2f);

            if (!pos.isEmpty(s))
            {
                GL.Texture t = null;
                int color = pos.colorOn(s); int p = pos.PieceOn(s);
                List<int> sqs = pos.PieceSquares(color, p);
                for (int j = 0; j < sqs.Count; ++j) if (s == sqs[j]) { t = PieceTextures[color][p][j]; break; }

                if (t != null && t != id.ActivePiece)
                {
                    r = 7 - r; // flip rows when rendering
                    t.Bind();
                    GL.Enable(GL.BLEND); // blend to remove ugly piece background
                    GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
                    GL.Begin(GL.QUADS);
                    GL.TexCoord2f(0, 0); GL.Vertex2d(c * x + oX, r * y + oY);
                    GL.TexCoord2f(1, 0); GL.Vertex2d((c + 1) * x + oX, r * y + oY);
                    GL.TexCoord2f(1, 1); GL.Vertex2d((c + 1) * x + oX, (r + 1) * y + oY);
                    GL.TexCoord2f(0, 1); GL.Vertex2d(c * x + oX, (r + 1) * y + oY);
                    GL.Disable(GL.BLEND);
                    GL.End();
                }
            }
        }
        private void renderDraggingPiece(double x, double y)
        {
            if (!id.valid()) return;
            id.ActivePiece.Bind();
            GL.Enable(GL.BLEND); // blend to remove ugly piece background
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
            GL.Begin(GL.QUADS);
            GL.TexCoord2f(0, 0); GL.Vertex2d(MousePos.X - 0.5 * x, MousePos.Y - 0.5 * y);
            GL.TexCoord2f(1, 0); GL.Vertex2d(MousePos.X + 0.5 * x, MousePos.Y - 0.5 * y);
            GL.TexCoord2f(1, 1); GL.Vertex2d(MousePos.X + 0.5 * x, MousePos.Y + 0.5 * y);
            GL.TexCoord2f(0, 1); GL.Vertex2d(MousePos.X - 0.5 * x, MousePos.Y + 0.5 * y);
            GL.Disable(GL.BLEND);
            GL.End();
        }
        /* Mouse interactions - utilities */
        public int SquareFromMouseLoc(Point v)
        {
            float oX = 0; // (float)((getClientArea().width - BoardDims.x) / 2f);
            float oY = 0; // (float)((getClientArea().height - BoardDims.y) / 2f);
            float dX = (float)(boardPane.Width / 8f);//(float) r.width / 8f;
            float dY = (float)(boardPane.Height / 8f);//(float) r.height / 8f;
            int row = (int)Math.Floor((float)(v.Y - oY) / dY);
            int col = (int)Math.Floor((float)(v.X - oX) / dX);
            // flip row for display
            row = 7 - row;
            return (int)(8 * row + col);
        }
        private void OnMouse_click(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    id.reset();
                    break;
                case MouseButtons.Left:
                    int s = SquareFromMouseLoc(e.Location);
                    if (pos.isEmpty(s)) { id.reset(); return; }
                    id.from = s; id.color = pos.colorOn(s); id.piecetype = pos.PieceOn(s);
                    List<int> sqs = pos.PieceSquares(id.color, id.piecetype);
                    for (int j = 0; j < sqs.Count; ++j) if (s == sqs[j]) { id.ActivePiece = PieceTextures[id.color][id.piecetype][j]; break; }
                    break;
                default:
                    id.reset();
                    break;
            }
        }
        private void OnMouse_Up(object sender, MouseEventArgs e)
        {
            if (!id.valid()) return; // we aren't dragging any piece (but could be coloring the board).
            int s = SquareFromMouseLoc(e.Location);
            if (pos.isLegal(id.from, s, id.piecetype, id.color, true))
            {
                // drop piece..handle all special move types
                pos.doMove(id.from, s, id.piecetype, id.color);
                if (pos.isPromotion()) // TODO: fixme
                {
                    //popupPromotionWindow(fromSq, toSq, movingPiece, movingColor);
                    //fromSq = -1;
                    //toSq = -1;
                    //movingColor = -1;
                    //movingPiece = -1;
                    //draggingPiece = false;
                    //ActivePiece = null;
                    return;
                }
                // check mate/stalemate
                string game_over = "";
                if (pos.isMate(id.from, s, id.piecetype, id.color)) game_over += "mate";
                else if (pos.isStaleMate()) game_over += "stalemate";
                else if (pos.isRepetitionDraw()) game_over += "3-fold repetition";

                if (!String.IsNullOrWhiteSpace(game_over))
                {
                    Log.WriteLine("..game finished, {0}", game_over);
                    return;
                }
                // send move data to engine
                String fen = pos.getPosition(pos.maxDisplayedMoveIdx(), 0);
                //engineMonitor.sendCommand("position fen " + fen);
                //engineMonitor.sendCommand("go wtime 8000 btime 8000");
                //engine.UCI_CMD("position fen " + fen);
                //engine.UCI_CMD("go wtime 8000 btime 8000");
                //engine.startListening("bestmove");
            }
            pos.clearMoveData();
            id.reset();
        }
        private void OnMouse_move(object sender, MouseEventArgs e)
        {
            MousePos = e.Location; // member variable even needed?
        }
        private void OnMouse_scroll(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) setPreviousBoard();
            else setFutureBoard();
        }
        /* Key input on chess board*/
        private void OnKey_Down(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                if (gi == null) gi = new GameInfo();
                gi.Show();
                gi.BringToFront();
            }
        }
        // for those keys not handled by key-down events (navigation/tab/enter)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down) { setPreviousBoard(3); return true; }
            else if (keyData == Keys.Up) { setFutureBoard(3); return true; }
            else if (keyData == Keys.Right) { setFutureBoard(); return true; }
            else if (keyData == Keys.Left) { setPreviousBoard(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void setPreviousBoard(int nb_moves = 1)
        {
            int idx = pos.displayedMoveIdx();
            if ((idx - nb_moves) < 0) return;
            pos.setPositionFromFenStrings(idx - nb_moves, 0);
            boardPane.SafeInvalidate(true);
        }
        private void setFutureBoard(int nb_moves = 1)
        {
            int idx = pos.displayedMoveIdx();
            if ((idx + nb_moves) > pos.maxDisplayedMoveIdx()) return;
            pos.setPositionFromFenStrings(idx + nb_moves, 0);
            boardPane.SafeInvalidate(true);
        }
    }
}
