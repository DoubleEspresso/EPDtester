using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public class EpdFile
    {
        public List<EPDPosition> Positions = null;
        public string Name = null; // classifier, if any
        public bool canLoad = false;
        public string FileName = null;
               
        /*entry data for epd test*/
        public struct EPDPosition
        {
            public string fen; // chess position
            public string acn; //analysis count nodes
            public string acs; //analysis count seconds
            public string am; //avoid move
            public string bm; //best move
            public string c0; //comment
            public string ce; // centipawn evaluation
            public string dm; //draw move
            public string draw_accept;
            public string draw_offer;
            public string draw_reject;
            public string eco;
            public string fmvn;
            public string hmvc;
            public string id;
            public string nic;
            public string noop; //no operation
            public string pm; //predicted move
            public string pv; //predicted variation
            public string rc; //repetition count
            public string resign;
            public string sm;
            public string tcgs;
            public string tcri;
            public string tcsi;
            public Result EngineResult;
        }
        public struct Result
        {                     
            public string enginemove;
            public uint depth;
            public uint time;
            public uint nodes;
            public uint nps;
            public uint hHits;
            public bool correct;
        }        
        public EpdFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Log.WriteLine("..[epd-file] warning: epd-file ({0}) does not exist.", filename);
                canLoad = false;
                return;
            }
            FileName = filename;
            Name = StringUtils.RemoveExtension(StringUtils.BaseName(filename));
            Positions = new List<EPDPosition>();
            Read();
        }
        void Read()
        {
            using (StreamReader r = new StreamReader(FileName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    EPDPosition p = new EPDPosition();
                    if (ParseEPDLine(line, ref p)) Positions.Add(p);
                }
            }
        }
        bool ParseEPDLine(string line, ref EPDPosition p)
        {
            // see e.g. chessprogrammingwiki (epd-description)
            string[] tags = new string[]
            {
                "acn", //analysis count nodes
                "acs", //analysis count seconds
                "am", //avoid move
                "bm", //best move
                "c0", //comment
                "ce", // centipawn evaluation
                "dm", //draw move
                "draw_accept",
                "draw_offer",
                "draw_reject",
                "eco",
                "fmvn",
                "hmvc",
                "id",
                "nic",
                "noop", //no operation
                "pm", //predicted move
                "pv", //predicted variation
                "rc", //repetition count
                "resign",
                "sm",
                "tcgs",
                "tcri",
                "tcsi"
                //"v0", "v1", "v2", "v3", "v4", "v5", "v6", "v7", "v8", "v9" // too many epd descriptions contain v1-v9 
            };
            // sort indicies of all tags found in line (smallest to largest)
            // need to search through each line since epd file lines are scrambled in general, however
            // 1. substring from idx 0 to idx (first tag) is fen (always?) 
            Dictionary<int, string> tagdict = new Dictionary<int, string>();
            try
            {
                foreach (string t in tags)
                {
                   if (line.IndexOf(t) >= 0) tagdict.Add(line.IndexOf(t), t);
                }
                List<int> indices = tagdict.Keys.ToList(); indices.Sort(); // smallest to largest by default
                int lastidx = 0; string lasttag = "";
                foreach (int idx in indices)
                {
                    string tag = tagdict[idx];
                    string substring = line.Substring(lastidx, idx - lastidx);
                    if (lastidx == 0) // fen string
                    {
                        p.fen = substring.Replace(";", "");
                    }
                    lastidx = idx;
                    // load struct -- just basic tags
                    LoadTagData(ref p, lasttag, substring); // cleanup tag and load into EPDPosition structure
                    lasttag = tag;
                }
                string last_substring = line.Substring(lastidx, line.Length - lastidx);
                LoadTagData(ref p, lasttag, last_substring); 

                // initialize engine result struct
                p.EngineResult.enginemove = "(none)";
                p.EngineResult.depth = p.EngineResult.time = p.EngineResult.nodes = p.EngineResult.nps = p.EngineResult.hHits = 0;
                p.EngineResult.correct = false;
                return true;
            }
            catch (Exception any)
            {
                Log.WriteLine("..[epd-file] exception parsing epd line ({0}) : {1}", line, any.Message);
                return false;
            }
        }
        string Clean(string tag, string data)
        {
            return data.Replace(tag, "").Replace(";", "");
        }
        void LoadTagData(ref EPDPosition p, string tag, string data)
        {
            switch(tag)
            {
                case "acn": p.acn = Clean(tag, data); break;
                case "acs": p.acs = Clean(tag, data); break;
                case "am": p.am = Clean(tag, data); break;
                case "bm": p.bm = Clean(tag, data); break;
                case "c0": p.c0 = Clean(tag, data); break;
                case "ce": p.ce = Clean(tag, data); break;
                case "dm": p.dm = Clean(tag, data); break;
                case "draw_accept": p.draw_accept = Clean(tag, data); break;
                case "draw_offer": p.draw_offer = Clean(tag, data); break;
                case "draw_reject": p.draw_reject = Clean(tag, data); break;
                case "eco": p.eco = Clean(tag, data); break;
                case "fmvn": p.fmvn = Clean(tag, data); break;
                case "hmvc": p.hmvc = Clean(tag, data); break;
                case "id": p.id = Clean(tag, data); break;
                case "nic": p.nic = Clean(tag, data); break;
                case "noop": p.noop = Clean(tag, data); break;
                case "pm": p.pm = Clean(tag, data); break;
                case "pv": p.pv = Clean(tag, data); break;
                case "rc": p.rc = Clean(tag, data); break;
                case "resign": p.resign = Clean(tag, data); break;
                case "sm": p.sm = Clean(tag, data); break;
                case "tcgs": p.tcgs = Clean(tag, data); break;
                case "tcri": p.tcri = Clean(tag, data); break;
                case "tcsi": p.tcsi = Clean(tag, data); break;
                default: if (tag != null && tag != "") Log.WriteLine("..[epd-file] unknown tag ({0}) .. ignored", tag); break;
            }
        }
        public uint EloEstimate
        {
            get { return analyze(); }
        }
        private uint analyze()
        {
            if (!isLoaded) return 0;
            else
            {
                return 1; // TODO
            }
        }
        public bool isLoaded
        {
            get { return Positions != null && Positions.Count > 0; }
        }
        public void Load(string filename)
        {

        }
    }
}
