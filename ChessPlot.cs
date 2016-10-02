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
        PointF[] points = null;
        Color winColor = Color.AliceBlue;
        Color loseColor = Color.IndianRed;
        public ChessPlot()
        {
            InitializeComponent();
            canvas.Dock = DockStyle.Fill;
            canvas.Paint += new System.Windows.Forms.PaintEventHandler(Draw);
            plotPen = new Pen(Color.DodgerBlue, 2.0f);
            
            gridPen = new Pen(Color.DarkBlue, 0.2f);
            
            makePoints();
        }
        protected override void OnLoad(EventArgs e)
        {
             base.OnLoad(e);
        }
        void makePoints()
        {
            int npts = 1024;
            float midY = (canvas.Bottom - canvas.Top) * 0.5f;
            float a = 0.6f;
            float lambdaBox = (float) ((canvas.Right - canvas.Left) * 0.5f);
            float dX = (float)((canvas.Right - canvas.Left) / (float) npts);
            float k = (float) (2 * Math.PI / lambdaBox);
            points = new PointF[npts];
            for (int i = 0; i < npts; ++i)
            {
                float x = i * dX;
                points[i].X = (float) i;
                points[i].Y = midY + a * midY * (float) Math.Sin(6 * x * k);
            }
        }
        private void Draw(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            FillMode newFillMode = FillMode.Alternate;
            SolidBrush fillBrush = new SolidBrush(winColor);
            g.FillClosedCurve(fillBrush, points, newFillMode, 0);
          
            g.DrawClosedCurve(gridPen, points);
            g.DrawCurve(plotPen, points);
        }
    }
}
