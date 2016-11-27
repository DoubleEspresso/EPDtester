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
using System.Security;

namespace epdTester
{
    public partial class GL : UserControl
    {
        public GL()
        {
            PreDrawGL = DefaultPreDraw;
            InitializeComponent();
            gl_inited = false;
        }
        public delegate void PaintGLFunc();
        public PaintGLFunc PreDrawGL = null;
        public PaintGLFunc PaintGL = null;
        public PaintGLFunc PostDrawGL = null;

        private const string OPENGL = "opengl32.dll";
        private const string GLU = "glu32.dll";
        private const string USER32 = "user32.dll";
        private const string GDI32 = "GDI32.Dll";

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

        public const int PROJECTION = 0x1701;
        public const int MODELVIEW = 0x1700;
        public const int DEPTH_BUFFER_BIT = 0x100;
        public const int COLOR_BUFFER_BIT = 0x4000;
        public const int RGBA8 = 0x8058;

        public const int ALL_ATTRIB_BITS = unchecked((Int32)0xFFFFFFFF);
        public const int SRC_COLOR = 0x0300;
        public const int ONE_MINUS_SRC_COLOR = 0x0301;
        public const int SRC_ALPHA = 0x0302;
        public const int ONE_MINUS_SRC_ALPHA = 0x0303;
        public const int DST_ALPHA = 0x0304;
        public const int ONE_MINUS_DST_ALPHA = 0x0305;
        public const int DST_COLOR = 0x0306;
        public const int ONE_MINUS_DST_COLOR = 0x0307;
        public const int ONE = 1;
        public const int ZERO = 0;
        public const int BLEND = 0x0BE2;
        public const int CONSTANT_ALPHA = 0x8003;

        public const int LIGHT0 = 0x4000;
        public const int LIGHT1 = 0x4001;
        public const int LIGHT2 = 0x4002;
        public const int LIGHT3 = 0x4003;
        public const int LIGHT4 = 0x4004;
        public const int LIGHTING = 0x0B50;
        public const int POSITION = 0x1203;
        public const int LIGHT_MODEL_TWO_SIDE = 0xb52;
        public const int LIGHT_MODEL_LOCAL_VIEWER = 0xb51;

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
        public const int PIXEL_UNPACK_BUFFER = 0x88EC;
        public const int PACK_ALIGNMENT = 0x0D05;

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
        [DllImport(OPENGL, EntryPoint = "glOrtho")]
        public static extern void Ortho(double left, double right, double bottom, double top, double zNear, double zFar);
        [DllImport(OPENGL, EntryPoint = "glMatrixMode")]
        public static extern void MatrixMode(uint mode);
        [DllImport(OPENGL, EntryPoint = "glPopMatrix")]
        public static extern void PopMatrix();
        [DllImport(OPENGL, EntryPoint = "glPixelStorei")]
        public static extern void PixelStorei(int param, int value);
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

        [DllImport(USER32), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport(USER32), SuppressUnmanagedCodeSecurity]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr hdc);

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

        [DllImport(OPENGL, EntryPoint = "wglMakeCurrent")]
        public static extern bool MakeCurrent(IntPtr glDC, IntPtr glRC);
        [DllImport(OPENGL, EntryPoint = "wglDeleteContext")]
        public static extern bool DeleteContext(IntPtr glRC);
        [DllImport(OPENGL, EntryPoint = "wglCreateContext")]
        public static extern IntPtr CreateContext(IntPtr glDC);
        [DllImport(OPENGL, EntryPoint = "wglShareLists")]
        public static extern int ShareLists(IntPtr glDC0, IntPtr glDC1);
        public static void ShareRenderingContext(GL old, GL @new)
        {
            if (ShareLists(old.glhdc, @new.glhdc) == 0)
            {
                Log.WriteLine("Failed to share rendering contexts");
            }
        }

        [DllImport(GDI32)]
        public static extern unsafe int ChoosePixelFormat(IntPtr hDC, PIXELFORMATDESCRIPTOR* ppfd);
        [DllImport(GDI32)]
        public static extern unsafe uint SetPixelFormat(IntPtr hDC, int iPixelFormat, PIXELFORMATDESCRIPTOR* ppfd);

        // pixel format types/definitions
        public const uint PFD_TYPE_RGBA = 0;
        public const uint PFD_TYPE_COLORINDEX = 1;
        public const uint PFD_MAIN_PLANE = 0;
        public const uint PFD_OVERLAY_PLANE = 1;
        public const uint PFD_UNDERLAY_PLANE = 0xff; // (-1)
        public const uint PFD_DOUBLEBUFFER = 0x00000001;
        public const uint PFD_STEREO = 0x00000002;
        public const uint PFD_DRAW_TO_WINDOW = 0x00000004;
        public const uint PFD_DRAW_TO_BITMAP = 0x00000008;
        public const uint PFD_SUPPORT_GDI = 0x00000010;
        public const uint PFD_SUPPORT_OPENGL = 0x00000020;
        public const uint PFD_GENERIC_FORMAT = 0x00000040;
        public const uint PFD_NEED_PALETTE = 0x00000080;
        public const uint PFD_NEED_SYSTEM_PALETTE = 0x00000100;
        public const uint PFD_SWAP_EXCHANGE = 0x00000200;
        public const uint PFD_SWAP_COPY = 0x00000400;
        public const uint PFD_SWAP_LAYER_BUFFERS = 0x00000800;
        public const uint PFD_GENERIC_ACCELERATED = 0x00001000;
        public const uint PFD_SUPPORT_DIRECTDRAW = 0x00002000;
        public const uint PFD_DEPTH_DONTCARE = 0x20000000;
        public const uint PFD_DOUBLEBUFFER_DONTCARE = 0x40000000;
        public const uint PFD_STEREO_DONTCARE = 0x80000000;

        [StructLayout(LayoutKind.Sequential)]
        public struct PIXELFORMATDESCRIPTOR
        {
            public ushort nSize;
            public ushort nVersion;
            public uint dwFlags;
            public byte iPixelType;
            public byte cColorBits;
            public byte cRedBits;
            public byte cRedShift;
            public byte cGreenBits;
            public byte cGreenShift;
            public byte cBlueBits;
            public byte cBlueShift;
            public byte cAlphaBits;
            public byte cAlphaShift;
            public byte cAccumBits;
            public byte cAccumRedBits;
            public byte cAccumGreenBits;
            public byte cAccumBlueBits;
            public byte cAccumAlphaBits;
            public byte cDepthBits;
            public byte cStencilBits;
            public byte cAuxBuffers;
            public byte iLayerType;
            public byte bReserved;
            public uint dwLayerMask;
            public uint dwVisibleMask;
            public uint dwDamageMask;
        }

        // GL context variables
        protected IntPtr hdc = IntPtr.Zero;
        public IntPtr DeviceContextHandle { get { return hdc; } }
        protected IntPtr glhdc = IntPtr.Zero;
        public IntPtr glContext { get { return glhdc; } }
        protected bool gl_inited = false;
        protected bool gl_disposing = false;
        // initialization
        private unsafe bool InitPixelFormat()
        {
            PIXELFORMATDESCRIPTOR pfd = new PIXELFORMATDESCRIPTOR();
            pfd.nSize = 40; // (ushort)sizeof(PIXELFORMATDESCRIPTOR);
            pfd.nVersion = 1;
            pfd.dwFlags = (uint)(PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER);// | PFD_DRAW_TO_BITMAP);
            pfd.dwLayerMask = PFD_MAIN_PLANE;
            pfd.iPixelType = (byte)PFD_TYPE_RGBA;
            pfd.cColorBits = 8; // nColorBits;
            pfd.cDepthBits = 16; // nDepthBits;
            pfd.cStencilBits = 1;// nStencilBits;
            pfd.cAccumBits = 1;// nAccumBits;

            int iPixelformat = ChoosePixelFormat(hdc, &pfd);
            if (iPixelformat == 0) return false;
            if (SetPixelFormat(hdc, iPixelformat, &pfd) == 0) return false;
            return true;
        }
        unsafe bool CreateGLContext()
        {
            hdc = GetDC(this.Handle);

            if (!InitPixelFormat()) return false;
            glhdc = CreateContext(hdc);
            if (glhdc == IntPtr.Zero) return false;
            if (MakeCurrent(hdc, glhdc) == false)
            {
                hdc = IntPtr.Zero; glhdc = IntPtr.Zero;
                return false;
            }
            //LoadExtensions();
            return true;
        }
        public bool MakeCurrent()
        {
            if (!gl_inited) return false;
            MakeCurrent(hdc, glhdc);
            IntPtr nhdc = GetCurrentDC();
            IntPtr nglhdc = GetCurrentContext();
            if (!Disposing && ((hdc != nhdc) || (glhdc != nglhdc)))
            {
                Log.WriteLine("Failed to make rendering device current (hdc={0}, glhdc={1})", hdc, glhdc);
                return false;
            }
            return true;
        }
        public void SwapBuffers()
        {
            if (gl_inited) SwapBuffers(hdc);
        }
        public const int NO_ERROR = 0;
        public const int INVALID_ENUM = 0x0500;
        public const int INVALID_VALUE = 0x0501;
        public const int INVALID_OPERATION = 0x0502;
        public const int STACK_OVERFLOW = 0x0503;
        public const int STACK_UNDERFLOW = 0x0504;
        public const int OUT_OF_MEMORY = 0x0505;
        public const int TABLE_TOO_LARGE = 0x8031;
        public void CheckGLError()
        {
            IntPtr curr_glhdc = GetCurrentContext();
            //if (curr_glhdc != glhdc) { Log.WriteLine("*** GL ERROR (wrong context was [{0}] instead of [{1}]!)", curr_glhdc, glhdc); MakeCurrent(); }
            int err = GetError();
            if ((err == NO_ERROR) || Disposing) return;
            string err_str = null;
            switch (err)
            {
                case INVALID_ENUM: err_str = "INVALID_ENUM"; break;
                case INVALID_VALUE: err_str = "INVALID_VALUE"; break;
                case INVALID_OPERATION: err_str = "INVALID_OPERATION"; break;
                case STACK_OVERFLOW: err_str = "STACK_OVERFLOW"; break;
                case STACK_UNDERFLOW: err_str = "STACK_UNDERFLOW"; break;
                case OUT_OF_MEMORY: err_str = "OUT_OF_MEMORY"; break;
                case TABLE_TOO_LARGE: err_str = "TABLE_TOO_LARGE"; break;
                default: err_str = "unknown error"; break;
            }
            Log.WriteLine("*** GL ERROR (hdc={0}, glhdc={1}): {2}", this.hdc, this.glhdc, err_str);
        }
        // Basic Wire from Forms Update to virtual exposed funcs
        protected override void OnLoad(EventArgs e)
        {
            if (gl_disposing) return; // weird bug!
            base.OnLoad(e);
            if (!DesignMode && !gl_inited)
            {
                InitializeComponent();
                if (CreateGLContext())
                {
                    if (Disposing) return; //weird bug
                    InitGLpriv();
                    //ChangeFont(Font, ForeColor);
                    gl_inited = true;
                }
            }
        }
        virtual protected void InitGLpriv()
        {
            MakeCurrent();
            ClearColor(1f, 1f, 1f, 1f);
        }
        public void DefaultPreDraw()
        {
            //DrawBuffer(BACK);
            //CheckGLError();
            //if (bgMode == 0)
            //{
            //    Clear(COLOR_BUFFER_BIT | DEPTH_BUFFER_BIT);
            //    if (bgImageName == null) return;
            //    // else : draw the bgImage(s)
            //    DepthMask(false);
            //    GL.MatrixMode(GL.PROJECTION);
            //    GL.LoadIdentity();
            //    GL.Ortho(0, 1, 1, 0, -1, 1);
            //    GL.MatrixMode(GL.MODELVIEW);
            //    GL.LoadIdentity();
            //    GL.Disable(LIGHTING);
            //}
            //else
            //{
            //    // else: we will need to draw a (textured??) quad ...
            //    // only clear Z values ...
            //    Clear(DEPTH_BUFFER_BIT);
            //    DepthMask(false);

            //    // do not clear the color buffer ... draw a quad instead ..
            //    GL.MatrixMode(GL.PROJECTION);
            //    GL.LoadIdentity();
            //    GL.Ortho(0, 1, 1, 0, -1, 1);
            //    GL.MatrixMode(GL.MODELVIEW);
            //    GL.LoadIdentity();
            //    GL.Disable(LIGHTING);

            //    // draw colors
            //    Begin(QUADS);
            //    GL.Color(bgColor);

            //    Vertex2d(0, 0);
            //    Vertex2d(1, 0);

            //    GL.Color(bgColorBottom);
            //    //if (tex1d_colorRamp != null)
            //    //{
            //    //    ActiveTexture(TEXTURE1);
            //    //    TexCoord1f(1);
            //    //}
            //    Vertex2d(1, 1);
            //    Vertex2d(0, 1);
            //    End();
            //    CheckGLError();

            //} // bgMode ...

            //if (bgImageName != null)
            //{
            //    //draw a transparent textured quad over it
            //    Enable(BLEND);
            //    BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
            //    GLTexture.env(REPLACE);
            //    tex_bgImage.DrawXYQuad(0, 0, 1, 1);
            //    CheckGLError();
            //    Disable(BLEND);
            //}
            //else if (tex_bgImage != null)
            //{
            //    tex_bgImage.free();
            //    tex_bgImage = null;
            //}
            //CheckGLError();

            //DepthMask(true);
            //GL.DepthFunc(GL.LESS);
            //GL.Enable(LIGHTING);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!gl_inited) return;
            MakeCurrent();
            Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            SafeInvalidate(true);
        }
        protected virtual void glDispose()
        {
            if (!gl_inited) return;
            if (gl_disposing) return;

            if (!MakeCurrent())
            {
                Log.WriteLine("Could not switch GL context, not disposing...");
                return;
            }
            gl_disposing = true;
            gl_inited = false;
            ReleaseDC(this.Handle, hdc);
            hdc = IntPtr.Zero;
            DeleteContext(glhdc);
            Log.WriteLine("GL context [{0}] released.", glhdc);
            glhdc = IntPtr.Zero;
        }
        delegate void InvalidateCallback(bool b);
        public new void Invalidate(bool b)
        {
            SafeInvalidate(b);
        }
        protected int invalidate_request_nb = 0;
        public void SafeInvalidate(bool b)
        {
            if (InvokeRequired)
            {
                if (gl_inited)
                {
                    if (System.Threading.Interlocked.Increment(ref invalidate_request_nb) > 1) return;
                    try
                    {
                        Invoke(new InvalidateCallback(SafeInvalidate), new object[] { b });
                    }
                    catch (Exception)
                    {
                        // silently ignore!
                    }
                }
                return; // at worst .. miss 1 invalidate.
            }
            // from the same thread!
            base.Invalidate(b);
            System.Threading.Interlocked.Exchange(ref invalidate_request_nb, 0);
        }
        int paint_request_nb = 0;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (System.Threading.Interlocked.Increment(ref paint_request_nb) > 1) return;
            try
            {
                if (DesignMode) return;
                if (gl_inited)
                {
                    if (!MakeCurrent())
                    {
                        CheckGLError();
                        return;
                    }
                    if (PreDrawGL != null) { PreDrawGL(); CheckGLError(); }
                    if (PaintGL != null) { PaintGL(); CheckGLError(); }
                    if (PostDrawGL != null) { PostDrawGL(); CheckGLError(); }
                    SwapBuffers();
                }
                else { base.OnPaint(e); }
            }
            catch (Exception ex)
            {
                Log.WriteLine("..[GL] paint exception: {0}", ex.Message);
            }
            finally
            {
                System.Threading.Interlocked.Exchange(ref paint_request_nb, 0);
            }
        }
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
                TexImage2D(TEXTURE_2D, 0, RGBA, image.Width, image.Height, 0, BGRA, UNSIGNED_BYTE, buffer);
            }
            public int Width { get { return (image != null ? image.Width : 0); } }
            public int Height { get { return (image != null ? image.Height : 0); } }
            public int TexID { get { return (image != null ? texture_id : -1); } }

            private bool Load(String fname)
            {
                try
                {
                    image = new Bitmap(Image.FromFile(fname));
                    BitmapData bd = image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, image.PixelFormat);
                    IntPtr ptr = bd.Scan0; int size = bd.Stride * Height * 4; // rgba components
                    buffer = new byte[size];
                    for (int h = 0; h < Height; ++h) Marshal.Copy(new IntPtr((long)(ptr + bd.Stride * h)), buffer, bd.Stride * h, bd.Stride);
                    image.UnlockBits(bd);

                    int[] texture = new int[1] { 0 };
                    GenTextures(1, texture);
                    texture_id = texture[0];

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
