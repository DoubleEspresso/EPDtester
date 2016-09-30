using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace epdTester
{
    public class Engine
    {
        public bool useBook = false;
        public string OpeningBook = null;
        public string TableBase = null;
        public string Name = null;
        public string ConfigFile = null;
        public string[] Options = null;
        public uint Elo = 0;       
        public string FileName = null;
        public bool Loaded = false;
        public bool Running = false;
        Process eprocess = null;
        StreamWriter engineWriter = null;
        StreamReader engineReader = null;
        public enum Type { UCI, WINBOARD, UNKNOWN }
        ChessParser Parser = null;

        public Engine() { }
        public bool open()
        {
            if (!Loaded)
            {
                    Loaded = File.Exists(FileName);
                    return Loaded;
            }
            Log.WriteLine("..[engine] {0} already loaded", Name);
            return true;
        }

        public void Command(string cmd)
        {
            if (!Running || !Loaded || eprocess == null || engineWriter == null) return;
            engineWriter.WriteLine(cmd);
            Thread.Sleep(100);
        }

        public void Start(string args = "")
        {            
            try
            {
                ProcessStartInfo pinfo = new ProcessStartInfo(FileName, args)
                {
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    StandardOutputEncoding = Encoding.UTF8,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                eprocess = Process.Start(pinfo);
                engineWriter = eprocess.StandardInput;
                engineReader = eprocess.StandardOutput;
                new Task(LogOutputAsync).Start();
                Running = true;
                Thread.Sleep(100);                 
            }
            catch(Exception any)
            {
                Log.WriteLine("..[engine] exception running {0} with args ({1}).\nStackTrace :\n {2}", Name, args, any.StackTrace);
            }
        }

        private async void LogOutputAsync()
        {
            char [] buff = new char[1024];
            while (!eprocess.HasExited)
            {
                int length = await engineReader.ReadAsync(buff, 0, buff.Length);
                Log.WriteLine("{0}", (new string(buff)).Substring(0, length-1)); // event handler for engine output
                Thread.Sleep(1);
            }
            Running = false;
        }

        public void stop()
        {
            if (!Running || !Loaded || eprocess == null) return;
            try
            {
                eprocess.Close();
                Log.WriteLine("..[engine] closed engine {0} successfully", Name);
            }
            catch { }
            Running = false;
        }
    }
}
