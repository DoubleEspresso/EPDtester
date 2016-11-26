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
        //    List<List<GL.Texture>> w_pieces = null;
        //    List<List<GL.Texture>> b_pieces = null;
        List<GL.Texture> squares = null;

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
            //for (int r = 0; r < 8; ++r)
            //{
            //    for (int c = 0; c < 8; ++c)
            //    {
            //        r = 7 - r;
            //        if (draggedPiece(r, c)) continue;
            //        renderPiece(dX, dY, r, c);
            //    }
            //}


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
    }
}
