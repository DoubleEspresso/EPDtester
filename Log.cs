using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace epdTester
{
    public class Log
    {
        static object LogLock = new object();
        static private StreamWriter logWriter = null;
        private static string dirName = null;
        static private string logName = null;
        static private string customLogName = null;
        static bool inited = false;

        static public string DirectoryName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(dirName)) dirName = string.Format("{0}\\Log", Application.LocalUserAppDataPath);
                return dirName;
            }
        }
        static public string CustomLogName
        {
            get
            {
                if (customLogName == null) return "";
                return customLogName;
            }
            set
            {
                customLogName = value;
            }
        }
        static public string LogName
        {
            get
            {
                if (logName == null)
                {
                    logName = string.Format("{0}log-{1}", CustomLogName, StringUtils.TimeStamp());
                }
                return logName;
            }
        }
        static public string FileName
        {
            get
            {
                return string.Format("{0}\\{1}.log", DirectoryName, LogName);
            }
        }
        static private bool open()
        {
            if (!inited)
            {
                try
                {
                    if (!Directory.Exists(DirectoryName))
                    {
                        Directory.CreateDirectory(DirectoryName);
                    }
                    FileStream fsout = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);
                    logWriter = new StreamWriter(fsout);
                    logWriter.AutoFlush = true;
                    inited = true;
                    LogReport();
                }
                catch (Exception)
                {
                    logWriter = null;
                }
            }
            return (logWriter != null);
        }
        static private void LogReport()
        {
            Log.WriteLine("{0} initialized on {1} ({2}, {3})", Application.ProductName, DateTime.Now, Application.CompanyName, Application.ProductVersion);
            Microsoft.VisualBasic.Devices.ComputerInfo pc = new Microsoft.VisualBasic.Devices.ComputerInfo();
            Log.WriteLine("  OS {0}, CLR version {1}", pc.OSFullName, Environment.Version.ToString());
            Log.WriteLine("  {0} [{1} {2}]", Environment.OSVersion.VersionString, pc.OSPlatform, pc.OSVersion);
            Log.WriteLine("  machine {0}, user {1} ", Environment.MachineName, Environment.UserName);
            {
                double ap = pc.AvailablePhysicalMemory / 1024.0;
                double tp = pc.TotalPhysicalMemory / 1024.0;
                double av = pc.AvailableVirtualMemory / 1024.0;
                double tv = pc.TotalVirtualMemory / 1024.0;
                Log.WriteLine("  {0} core(s), mem-avail {1}% physical ({2} / {3}) - {4}% virtual ({5}/{6}) ",
                    Environment.ProcessorCount, 
                    String.Format("{0:00}", (100.0 * ap) / tp),
                    String.Format("{0:0}GB", ap / (1024.0 * 1024.0)),
                    String.Format("{0:0}GB", tp / (1024.0 * 1024.0)),
                    String.Format("{0:00}", (100.0 * av) / tv),
                    String.Format("{0:0}GB", av / (1024.0 * 1024.0)),
                    String.Format("{0:0}GB", tv / (1024.0 * 1024.0)));
            }
            Log.WriteLine("== {0} {1} version {2} compiled on {3} (Branch={4}, Tag={5})",
                Program.appname(),
#if _64BIT
                "64bits",
#else
                "32bits",
#endif
                Program.versionStr(),
                About.compilationDate,
                About.gitBranch,
                About.gitTag);
            Log.WriteLine("");
        }
        public static void WriteLine(string str)
        {
            lock (LogLock)
            {
                if (!open()) return;
                logWriter.WriteLine(string.Format("{0}:  {1}", StringUtils.LogTimeStamp(), str));
                logWriter.Flush();
            }
        }
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }
 
    }
}
