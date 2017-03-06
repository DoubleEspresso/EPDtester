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
        /*deprecated examples*/
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate double IntegrationFunc(double x, IntPtr p);

        [DllImport(@"uLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GaussQuadratureInstance(uint N);

        [DllImport(@"uLib.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GaussQuadratureIntegrate(IntegrationFunc func, double xlow, double xhi, int evals, IntPtr handle);



        [DllImport(@"db.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr PGNIO_instance(IntPtr pgn_in_fname);

        [DllImport(@"db.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool PGN_parse(IntPtr pgn_in_fname, IntPtr db_name, uint size_mb);

        [DllImport(@"db.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void PGN_find(StringBuilder sres, IntPtr pgn_in_fname, IntPtr fen);


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

        public static bool CreateFromPGN(string infile, string dbname, uint size_mb)
        {
            //GCHandle in_gc = GCHandle.Alloc(infile, GCHandleType.Pinned);
            //GCHandle db_gc = GCHandle.Alloc(dbname, GCHandleType.Pinned);

            bool res = PGN_parse(Marshal.StringToHGlobalAnsi(infile), Marshal.StringToHGlobalAnsi(dbname), size_mb);

            //in_gc.Free();
            //db_gc.Free();

            return res;
        }

        public static string PGNLookup(string dbname, string fen)
        {
            // note: tried allocating a pgn instance .. so we don't have to instantiate the pgn
            // class (open the binary file) each time a request is made to search for a position
            // .. but could not solve "invalid memory access" errors associated with instantiating
            // the member "book" pointer in pgn_io (polyglot entry struct specifically) .. suspect GC moves
            // the member pointer between allocation and usage .. but unsure (todo)
            //GCHandle handle = GCHandle.Alloc(PGNIO_instance(dbs_ptr), GCHandleType.Pinned); // deprecated

            // current "sidestep" workaround ..
            //GCHandle dbn_gc = GCHandle.Alloc(dbname, GCHandleType.Pinned);
            //GCHandle fen_gc = GCHandle.Alloc(fen, GCHandleType.Pinned);
            StringBuilder res = new StringBuilder(1024); // to read back the cpp search results
            PGN_find(res, Marshal.StringToHGlobalAnsi(dbname), Marshal.StringToHGlobalAnsi(fen));

            //dbn_gc.Free();
            //fen_gc.Free();

            return res.ToString();
        }

    }
}
