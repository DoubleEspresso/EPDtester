using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace epdTester
{
    public partial class GL : UserControl
    {
        public GL()
        {
            InitializeComponent();
        }
        private const string OPENGL = "opengl32.dll";
        private const string GLU = "glu32.dll";
        private const string USER32 = "user32.dll";

        public const int POINTS = 0x0;
        public const int LINES = 0x1;
        public const int LINE_LOOP = 0x2;
        public const int LINE_STRIP = 0x3;
        public const int TRIANGLES = 0x4;
        public const int TRIANGLE_STRIP = 0x5;
        public const int TRIANGLE_FAN = 0x6;
        public const int QUADS = 0x7;
        public const int QUAD_STRIP = 0x8;
        public const int POLYGON = 0x9;

        /*DLL import definitions*/
        [DllImport(OPENGL, EntryPoint = "glTexImage1D")]
        public static extern void TexImage1D(int target, int level, int internalformat, int width, int border, int format, int type, byte[] pixels);
        [DllImport(OPENGL, EntryPoint = "glTexImage2D")]
        public static extern void TexImage2D(int target, int level, int internalformat, int width, int height, int border, int format, int type, byte[] pixels);
        [DllImport(OPENGL, EntryPoint = "glGenTextures")]
        public static extern void GenTextures(int n, [Out] int[] textures);
        [DllImport(OPENGL, EntryPoint = "glBindTexture")]
        public static extern void BindTexture(int ntarget, int texture);
        [DllImport(OPENGL, EntryPoint = "glDeleteTextures")]
        public static extern void DeleteTextures(int n, ref int textures);
        [DllImport(OPENGL, EntryPoint = "glTexCoord1d")]
        public static extern void TexCoord1d(double s);
        [DllImport(OPENGL, EntryPoint = "glTexCoord1f")]
        public static extern void TexCoord1f(float s);
        [DllImport(OPENGL, EntryPoint = "glTexCoord2f")]
        public static extern void TexCoord2f(float s, float t);
        [DllImport(OPENGL, EntryPoint = "glTexCoord3f")]
        public static extern void TexCoord3f(float r, float s, float t);
        [DllImport(OPENGL, EntryPoint = "glCopyTexSubImage2D")]
        public static extern void CopyTexSubImage2D(int target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
        [DllImport(OPENGL, EntryPoint = "glGetTexImage")]
        public static extern void GetTexImage(int target, int level, int format, int type, byte[] buffer);
        [DllImport(OPENGL, EntryPoint = "glReadPixels")]
        public static extern void ReadPixels(int x, int y, int width, int height, int format, int type, IntPtr pixels);
        [DllImport(OPENGL, EntryPoint = "glReadBuffer")]
        public static extern void ReadBuffer(int mode);
        [DllImport(OPENGL, EntryPoint = "glClearColor")]
        public static extern void ClearColor(float red, float green, float blue, float alpha);
        [DllImport(OPENGL, EntryPoint = "glBlendFunc")]
        public static extern void BlendFunc(uint sfactor, uint dfactor);
        [DllImport(OPENGL, EntryPoint = "glBlendColor")]
        public static extern void BlendColor(float red, float green, float blue, float alpha);
        [DllImport(OPENGL, EntryPoint = "glHint")]
        public static extern void Hint(uint ntarget, uint mode);
        [DllImport(OPENGL, EntryPoint = "glClearDepth")]
        public static extern void ClearDepth(double depth);
        [DllImport(OPENGL, EntryPoint = "glClear")]
        public static extern void Clear(uint mask);
        [DllImport(OPENGL, EntryPoint = "glEnable")]
        public static extern void Enable(int cap);
        [DllImport(OPENGL, EntryPoint = "glDisable")]
        public static extern void Disable(int cap);[DllImport(OPENGL, EntryPoint = "glColor3ub")]
        public static extern void Color3ub(byte red, byte green, byte blue);
        [DllImport(OPENGL, EntryPoint = "glColor3f")]
        public static extern void Color3f(float red, float green, float blue);
        [DllImport(OPENGL, EntryPoint = "glColor3d")]
        public static extern void Color3d(double red, double green, double blue);
        public static void Color(Color c) { Color3ub(c.R, c.G, c.B); }
        public static void Color(Color c, byte alpha) { Color4ub(c.R, c.G, c.B, alpha); }
        [DllImport(OPENGL, EntryPoint = "glColor4ub")]
        public static extern void Color4ub(byte red, byte green, byte blue, byte alpha);
        [DllImport(OPENGL, EntryPoint = "glColor4f")]
        public static extern void Color4f(float red, float green, float blue, float alpha);
        [DllImport(OPENGL, EntryPoint = "glVertex2i")]
        public static extern void Vertex2i(int x, int y);
        [DllImport(OPENGL, EntryPoint = "glVertex2f")]
        public static extern void Vertex2f(float x, float y);
        public static void Vertex(PointF p)
        {
            Vertex2f(p.X, p.Y);
        }
        [DllImport(OPENGL, EntryPoint = "glVertex2d")]
        public static extern void Vertex2d(double x, double y);
        public static void Vertex(Vec2 p)
        {
            Vertex2d(p.x, p.y);
        }
        [DllImport(OPENGL, EntryPoint = "glVertex3f")]
        public static extern void Vertex3f(float x, float y, float z);
        [DllImport(OPENGL, EntryPoint = "glViewport")]
        public static extern void Viewport(int x, int y, int width, int height);
        [DllImport(OPENGL, EntryPoint = "glMatrixMode")]
        public static extern void MatrixMode(uint mode);
        [DllImport(OPENGL, EntryPoint = "glPopMatrix")]
        public static extern void PopMatrix();
        [DllImport(OPENGL, EntryPoint = "glLoadIdentity")]
        public static extern void LoadIdentity();
        [DllImport(OPENGL, EntryPoint = "glPushMatrix")]
        public static extern void PushMatrix();
        [DllImport(OPENGL, EntryPoint = "glPushAttrib")]
        public static extern void PushAttrib(int mask);
        [DllImport(OPENGL, EntryPoint = "glPopAttrib")]
        public static extern void PopAttrib();
        [DllImport(OPENGL, EntryPoint = "glScalef")]
        public static extern void Scalef(float x, float y, float z);
        [DllImport(OPENGL, EntryPoint = "glScaled")]
        public static extern void Scaled(double x, double y, double z);
        public static void Scale(double s) { Scaled(s, s, s); }
        [DllImport(OPENGL, EntryPoint = "glTranslatef")]
        public static extern void Translatef(float x, float y, float z);
        [DllImport(OPENGL, EntryPoint = "glTranslated")]
        public static extern void Translated(double x, double y, double z);
        [DllImport(OPENGL, EntryPoint = "glRotatef")]
        public static extern void Rotatef(float angle, float x, float y, float z);
        [DllImport(OPENGL, EntryPoint = "glRotated")]
        public static extern void Rotated(double angle, double x, double y, double z);

        [DllImport(OPENGL, EntryPoint = "glLoadMatrixd")]
        public static extern void LoadMatrixd(double[] m);
        [DllImport(OPENGL, EntryPoint = "glMultMatrixd")]
        public static extern void MultMatrixd(double[] m);

        [DllImport(OPENGL, EntryPoint = "glBegin")]
        public static extern void Begin(uint mode);
        [DllImport(OPENGL, EntryPoint = "glEnd")]
        public static extern void End();
    }
}
