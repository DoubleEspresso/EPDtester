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
        public int Elo = 0;       
        public string FileName = null;
        public string Author = null;
        public string Version = null;
        public bool Loaded = false;
        public bool Running = false;
        public string EngineLogFilename = null;
        public string EngineLogDirectory = null;
        Process eprocess = null;
        StreamWriter engineWriter = null;
        StreamReader engineReader = null;
        private StreamWriter logWriter = null;
        public enum Type { UCI, WINBOARD, UNKNOWN }
        public Type EngineProtocol = Type.UCI;
        public ChessParser Parser = new UCIParser();

        public Engine() { }
        public bool canOpen()
        {
            if (!Loaded)
            {
                Loaded = File.Exists(FileName);
                return Loaded;
            }
            Log.WriteLine("..[engine] {0} already loaded", Name);
            return true;
        }
        public bool SaveSettings(int eidx)
        {
            try
            {
                // basic chess engine configuration/path location
                string basetag = string.Format("Engine\\{0}", eidx);
                Settings.set(string.Format("{0}\\Name", basetag), Name);
                Settings.set(string.Format("{0}\\Path", basetag), FileName);
                Settings.set(string.Format("{0}\\Opening Book\\Path", basetag), OpeningBook);
                Settings.set(string.Format("{0}\\Use Opening Book", basetag), useBook);
                Settings.set(string.Format("{0}\\Table Base\\Path", basetag), TableBase);
                Settings.set(string.Format("{0}\\Config file\\Path", basetag), ConfigFile);
                Settings.set(string.Format("{0}\\elo", basetag), (int) Elo);
                Settings.set(string.Format("{0}\\Author", basetag), Author);
                Settings.set(string.Format("{0}\\Version", basetag), Version);
                Settings.set(string.Format("{0}\\Protocol", basetag), (EngineProtocol == Type.UCI ? "UCI" : EngineProtocol == Type.WINBOARD ? "Winboard" : "Unknown"));
            }
            catch (Exception any)
            {
                Log.WriteLine("..[engine] exception saving settings entries for engine ({0}) : {1}", FileName, any.Message);
                return false;
            }
            return true;
        }
        public bool ReadSettings(int eidx)
        {
            try
            {
                // basic chess engine configuration/path location
                string basetag = string.Format("Engine\\{0}", eidx);
                Settings.get(string.Format("{0}\\Name", basetag), ref Name);
                Settings.get(string.Format("{0}\\Path", basetag), ref FileName);
                if (!canOpen()) return false;
                Settings.get(string.Format("{0}\\Opening Book\\Path", basetag), ref OpeningBook);
                Settings.get(string.Format("{0}\\Use Opening Book", basetag), ref useBook);
                Settings.get(string.Format("{0}\\Table Base\\Path", basetag), ref TableBase);
                Settings.get(string.Format("{0}\\Config file\\Path", basetag), ref ConfigFile);
                Settings.get(string.Format("{0}\\elo", basetag), ref Elo);
                Settings.get(string.Format("{0}\\Author", basetag), ref Author);
                Settings.get(string.Format("{0}\\Version", basetag), ref Version);
                string p = "Unknown";  Settings.get(string.Format("{0}\\Protocol", basetag), ref p);
                EngineProtocol = (p == "Unknown" ? Type.UNKNOWN : p == "UCI" ? Type.UCI : Type.WINBOARD);
            }
            catch (Exception any)
            {
                Log.WriteLine("..[engine] exception saving settings entries for engine ({0}) : {1}", FileName, any.Message);
                return false;
            }
            return true;

        }
        public void Command(string cmd, string waitToken = null)
        {
            if (!Running || !Loaded || eprocess == null || engineWriter == null) return;
            if (cmd.Contains("go"))
            {
                Parser.NewSearch(waitToken); // uci specific.
            }
            engineWriter.WriteLine(cmd);
            Thread.Sleep(20);
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
                EngineLogFilename = Name + "-" + StringUtils.TimeStamp() + ".log";
                EngineLogDirectory = Log.DirectoryName;
                if (!openEngineLog(EngineLogDirectory, EngineLogFilename))
                {
                    Log.WriteLine("..[engine] Warning: failed to start log file for engine @({0})", EngineLogFilename);
                }
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
                Log.CustomLogName = Name;
                WriteLine((new string(buff)).Substring(0, length-1)); // event handler for engine output
                Thread.Sleep(1);
            }
            Running = false;
        }
        object LogLock = new object();
        string stopOnToken = "";
        public void SetBestMoveCallback(EventHandler cb, string stopToken = "")
        {
            Parser.CallbackOnBestmove = null;
            Parser.CallbackOnBestmove += cb;
            stopOnToken = (stopToken == "" ? "bestmove" : stopToken);
        }
        public void WriteLine(string str)
        {
            lock (LogLock)
            {
                logWriter.Write(string.Format("{0}", str));
                Parser.ParseLine(str);
                if (str.Contains(stopOnToken) && Parser.CallbackOnBestmove != null)
                    Parser.CallbackOnBestmove(null, null);
                logWriter.Flush();
            }
        }
        bool openEngineLog(string DirectoryName, string EngineLogName)
        {
            if (!Running)
            {
                try
                {
                    if (!Directory.Exists(DirectoryName))
                    {
                        Directory.CreateDirectory(DirectoryName);
                    }
                    string fname = DirectoryName + "\\" + EngineLogName;
                    FileStream fsout = new FileStream(fname, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete);
                    logWriter = new StreamWriter(fsout);
                    logWriter.AutoFlush = true;
                }
                catch (Exception)
                {
                    logWriter = null;
                }
            }
            return (logWriter != null);
        }
        public void Close()
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
