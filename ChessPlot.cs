using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace epdTester
{
    public partial class ChessPlot : UserControl
    {
        Pen plotPen = null;
        Pen gridPen = null;
        List<PointF> points = null;
        Color winColor = Color.AliceBlue;
        Color loseColor = Color.IndianRed;
        public ChessPlot()
        {
            InitializeComponent();
            canvas.Dock = DockStyle.Fill;
            canvas.Paint += new System.Windows.Forms.PaintEventHandler(Draw);
            plotPen = new Pen(Color.SkyBlue, 2.0f);            
            gridPen = new Pen(Color.PowderBlue, 0.2f);
        }
        protected override void OnLoad(EventArgs e)
        {
             base.OnLoad(e);
        }
        public void setPoints(List<double> yvals)
        {
            int npts = 1024; 
            if (yvals.Count <= 0) return;
            if (points == null) points = new List<PointF>();
            if (points.Count > 50) points.Clear();
            if (points.Count > npts) return;
            float midY = (canvas.Bottom - canvas.Top) * 0.5f;
            float dX = (float)((canvas.Right - canvas.Left) / (float)npts);
            for (int i = 0; i < yvals.Count; ++i)
            {
                if (points.Count > npts) return;
                float x = 20 * points.Count * dX;
                PointF p = new PointF((float)x, (float)(midY - 12 * yvals[i]));
                points.Add(p);
            }
            Refresh();
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            if (points == null) return;
            // Create a local version of the graphics object for the PictureBox.    
            Graphics g = e.Graphics;
            drawGrid(g, 4, 4);
            ControlPaint.DrawBorder(g, e.ClipRectangle, Color.DodgerBlue, ButtonBorderStyle.Solid);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //FillMode newFillMode = FillMode.Alternate;
            //SolidBrush fillBrush = new SolidBrush(winColor);
            if (points.Count > 10)
            {
                PointF[] data = new PointF[points.Count]; int i = 0;
                foreach (PointF p in points) { data[i] = new PointF(p.X, p.Y); ++i; }
                g.DrawCurve(plotPen, data);
                //g.FillClosedCurve(fillBrush, data, newFillMode, 0);
                //g.DrawClosedCurve(gridPen, data);
            }
            if (points.Count > 50) points.Clear();
        }
        private void drawGrid(Graphics g, float LinesX, float LinesY)
        {
            float dY = (canvas.Bottom - canvas.Top) / LinesY;
            float dX = (canvas.Right - canvas.Left) / LinesX;
            float x = canvas.Left; float y = canvas.Top;
            for (int i = 0; i < LinesX; ++i, x += dX, y += dY)
            {
                g.DrawLine(gridPen, new PointF(x, canvas.Bottom), new PointF(x, canvas.Top));
                g.DrawLine(gridPen, new PointF(canvas.Left, y), new PointF(canvas.Right, y));
            }
            float midY = (canvas.Bottom - canvas.Top) * 0.5f;
            g.DrawString("+3.0", Font, new SolidBrush(Color.Black), 1, midY - (float)1.3 * dY);
            g.DrawString("-3.0", Font, new SolidBrush(Color.Black), 1, midY + (float)1.1 * dY);
        }
    }
}
