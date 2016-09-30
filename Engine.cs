﻿using System;
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
        Process eprocess = null;
        StreamWriter engineWriter = null;
        StreamReader engineReader = null;
        public enum Type { UCI, WINBOARD, UNKNOWN }
        public Type EngineProtocol = Type.UCI;
        ChessParser Parser = null;

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
