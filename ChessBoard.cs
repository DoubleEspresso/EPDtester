﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public class ChessBoard
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
        Engine ActiveEngine = null; // only 1 for now
        InteractionData id = new InteractionData();
        List<List<List<GL.Texture>>> PieceTextures = null;
        List<GL.Texture> squares = null;
        public Position pos = null;
        public enum Mode { ANALYSIS, GAME }
        public Mode mode = Mode.GAME;
        Point MousePos = new Point(0, 0);
        GL boardPane = null;
        GL evalPane = null;
        int[] AnalysisMove = new int[] { 0, 0 };
        BackgroundWorker eval_worker = new BackgroundWorker();
        mainWindow mw = null;
        public Engine ChessEngine { get { return ActiveEngine; } }
        bool mouseRightClick = false;
        bool mouseLeftClick = false;
        int startDragLoc = -1;
        bool doSquareHighlight = false;

        public ChessBoard(Engine e, GL boardPane, GL evalPane, mainWindow mw)
        {
            Initialize();
            ActiveEngine = e;
            if (ActiveEngine != null) ActiveEngine.SetBestMoveCallback(onEngineBestMoveParsed);
            // fixme
            if (gi == null) gi = new GameInfo(pos, this);
            if (this.boardPane == null) this.boardPane = boardPane;
            boardPane.PaintGL += Render;
            boardPane.MouseWheel += OnMouse_scroll;
            boardPane.MouseDown += OnMouse_click;
            boardPane.MouseUp += OnMouse_Up;
            boardPane.MouseMove += OnMouse_move;
            boardPane.KeyDown += OnKey_Down;
            boardPane.PreviewKeyDown += PreviewKeyDown;
            this.mw = mw;

            this.evalPane = evalPane;
            evalPane.PaintGL += DrawEval;
        }
        void Initialize()
        {
            // todo : handle other extensions for pieces/squares (only gif/png are supported now).
            string squaredir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\graphics\boards\custom"));
            string piecedir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\graphics\pieces\custom"));
            string[,] texpiece = new string[2, 6]
            {
                //{ "wp.svg", "wn.svg", "wb.svg", "wr.svg", "wq.svg", "wk.svg" },
                //{ "bp.svg", "bn.svg", "bb.svg", "br.svg", "bq.svg", "bk.svg" }
                { "wp.png", "wn.png", "wb.png", "wr.png", "wq.png", "wk.png" },
                { "bp.png", "bn.png", "bb.png", "br.png", "bq.png", "bk.png" }
            };
            string[] texsquare = new string[2] { "light.png", "dark.png" };

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
            //SetAspectRatio(Width, Height);

            eval_worker = new BackgroundWorker();
            eval_worker.DoWork += new DoWorkEventHandler(eval_renderWork);
            eval_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(eval_renderFinished);
            eval_worker.WorkerReportsProgress = false;
            eval_worker.WorkerSupportsCancellation = false;

            eval_worker.RunWorkerAsync();
        }
        public bool hasEngine() { return ActiveEngine != null; }
        public void CloseEngine() { if (hasEngine()) ActiveEngine.Close(); }
        public void SetEngine(Engine e)
        {
            if (e == ActiveEngine) return;
            if (hasEngine())
            {
                ActiveEngine.Close();
                ActiveEngine = null;
            }
            ActiveEngine = e;
            ActiveEngine.chessBoard = this;
        }

        float prev_eval = 0f;
        float eval = 0f;
        float Eval
        {
            get
            {
                return eval;
            }
            set
            {
                float tmp = (value < -4f ? 0 : value > 4f ? 1.0f : value / 8.0f + 0.5f); // map to [0, 1] range
                eval = -boardPane.Height * 0.5f + boardPane.Height * tmp; // map to [-height/2, height/2]
            }
        }
        List<float> EvalVertices
        {
            get
            {
                List<float> res = new List<float>();
                float target = Eval;

                // abort if the eval change is < 10% of the width of the GL graphic
                float percent_change = (target - prev_eval) / boardPane.Height * 100f;
                if (Math.Abs(percent_change) <= 0.5) return res;

                float curr_pos = 0;
                float dist_left = target - curr_pos;
                float total_dist = dist_left;
                float dx = 12f;

                int j = 1;
                while (Math.Abs(dist_left) > 1.1f * dx && j < 250)
                {
                    curr_pos += (total_dist < 0 ? -dx : dx);
                    dist_left = target - curr_pos;
                    res.Add(curr_pos);
                    ++j;
                }
                prev_eval = target;
                return res;
            }
        }
        bool updating = false;
        float y = 0;
        AutoResetEvent newEvalAvail = new AutoResetEvent(false);
        void eval_renderWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                newEvalAvail.WaitOne();
                updating = true;

                List<float> vertices = new List<float>(EvalVertices);
                foreach (float v in vertices)
                {
                    y = v;
                    evalPane.SafeInvalidate(true);
                    boardPane.SafeInvalidate(true);
                }
                Thread.Sleep(300); // wait here 
                newEvalAvail.Reset();
                updating = false;
            }
        }
        void eval_renderFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteLine("..closing eval update thread");
        }
        public void updateEval(float eval, int [] bestmove = null)
        {
            if (!updating)
            {
                AnalysisMove = bestmove;
                boardPane.SafeInvalidate(true);
                Eval = eval;
                newEvalAvail.Set();
            } 
        }
        void DrawEval()
        {
            GL.MatrixMode(GL.PROJECTION);
            GL.LoadIdentity();
            GL.Ortho(0, evalPane.Width, boardPane.Height, 0, 0, 1);
            GL.MatrixMode(GL.MODELVIEW);
            GL.Viewport(0, 0, evalPane.Width, boardPane.Height);
            GL.Clear(GL.DEPTH_BUFFER_BIT | GL.COLOR_BUFFER_BIT);
            GL.LoadIdentity();
            //GL.ClearColor(0f, 0f, 0f, 1f);

            // Enable blending
            GL.Enable(GL.BLEND);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            // draw background
            GL.Color3f(0.6f, 0.6f, 0.6f);
            float w = evalPane.Width;
            float h = 0.5f * boardPane.Height;

            GL.Begin(GL.QUADS);
            GL.Vertex2f(0, w/2f);
            GL.Vertex2f(evalPane.Width, w/2f);
            GL.Vertex2f(evalPane.Width, boardPane.Height-w/2f);
            GL.Vertex2f(0, boardPane.Height-w/2f);
            GL.End();

            // rounded cornerssss
            float[] color = new float[4] { 0.6f, 0.6f, 0.6f, 1f };
            render_rounded_corner(new Vec2(w / 2f, w / 2f), w / 2f, false, color); // top   
           
            render_rounded_corner(new Vec2(w / 2f, 2 * h - w / 2f), w / 2f, true, color); // bottom

            {
                GL.Begin(GL.QUADS);
                GL.ClearColor(1f, 1f, 1f, 1f);
                GL.Color4f(0.2f, 0.2f, 0.2f, 0.5f);

                GL.Vertex2f(0, h);

                GL.Color4f((y < 0f ? 0.01f + Math.Abs(y / 100.0f) : 0), (y >= 0 ? 0.01f + Math.Abs(y / 100.0f) : 0), 0.01f, 0.5f);

                GL.Vertex2f(0, h + y);

                GL.Color4f((y < 0f ? 0.01f + Math.Abs(y / 100.0f) : 0), (y >= 0 ? 0.01f + Math.Abs(y / 100.0f) : 0), 0.01f, 0.5f);

                GL.Vertex2f(w, h + y);
                GL.Vertex2f(w, h);

                GL.End();
            }
            if (y != 0)
            {
                color = new float[4] { (y < 0f ? 0.01f + Math.Abs(y / 100.0f) : 0), (y >= 0 ? 0.01f + Math.Abs(y / 100.0f) : 0), 0.01f, 0.5f };
                float d = (y < 0) ? h + y + 1f : h + y;
                render_rounded_corner(new Vec2(w / 2f, d), w / 2f, (y > 0), color); // top
            }
            GL.Disable(GL.BLEND);
        }

        Vec2[] rounded_cap(Vec2 center, float radius, bool bot, int pts)
        {
            Vec2[] res = new Vec2[pts];
            float dtheta = (bot ? 1.0f : -1.0f) * (float) Math.PI / (float) pts;

            for(int j=0; j< pts; ++j)
            {
                res[j] = new Vec2(radius * Math.Cos(j * dtheta), radius * Math.Sin(j * dtheta));
                res[j].x += center.x;
                res[j].y += center.y;
            }
            return res;
        }

        void render_rounded_corner(Vec2 center, float radius, bool bot, float[] color)
        {
            int nb_points = 200;
            Vec2[] points = rounded_cap(center, radius, bot, nb_points);

            GL.Begin(GL.TRIANGLE_FAN);
            for (int j = 0; j < nb_points; ++j)
            {
                GL.Color4f(color[0], color[1], color[2], color[3]);
                GL.Vertex3f((float)points[j].x, (float)points[j].y, 0f);
            }
            GL.End();
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
        public void SetDims(int w, int h)
        {
            boardPane.Width = w-4;
            boardPane.Height = h;
            evalPane.Width = (int) Math.Max(12, 0.01f * boardPane.Width)-2;
            evalPane.Height = h;
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
            GLGraphics.renderPreviousSquareHighlights();
            if (doSquareHighlight)
            {
                int r = 7 - (int)(startDragLoc >> 3);
                int c = (int)(startDragLoc & 7);
                GLGraphics.highlightSquare(new Vec2(oX, oY), new Vec2(dX, dY), r, c, 0);
                GLGraphics.storeSquare(new Vec2(oX, oY), new Vec2(dX, dY), r, c, 0);
                GLGraphics.renderPreviousSquareHighlights();
            }

            // render pieces after all squares are rendered .. else dragging pieces sometimes renders squares over the dragging piece
            for (int r = 0; r < 8; ++r)
            {
                for (int c = 0; c < 8; ++c)
                {
                    //if (draggedPiece(r, c)) continue;
                    renderPiece(dX, dY, r, c);
                }
            }

            GLGraphics.renderPreviousArrows();
            if (mouseRightClick)
            {
                int r = 7-(int)(startDragLoc >> 3);
                int c = (int)(startDragLoc & 7);
                Vec2 start = new Vec2((oX + c * dX) + 0.58 * dX, (oY + r * dY) + 0.5 * dY);
                int s2 = SquareFromMouseLoc(MousePos);
                if (startDragLoc != s2)
                {
                    r = 7-(int)(s2 >> 3);
                    c = (int)(s2 & 7);
                    if (r > 7 || r < 0) r = (r > 7 ? 7 : 0); // keep it on the board
                    if (c > 7 || c < 0) c = (c > 7 ? 7 : 0);
                    Vec2 end = new Vec2((oX + c * dX) + 0.58 * dX, (oY + r * dY) + 0.5 * dY);
                    GLGraphics.storeArrowData(start, end, (float)(0.17 * dX));
                    GLGraphics.renderArrow(start, end, (float)(0.17 * dX));
                }

            }

            if (AnalysisMove != null && AnalysisMove[0] != AnalysisMove[1] && !mouseRightClick && !mouseLeftClick)
            {
                int fr = 7 - (int)(AnalysisMove[0] >> 3);
                int fc = (int)(AnalysisMove[0] & 7);
                Vec2 start = new Vec2((oX + fc * dX) + 0.58 * dX, (oY + fr * dY) + 0.5 * dY);

                int tr = 7 - (int)(AnalysisMove[1] >> 3);
                int tc = (int)(AnalysisMove[1] & 7);
                Vec2 end = new Vec2((oX + tc * dX) + 0.58 * dX, (oY + tr * dY) + 0.5 * dY);
                //GLGraphics.storeArrowData(start, end, (float)(0.17 * dX));
                GLGraphics.renderArrow(start, end, (float)(0.17 * dX));
                //AnalysisMove = null;
            }

            // render dragging piece last, so it renders *over* all other textures.
            renderDraggingPiece(dX, dY);
        }
        private void renderPiece(double x, double y, int r, int c)
        {
            int s = 8 * r + c;
            float oX = 0;// (float)((Width - 8 * x) / 2f);
            float oY = 0; // (float)((Height - 8 * y) / 2f);

            if (!pos.Empty(s))
            {
                GL.Texture t = null;
                int color = pos.ColorOn(s); int p = pos.PieceOn(s);
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
                    mouseRightClick = true;
                    doSquareHighlight = false;
                    startDragLoc = SquareFromMouseLoc(e.Location);
                    break;
                case MouseButtons.Left:
                    mouseRightClick = false;
                    mouseLeftClick = true;
                    doSquareHighlight = false;
                    startDragLoc = -1;
                    GLGraphics.clearArrows();
                    GLGraphics.clearSquares();
                    int s = SquareFromMouseLoc(e.Location);
                    if (pos.Empty(s)) { id.reset(); return; }
                    id.from = s; id.color = pos.ColorOn(s); id.piecetype = pos.PieceOn(s);
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
            int s = SquareFromMouseLoc(e.Location);
            
            if (e.Button == MouseButtons.Right)
            { 
                mouseRightClick = false;
                if (s == startDragLoc)
                {
                    doSquareHighlight = true;
                    boardPane.SafeInvalidate(true);
                }
                else
                {
                    GLGraphics.storeArrow(GLGraphics.getStoredStart(), GLGraphics.getStoredEnd(), GLGraphics.getStoredWidth());
                }
                return;
            }

            if (!id.valid())
            {
                mouseLeftClick = false;
                return; // not dragging any piece (?)
            }

            if (!pos.doMove(id.from, s, id.piecetype, id.color))
            {
                id.ActivePiece = null; // for UI updates (piece will "snap" back to place).
                return; // note : invalidate called in glpane.cs
            }
            if (pos.isPromotion())
            {
                // note : doMove has moved the pawn forward and taken care of all captures (if any)
                PromotionUI p = new PromotionUI();
                p.setButtonImages(id.color, pos);
                p.ShowDialog(); // pause execution and continue only when closed.
                if (p.selectedPiece < 0)
                {
                    // todo: (not working for captures) undo the promotion, user closed the dialog without making a selection.
                    pos.undoPromotion(id.from, s, id.piecetype, id.color);
                    id.ActivePiece = null; // for UI updates (piece will "snap" back to place).
                    return;
                }
                else pos.doPromotion(id.from, s, id.piecetype, id.color, p.selectedPiece);
                // todo: tosan() for promotions not working.
            }
            pos.UpdatePosition();
            UpdateGameMoves(); // needs to be after position info is updated.

            // check mate/stalemate
            string game_over = "";
            if (pos.isMate()) game_over += "mate";
            else if (pos.isStaleMate()) game_over += "stalemate";
            if (!String.IsNullOrWhiteSpace(game_over)) Log.WriteLine("..game finished, {0}", game_over);

            // send move data to engine
            if (ActiveEngine != null)
            {
                switch (mode)
                {
                    case Mode.GAME:
                        ActiveEngine.Command("position fen " + pos.toFen()); // hack for now
                        ActiveEngine.Command("go wtime 3000 btime 3000"); // hack for now
                        break;
                    case Mode.ANALYSIS:
                        if (ActiveEngine.Thinking)
                        {
                            ActiveEngine.Command("stop");
                            ActiveEngine.Command("position fen " + pos.toFen());
                            ActiveEngine.Command("go infinite");
                        }
                        break;
                    default: break;
                }
            }
            mw.lookupPosition();
            id.reset();
            mouseLeftClick = false;
        }
        public bool UpdateGameMoves()
        {
            try
            {
                if (mw == null) return false;
                mw.MoveList.setGameText(pos.Game);
                mw.MoveList.highlightMove(pos.ToMove() ^ 1, pos.Game.MoveIndex());
            }
            catch (Exception ex)
            {
                Log.WriteLine("..failed to update move list: {0}", ex.Message);
                return false;
            }
            return true;
        }

        public void onEngineBestMoveParsed(object sender, EventArgs e)
        {
            if (ActiveEngine == null) return;
            ChessParser cp = ActiveEngine.Parser;
            if (cp == null) return;
            string bestmove = cp.SearchBestMove();
            if (bestmove == "") return;
            int[] fromto = Position.FromTo(cp.SearchBestMove());
            if (fromto == null)
            {
                Log.WriteLine("..ERROR failed to parse bestmove ({0})", bestmove);
                return;
            }
            int from = fromto[0]; int to = fromto[1];
            int piecetype = pos.PieceOn(from); int color = pos.ColorOn(from);
            if (!pos.doMove(from, to, piecetype, color))
            {
                Log.WriteLine("..[chessboard] ERROR : engine made illegal move!");
                boardPane.SafeInvalidate(true);
                return;
            }
            if (pos.isPromotion())
            {
                // todo:
                MessageBox.Show("..uh oh, promotion, time to crash.");
                return;
            }
            // history tracking (for UI back/forward buttons/scrolling through moves)
            pos.UpdatePosition();
            gi.UpdateGameMoves(); // after position update

            string game_over = "";
            if (pos.isMate()) game_over += "mate";
            else if (pos.isStaleMate()) game_over += "stalemate";
            //else if (pos2.isRepetitionDraw()) game_over += "3-fold repetition";

            if (!String.IsNullOrWhiteSpace(game_over))
            {
                Log.WriteLine("..game finished, {0}", game_over); // todo : sometimes invalid mates returned.
                // todo : engame callback
            }
            id.reset();
            boardPane.SafeInvalidate(true); // move the piece
        }
        private void OnMouse_move(object sender, MouseEventArgs e)
        {
            MousePos = e.Location; // member variable even needed?
        }
        private int scroll_req = 0;
        private void OnMouse_scroll(object sender, MouseEventArgs e)
        {
            if (scroll_req != 0)
            {
                Log.WriteLine("  !!DBG ignored scroll req nb({0})", scroll_req);
                return;
            }
            Interlocked.Increment(ref scroll_req);
            if (e.Delta > 0) setPreviousBoard();
            else setFutureBoard();
            Interlocked.Decrement(ref scroll_req);
        }
        /* Key input on chess board*/
        private void OnKey_Down(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                // fixme
                //if (gi == null) gi = new GameInfo(pos, this);
                gi.Show();
                gi.BringToFront();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (ActiveEngine != null) ActiveEngine.Command("stop");

                string pgn_text = Clipboard.GetText();
                if (!string.IsNullOrWhiteSpace(pgn_text))
                {
                    if (!valid_pgn(pgn_text))
                    {
                        Log.WriteLine("..ERROR: failed to set game from pgn string");
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                if (ActiveEngine != null && ActiveEngine.Thinking)
                {
                    ActiveEngine.Command("stop");
                }
                Log.WriteLine("..clearing board");
                pos = new Position(Position.StartFen);
                pos.UpdatePosition();
                mw.MoveList.ClearText();
                RefreshBoard();
                if (ActiveEngine != null && ActiveEngine.Thinking)
                {
                    ActiveEngine.Command("go infinite");
                }
            }
        }
        // utility to load a position from pgn text
        bool valid_pgn(string pgn)
        {
            if (string.IsNullOrWhiteSpace(pgn)) return false;
            // clean the string .. removing "\r\n" and "\n" chars --> " "
            pgn = pgn.Replace("\r\n", " "); pgn = pgn.Replace("\n", " ");

            string[] moves = pgn.Split(' ');
            if (moves.Length <= 0) return false;
            List<string> valid_moves = new List<string>();

            pos = new Position(Position.StartFen);

            int pgn_mv = 0; bool white_move = false;
            int valid_counter = 0;
            for (int j = 0; j < moves.Length; ++j)
            {
                string move = moves[j];
                if (int.TryParse(move.Trim().Replace(".", ""), out pgn_mv)) { white_move = true; continue; }
                else
                {
                    move = move.Trim().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");

                    int color = (white_move ? 0 : 1);
                    if (!pos.doSanMove(color, move))
                    {
                        Log.WriteLine("..[pgn] failed to parse {0}", move);
                        break;
                    }
                    pos.UpdatePosition();
                    UpdateGameMoves();
                    white_move = false;
                    ++valid_counter;
                    RefreshBoard();
                }
            }
            return true;
        }
        // left-right keys switch tab-display by default .. so catch their events 
        // in preview-key-down handlers
        public void PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (ActiveEngine != null) ActiveEngine.Command("stop");
            if (e.KeyCode == Keys.Down) { setPreviousBoard(3); e.IsInputKey = true; }
            else if (e.KeyCode == Keys.Up) { setFutureBoard(3); e.IsInputKey = true; }
            else if (e.KeyCode == Keys.Right) { setFutureBoard(); e.IsInputKey = true; }
            else if (e.KeyCode == Keys.Left) { setPreviousBoard(); e.IsInputKey = true; }
        }
        public void setPreviousBoard(int nb_moves = 1)
        {
            if (!pos.setPositionFromDisplay(-nb_moves)) return;
            if (mw != null) mw.MoveList.highlightMove(pos.ToMove() ^ 1, pos.Game.MoveIndex());
            GLGraphics.clearArrows();
            GLGraphics.clearSquares();
            RefreshBoard();
        }
        public void setFutureBoard(int nb_moves = 1)
        {
            if (!pos.setPositionFromDisplay(nb_moves)) return;
            if (mw != null) mw.MoveList.highlightMove(pos.ToMove() ^ 1, pos.Game.MoveIndex());
            GLGraphics.clearArrows();
            GLGraphics.clearSquares();
            RefreshBoard();
        }
        public void RefreshBoard()
        {
            mouseLeftClick = false;
            mouseRightClick = false;
            startDragLoc = -1;
            boardPane.SafeInvalidate(true);
            if (ActiveEngine != null && mode == Mode.ANALYSIS)
            {
                gi.ClearAnalysisPane();
                ActiveEngine.Command("stop");
                Thread.Sleep(100);
                ActiveEngine.Command("position fen " + pos.toFen());
                Thread.Sleep(100);
                ActiveEngine.Command("go infinite");
                mw.lookupPosition();
            }
        }
        public int CurrentMoveIdx()
        {
            return pos.Game.MoveIndex();
        }
        public void UpdateAnalysisGraph(List<double> evals)
        {
            gi.setPlotValues(evals);
        }
    }
}
