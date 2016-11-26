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
using System.Drawing.Imaging;

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

        public const int TEXTURE_1D = 0x0DE0;
        public const int TEXTURE_2D = 0x0DE1;
        public const int TEXTURE_3D = 0x806F;

        public const int TEXTURE_MAG_FILTER = 0x2800;
        public const int TEXTURE_MIN_FILTER = 0x2801;

        public const int NEAREST = 0x2600;
        public const int LINEAR = 0x2601;

        public const int TEXTURE_WRAP_R = 0x8072;
        public const int TEXTURE_WRAP_S = 0x2802;
        public const int TEXTURE_WRAP_T = 0x2803;
        public const int CLAMP = 0x2900;
        public const int CLAMP_TO_BORDER = 0x812D;
        public const int CLAMP_TO_EDGE = 0x812F;

        /*data types*/
        public const int BYTE = 0x1400;
        public const int UNSIGNED_BYTE = 0x1401;
        public const int SHORT = 0x1402;
        public const int UNSIGNED_SHORT = 0x1403;
        public const int FLOAT = 0x1406;
        public const int FIXED = 0x140C;

        // format of pixel data
        public const int COLOR_INDEX = 0x1900;
        public const int RED = 0x1903;
        public const int GREEN = 0x1904;
        public const int BLUE = 0x1905;
        public const int ALPHA = 0x1906;
        public const int RGB = 0x1907;
        public const int RGBA = 0x1908;
        public const int BGR = 0x80E0;
        public const int BGRA = 0x80E1;
        public const int LUMINANCE = 0x1909;
        public const int LUMINANCE_ALPHA = 0x190A;

        public const int FALSE = 0;
        public const int TRUE = 1;

        public const int FRONT = 0x404;
        public const int BACK = 0x405;
        public const int FRONT_AND_BACK = 0x0408;


        /*DLL import definitions*/
        [DllImport(OPENGL, EntryPoint = "glTexParameteri")]
        public static extern void TexParameteri(int target, int pname, int param);
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

        [DllImport(OPENGL, EntryPoint = "glGetError")]
        public static extern int GetError();

        [DllImport(OPENGL, EntryPoint = "wglGetCurrentContext")]
        public static extern IntPtr GetCurrentContext();
        [DllImport(OPENGL, EntryPoint = "wglGetCurrentDC")]
        public static extern IntPtr GetCurrentDC();
        [DllImport(OPENGL, EntryPoint = "glDrawBuffer")]
        public static extern void DrawBuffer(int mode);
        [DllImport(OPENGL, EntryPoint = "wglSwapBuffers")]
        public static extern uint SwapBuffers(IntPtr hdc);
        [DllImport(OPENGL, EntryPoint = "wglGetProcAddress")]
        internal static extern IntPtr GetProcAddress(string s);

        public class Texture
        {
            private Bitmap image = null;
            private int texture_id;
            private byte[] buffer = null;
            private bool inited = false;
            
            public Texture(String filename)
            {
                inited = Load(filename);
            }

            public void Bind()
            {
                if (!inited)
                {
                    Log.WriteLine("..[Texture] internal error, glBind() called before texture load, abort.");
                    return;
                }
                BindTexture(TEXTURE_2D, texture_id);
                TexParameteri(TEXTURE_2D, TEXTURE_WRAP_S, CLAMP_TO_EDGE);
                TexParameteri(TEXTURE_2D, TEXTURE_WRAP_T, CLAMP_TO_EDGE);
                TexParameteri(TEXTURE_2D, TEXTURE_MIN_FILTER, LINEAR);
                TexParameteri(TEXTURE_2D, TEXTURE_MAG_FILTER, LINEAR);
                TexImage2D(TEXTURE_2D, 0, RGBA, image.Width, image.Height, 0, RGBA, UNSIGNED_BYTE, buffer);
            }
            public int Width { get { return (inited && image != null ? image.Width : 0); } }
            public int Height { get { return (inited && image != null ? image.Height : 0); } }
            public int TexID { get { return (inited && image != null ? texture_id : -1); } }
            

            private bool Load(String fname)
            {
                try
                {
                    image = new Bitmap(Image.FromFile(fname));
                    BitmapData bd = image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, image.PixelFormat);
                    IntPtr ptr = bd.Scan0; int size = bd.Stride * Height;
                    buffer = new byte[size];
                    Marshal.Copy(ptr, buffer, 0, size);
                    image.UnlockBits(bd); // is that mangled?

                    //image.getRGB(0, 0, image.Width, image.Height, pixelData, 0, image.Width);
                    //buffer = BufferUtils.createByteBuffer(image.Width * image.Height * 4); //4 for RGBA, 3 for RGB
                    /*
                    for (int j = 0, idx = 0; j < Height; ++j)
                    {
                        for (int i = 0; i < Width; ++i, ++idx)
                        {
                            int p = pdata[idx];
                            //buffer.put((byte)((pixel >> 16) & 0xFF));     // Red component
                            //buffer.put((byte)((pixel >> 8) & 0xFF));      // Green component
                            //buffer.put((byte)(pixel & 0xFF));             // Blue component
                            //buffer.put((byte)((pixel >> 24) & 0xFF));     // Alpha component. Only for RGBA
                        }
                    }
                    */
                    //buffer.flip();
                    int[] texture = new int[1] { 0 };
                    GenTextures(1, texture); texture_id = texture[0];
                    return true;
                }
                catch (Exception any)
                {
                    Log.WriteLine("..[Texture] Load exception: {0}", any.Message);
                    return false;
                }
            }
        }
    }
}
