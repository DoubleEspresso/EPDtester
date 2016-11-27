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
    public partial class ChessBoard : Form
    {
        List<List<GL.Texture>> w_pieces = null;
        //    List<List<GL.Texture>> b_pieces = null;
        List<GL.Texture> squares = null;
        Position position = null;
        public ChessBoard()
        {
            InitializeComponent();
            Initialize();
            boardPane.PaintGL += Render;
        }
        void Initialize()
        {
            // todo : fail if textures fail to load.
            if (squares == null) squares = new List<GL.Texture>();
            squares.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\boards\\light.gif"));
            squares.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\boards\\dark.gif"));

            // pawns
            w_pieces = new List<List<GL.Texture>>();
            List<GL.Texture> pawns = new List<GL.Texture>();
            for (int i = 0; i < 8; ++i) pawns.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wp.png"));
            w_pieces.Add(pawns);

            // knights
            List<GL.Texture> knights = new List<GL.Texture>();
            for (int i = 0; i < 8; ++i) knights.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wn.png"));
            w_pieces.Add(knights);

            // bishops
            List<GL.Texture> bishops = new List<GL.Texture>();
            for (int i = 0; i < 8; ++i) bishops.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wb.png"));
            w_pieces.Add(bishops);

            // rooks
            List<GL.Texture> rooks = new List<GL.Texture>();
            for (int i = 0; i < 8; ++i) rooks.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wr.png"));
            w_pieces.Add(rooks);

            // queens
            List<GL.Texture> queens = new List<GL.Texture>();
            for (int i = 0; i < 8; ++i) queens.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wq.png"));
            w_pieces.Add(queens);

            // king
            List<GL.Texture> king = new List<GL.Texture>();
            king.Add(new GL.Texture("A:\\code\\chess\\testing\\epd\\vs2012\\epdTester\\epdTester\\graphics\\pieces\\wk.png"));
            w_pieces.Add(king);

            // the chess position (default is starting position)
            if (position == null) position = new Position(Position.StartFen);

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

            //renderDraggingPiece(dX, dY); // render dragging piece last, so it render *over* all other textures.

        }
        private void renderPiece(double x, double y, int r, int c)
        {
            int s = 8 * r + c;
            float oX = 0;// (float)((Width - 8 * x) / 2f);
            float oY = 0; // (float)((Height - 8 * y) / 2f);

            if (!position.isEmpty(s))
            {
                GL.Texture t = null;
                if (position.pieceColorAt(s) == Position.WHITE)
                {
                    List<int> wsquares = position.PieceSquares(Position.WHITE, position.PieceOn(s));
                    for (int j = 0; j < wsquares.Count; ++j)  if (s == wsquares[j]) t = w_pieces[position.PieceOn(s)][j];
                    if (t != null) t.Bind();
                }
                else if (position.pieceColorAt(s) == Position.BLACK)
                {
                    //List<int> bsquares = position.PieceSquares(Position.BLACK, position.PieceOn(s));
                    //for (int j = 0; j < bsquares.Count; ++j) if (s == bsquares[j]) t = b_pieces[position.PieceOn(s)][j];
                    //if (t != null) t.Bind();
                }
                if (t != null)
                {
                    r = 7 - r; // flip rows when rendering
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
    }
}
