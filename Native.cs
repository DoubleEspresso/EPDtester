using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    // note : basic importing from cpp dll (idea would be for data compression/searching opening books?)
    public class Native
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double IntegrationFunc(double x, IntPtr p);
        [DllImport(@"uLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GaussQuadratureInstance(uint N);
        [DllImport(@"uLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GaussQuadratureIntegrate(IntegrationFunc func, double xlow, double xhi, int evals, IntPtr handle);

        /*external calls to dll methods*/
        public static bool GaussQuadIntegrate(IntegrationFunc f, double xlow, double xhi, int evals)
        {
            try
            {
                IntPtr h = GaussQuadratureInstance((uint)evals);
                GCHandle.Alloc(h);               
                GCHandle.Alloc(Marshal.GetFunctionPointerForDelegate(f));

                double r = GaussQuadratureIntegrate(f, xlow, xhi, evals, h);
                Log.WriteLine("...gauss-quadrature result={0}", r);
                return true;
            }
            catch (Exception e)
            {
                Log.WriteLine("..exception calling DLL method (gaussquad-integrate) : {0}", e.Message);
                return false;
            }
        }
    }
}
