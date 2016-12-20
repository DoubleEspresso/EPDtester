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
        bool useLog = false;
        public string EngineLogFilename = null;
        public string EngineLogDirectory = null;
        Process eprocess = null;
        StreamWriter engineWriter = null;
        StreamReader engineReader = null;
        private StreamWriter logWriter = null;
        public enum Type { UCI, WINBOARD, UNKNOWN }
        public Type EngineProtocol = Type.UCI;
        public ChessParser Parser = new UCIParser();
        public ChessBoard chessBoard = null;

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
                Settings.set(string.Format("{0}\\Log output", basetag), useLog);
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
                Settings.get(string.Format("{0}\\Log output", basetag), ref useLog);
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
        public bool WritingToLogFile()
        {
            return useLog;
        }
        public void UseLog(bool usingLog)
        {
            useLog = usingLog;
            if (useLog) WriteToLogCallback += WriteLine;
            else WriteToLogCallback -= WriteLine;
        }
        public void Command(string cmd)
        {
            if (!Running || !Loaded || eprocess == null || engineWriter == null) return;
            if (cmd.Contains("go"))
            {
                Parser.NewSearch(); // uci specific.
            }
            engineWriter.WriteLine(cmd);
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

                if (useLog)
                {
                    EngineLogFilename = Name + "-" + StringUtils.TimeStamp() + ".log";
                    EngineLogDirectory = Log.DirectoryName;
                    if (!openEngineLog(EngineLogDirectory, EngineLogFilename))
                    {
                        EngineLogFilename = null;
                        EngineLogDirectory = null;
                        Log.WriteLine("..[engine] Warning: failed to start log file for engine @({0})", EngineLogFilename);
                        useLog = false; // does not update UI (fyi).
                    }
                }
                else
                {
                    EngineLogFilename = null;
                    EngineLogDirectory = null;
                }
                eprocess = Process.Start(pinfo);                
                engineWriter = eprocess.StandardInput;
                engineReader = eprocess.StandardOutput;
                new Task(ReadEngineStreamAsync).Start(); // settings controlled?
                Running = true;
                Thread.Sleep(100); 
            }
            catch(Exception any)
            {
                Log.WriteLine("..[engine] exception running {0} with args ({1}).\nStackTrace :\n {2}", Name, args, any.StackTrace);
            }
        }
        public event EventHandler<AnalysisUIData> AnalysisUICallback = null;
        event EventHandler<string> WriteToLogCallback = null;
        private async void ReadEngineStreamAsync()
        {
            char [] buff = new char[2048];
            while (!eprocess.HasExited)
            {
                int length = await engineReader.ReadAsync(buff, 0, buff.Length);
                if (length <= 1) return;
                Log.CustomLogName = Name;
                string readout = new string(buff).Substring(0, length-1);
                if (AnalysisUICallback != null)
                {
                    // parse readout on this (background) thread
                    AnalysisUIData data = FormatForAnalysisUI();
                    if (data != null) AnalysisUICallback(this, data);
                }
                if (useLog && WriteToLogCallback != null) WriteToLogCallback(this, readout);
                Parser.ParseLine(readout);
                if (readout.Contains(stopOnToken) && Parser.CallbackOnBestmove != null)
                    Parser.CallbackOnBestmove(null, null);
                Thread.Sleep(10);
            }
            Running = false;
            Log.WriteLine("..WARNING: engine process has exited");
        }
        public class AnalysisUIData
        {
            public string depth;
            public string nps;
            public string hashfull;
            public string currmove;
            public string cpu;
            public string pv;
            public List<double> evals;
        }
        private AnalysisUIData FormatForAnalysisUI()
        {
            if (Parser == null) return null;
            ChessParser.Data[] history = new ChessParser.Data[Parser.History.Count];
            Parser.History.CopyTo(history);
            AnalysisUIData data = new AnalysisUIData();
            data.evals = new List<double>();
            if (history == null || history.Length <= 0) return null;
            foreach (ChessParser.Data d in history)
            {
                if (String.IsNullOrWhiteSpace(d.pv)) continue;
                data.depth = "depth: " + Convert.ToString(d.depth);
                double eval = d.eval / 100.0; data.evals.Add(eval);
                data.nps = "nps: " + Convert.ToString(d.nps);
                data.hashfull = "hashhits: " + Convert.ToString(d.hashhits);
                data.currmove = "currmove: " + Convert.ToString("n/a");
                data.cpu = "cpu: " + Convert.ToString("n/a");
                data.pv = "[" + String.Format("{0:F2}", eval) + "]  " + FormatPV(d.pv);
            }
            return data;
        }
        string FormatPV(string moves)
        {
            if (String.IsNullOrWhiteSpace(moves)) return "";
            try
            {
                Position p = new Position(chessBoard.pos.toFen());
                string san_moves = "";
                string[] tokens = moves.Split(' ');
                foreach (string move in tokens)
                {
                    if (String.IsNullOrWhiteSpace(move)) continue;
                    int[] fto = Position.FromTo(move);
                    if (fto == null) break;
                    if (p.doMove(fto[0], fto[1], p.PieceOn(fto[0]), p.ToMove())) san_moves += p.toSan(move) + " ";
                    else break;
                    if (san_moves.Length > 100) break;
                }
                return san_moves;
            }
            catch { } // silently ignore.
            return "";
        }
        object LogLock = new object();
        string stopOnToken = "";
        public void SetBestMoveCallback(EventHandler cb, string stopToken = "")
        {
            Parser.CallbackOnBestmove = null;
            Parser.CallbackOnBestmove += cb;
            stopOnToken = (stopToken == "" ? "bestmove" : stopToken);
        }
        public void WriteLine(object sender, string str)
        {
            // fixme : with analysis mode active, possible race condition/deadlock
            // results from writing to the log both for the engine/analysis and 
            // for the main event logs .. issue (engine stalls/log writing stalls) is reproducible, but does not appear
            // when not logging engine output 
            lock (LogLock)
            {
                logWriter.Write(string.Format("{0}", str));
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
                eprocess.Kill();
                Log.WriteLine("..[engine] closed engine {0} successfully", Name != null ? Name : "(unknown-name)");
            }
            catch (Exception any)
            {
                Log.WriteLine("!!Warning: exception closing engine {0} : {1}", Name != null ? Name : "(unknown-name)", any.Message);
            }
            Running = false;
        }
    }
}
