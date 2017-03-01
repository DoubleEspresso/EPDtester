using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public class GLGraphics
    {
        // arrows
        private static List<Arrow> arrows = null;
        private static Arrow storedArrowData = new Arrow();

        // square highlights
        private static List<Square> squares = null;

        private class Arrow
        {
            public Arrow() { }
            public Arrow(Vec2 s, Vec2 e, float w) { start = s; end = e; width = w; }
            public Vec2 start;
            public Vec2 end;
            public float width;
        }

        private class Square
        {
            public Square() { }
            public Square(Vec2 o, Vec2 d, int r, int c, int type)
            {
                this.o = o; this.d = d; this.r = r; this.c = c; this.type = type;
            }
            public Vec2 o;
            public Vec2 d;
            public int r;
            public int c;
            public int type;
        }

        public static void storeArrowData(Vec2 startPos, Vec2 endPos, float w)
        {
            storedArrowData.width = w; storedArrowData.start = startPos; storedArrowData.end = endPos;
        }

        public static float getStoredWidth()
        {
            return storedArrowData.width;
        }

        public static Vec2 getStoredStart()
        {
            return storedArrowData.start;
        }

        public static Vec2 getStoredEnd()
        {
            return storedArrowData.end;
        }

        public static void storeSquare(Vec2 o, Vec2 d, int r, int c, int type)
        {
            if (squares == null)
            {
                squares = new List<Square>();
                squares.Clear();
            }
            squares.Add(new Square(o, d, r, c, type));
        }

        public static void storeArrow(Vec2 startPos, Vec2 endPos, float w)
        {
            if (arrows == null)
            {
                arrows = new List<Arrow>();
                arrows.Clear();
            }
            arrows.Add(new Arrow(startPos, endPos, w));
        }

        public static Boolean hasPreviousArrows() { return (arrows != null && arrows.Count > 0); }

        public static Boolean hasPreviousSquares() { return (squares != null && squares.Count > 0); }

        public static void clearArrows() { if (hasPreviousArrows()) arrows.Clear(); }

        public static void clearSquares() { if (hasPreviousSquares()) squares.Clear(); }

        public static void renderPreviousArrows()
        {
            if (!hasPreviousArrows()) return;

            for (int j = 0; j < arrows.Count; ++j)
            {
                renderArrow(arrows[j].start, arrows[j].end, arrows[j].width);
            }
        }

        public static void renderPreviousSquareHighlights()
        {
            if (!hasPreviousSquares()) return;

            for (int j = 0; j < squares.Count; ++j)
            {
                highlightSquare(squares[j].o, squares[j].d, squares[j].r, squares[j].c, squares[j].type);
            }
        }

        public static void highlightSquare(Vec2 o, Vec2 d, int r, int c, int type)
        {
            double oX = o.x; double oY = o.y;
            double dX = d.x; double dY = d.y;

            // 2 triangles to make square
            double x0 = oX + c * dX; double y0 = oY + r * dY;
            double x1 = oX + (c + 1) * dX; double y2 = oY + (r + 1) * dY;

            float[] colors = { 0f, 0f, 0f, 0f };

            switch (type)
            {
                case 0: // selection
                    colors = new float[] { 0.25f, 0.9f, 0.25f, 0.4f };
                    break;
                case 1: // check
                    colors = new float[] { 0.8f, 0.3f, 0.3f, 0.4f };
                    break;
                case 2: // capture (potential)
                    colors = new float[] { 0.8f, 0.5f, 0.5f, 0.4f };
                    break;
            }
            GL.Enable(GL.BLEND);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
            GL.Enable(GL.TEXTURE_2D);
            GL.Begin(GL.TRIANGLES);
            GL.Color4f(colors[0], colors[1], colors[2], colors[3]);
            GL.Vertex2d(x0, y0); GL.Vertex2d(x1, y0); GL.Vertex2d(x1, y2);
            GL.Vertex2d(x0, y0); GL.Vertex2d(x0, y2); GL.Vertex2d(x1, y2);
            GL.End();
            GL.Disable(GL.BLEND);
            GL.Color4f(1f, 1f, 1f, 1f);
        }

        public static void renderArrow(Vec2 startPos, Vec2 endPos, float w)
        {
            if (startPos == null || endPos == null) return;
            double dy = endPos.y - startPos.y;
            double dx = endPos.x - startPos.x;
            double theta = (dx < 0 ? Math.PI + Math.Atan(dy / dx) : Math.Atan(dy / dx));
            double len = Math.Sqrt(dy * dy + dx * dx);

            makeArrow(startPos, w, len, -Math.PI / 2 + theta);

        }

        public static void makeArrow(Vec2 start, double width, double height, double theta_rad)
        {
            // basic rectangle from 2 triangles
            // triangle 1	
            double x0 = start.x - width / 2; double y0 = start.y;
            double x1 = start.x + width / 2; double y1 = y0;
            double x2 = start.x - width / 2; double y2 = y0 + height;
            double yy2 = y2 - 1.5 * width;

            // make the base of the arrow (better to do this after arrowhead last..but lazy)
            GL.Enable(GL.BLEND);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
            GL.Disable(GL.TEXTURE_2D);
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Translatef((float)x0, (float)y0, 0f);
            GL.Rotatef((float)(theta_rad * 180 / Math.PI), 0f, 0f, 1f);
            GL.Translatef(-(float)start.x, -(float)start.y, 0f);

            GL.Begin(GL.TRIANGLES);
            GL.Color4f(0.2f, 0.2f, 0.7f, 0.7f);

            GL.Vertex2d(x0, y0); GL.Vertex2d(x1, y1); GL.Vertex2d(x2, yy2);
            GL.Vertex2d(x1, y0); GL.Vertex2d(x1, yy2); GL.Vertex2d(x0, yy2);

            GL.End();
            GL.PopMatrix();

            // arrowhead
            GL.PushMatrix();
            GL.Translatef((float)x0, (float)y0, 0f);
            GL.Rotatef((float)(theta_rad * 180 / Math.PI), 0f, 0f, 1f);
            GL.Translatef(-(float)start.x, (float)-start.y, 0f);
            GL.Begin(GL.TRIANGLES);
            GL.Vertex2d(x1 + 0.5 * width, yy2); GL.Vertex2d((x1 + x2) / 2f, y2); GL.Vertex2d(x2 - 0.5 * width, yy2);
            GL.End();
            render_rounded_corner(new Vec2((x1 + x2) / 2f, y1 + 0.15), (float)width / 2f, 6);
            GL.PopMatrix();

            GL.Disable(GL.BLEND);
            GL.Color4f(1f, 1f, 1f, 1f);
        }

        private static void render_rounded_corner(Vec2 center, float radius, int type)
        {
            int nb_points = 200; // default
            Vec2[] points = rounded_corner(center, radius, nb_points, type);

            // default color will be white (alpha = 0) ??
            GL.Begin(GL.TRIANGLE_FAN);
            for (int j = 0; j < nb_points; ++j)
            {
                GL.Vertex3f((float)points[j].x, (float)points[j].y, 0f);
            }
            GL.End();
        }

        private static Vec2[] rounded_corner(Vec2 center, float radius, int nb_points, int type)
        {

            float start_theta = 0; float end_theta = 90; //defaults

            switch (type)
            {
                case 1:
                    {
                        start_theta = 0;
                        end_theta = 90; // 1st quadrant of a circle;
                        break;
                    }
                case 2:
                    {
                        start_theta = 90;
                        end_theta = 180; // 2nd quadrant of a circle;
                        break;
                    }
                case 3:
                    {
                        start_theta = 180;
                        end_theta = 270; // 3rd quadrant of a circle;
                        break;
                    }
                case 4:
                    {
                        start_theta = 270;
                        end_theta = 360; // 4th quadrant of a circle;
                        break;
                    }
                case 5:
                    {
                        start_theta = 0; end_theta = 180; break;
                    }
                case 6:
                    {
                        start_theta = 180; end_theta = 360; break;
                    }
            }

            float dtheta = (end_theta - start_theta) / nb_points;
            Vec2[] points = new Vec2[nb_points + 1];
            points[0] = new Vec2(center.x, center.y);


            // compute the points, and return a list of them to openGL
            for (int j = 1; j < nb_points + 1; ++j)
            {
                double theta = (start_theta + j * dtheta) * Math.PI / 180.0;
                points[j] = new Vec2(0, 0);
                points[j].x = (float)(center.x + radius * Math.Cos(theta));
                points[j].y = (float)(center.y + radius * Math.Sin(theta));
            }

            return points;
        }
    }
}
