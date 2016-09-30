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
    public partial class ChessPlot : UserControl
    {
        Pen plotPen = null;
        Pen gridPen = null;
        PointF[] points = null;
        public ChessPlot()
        {
            InitializeComponent();
            canvas.Dock = DockStyle.Fill;
            canvas.Paint += new System.Windows.Forms.PaintEventHandler(Draw);
            plotPen = new Pen(Pens.LightBlue.Color, 2.5f);
            gridPen = new Pen(Pens.MediumBlue.Color, 0.2f);
            makePoints();
        }
        protected override void OnLoad(EventArgs e)
        {
             base.OnLoad(e);
        }
        void makePoints()
        {
            float midY = (canvas.Bottom - canvas.Top) * 0.5f;
            float a = 0.6f;
            float lambdaBox = (float) ((canvas.Right - canvas.Left) * 0.5f);
            float dX = (float)((canvas.Right - canvas.Left) / 1024.0);
            float k = (float) (2 * Math.PI / lambdaBox);
            points = new PointF[1024];
            for (int i = 0; i < 1024; ++i)
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
            g.DrawString("Current eval",  new Font("Arial", 10), System.Drawing.Brushes.Blue, new Point(1, 1));
            //g.DrawLine(plotPen, canvas.Left, canvas.Top, canvas.Right, canvas.Bottom);
            g.DrawBeziers(plotPen, points);
           
        }
    }
}
