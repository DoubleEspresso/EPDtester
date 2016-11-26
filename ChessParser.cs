using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public abstract class ChessParser
    {
        // common storage (following UCI protocol mostly)
        public struct Data
        {
            public uint depth;
            public uint ms;
            public uint eval;
            public uint hashhits;
            public uint nodes;
            public uint nps;
            public string pv;
            public string bestmove;
            public string pondermove;
        }
        string waitToken = "";
        public List<Data> History = null;
        public EventHandler CallbackOnBestmove = null;

        public ChessParser() { }

        public void NewSearch(string waitToken)
        {
            if (History == null) History = new List<Data>();
            this.waitToken = waitToken;
            History.Clear();
        }
        public string SearchBestMove()
        {
            if (History == null || History.Count <= 0) return "";
            string bestmove = "";
            foreach (Data node in History)
            {
                if (node.bestmove != null && node.bestmove != "") bestmove = node.bestmove;
            }
            return bestmove == null ? "" : bestmove;

        }
        public float BranchingFactor()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }
        public float AverageHashHits()
        {
            if (History == null || History.Count < 2) return -1;
            float avgHashHits = 0; float count = 0;
            foreach (Data node in History)
            {
                avgHashHits += node.hashhits; ++count;
            }
            return (count > 0 ? avgHashHits / count : -1);
        }
        public float AverageNPS()
        {
            if (History == null || History.Count < 2) return -1;
            float avgNPS = 0; float count = 0;
            foreach(Data node in History)
            {
                avgNPS += node.nps; ++count;
            }
            return (count > 0 ? avgNPS / count : -1);
        }
        public float AverageNodesPerDepth()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }
        public float EvalVariance()
        {
            if (History == null || History.Count < 2) return -1;

            float avgEval = 0; float count = 0;
            foreach (Data node in History)
            {
                avgEval += node.eval; ++count;
            }
            avgEval = (count > 0 ? avgEval / count : -1);
            if (avgEval == -1) return -1;
            float sigmaEval = 0;
            foreach (Data node in History)
            {
                sigmaEval += (node.eval - avgEval) * (node.eval - avgEval);
            }
            return (float) Math.Sqrt(sigmaEval / (count - 1));
        }
        public float AverageEval()
        {
            if (History == null || History.Count < 2) return -1;
            float avgEval = 0; float count = 0;
            foreach (Data node in History)
            {
                avgEval += node.eval; ++count;
            }
            return (count > 0 ? avgEval / count : -1);
        }

        public virtual bool ParseLine(string line)
        {
            return true;
        }

        public string ToSan(string move)
        {
            return "";
        }
        public string Square(int s)
        {
            return "";
        }
    }

    public class UCIParser : ChessParser
    {
        public UCIParser()
        {
        }
        public override bool ParseLine(string line)
        {
            if (History == null) History = new List<Data>();
            try
            {
                string[] delimits = new string[] { "\r\n" };
                string[] bufferedLines = line.Split(delimits, StringSplitOptions.None);
                for (int i = 0; i < bufferedLines.Length; ++i)
                {
                    string[] tokens = bufferedLines[i].Split(' ');
                    int skipped = 0; Data d = new Data();
                    for (int j = 0; j < tokens.Length - 1; ++j)
                    {
                        string token = tokens[j];
                        switch (token)
                        {
                            case "cp": d.eval = Convert.ToUInt32(tokens[++j]); break;
                            case "depth": d.depth = Convert.ToUInt32(tokens[++j]); break;
                            case "nodes": d.nodes = Convert.ToUInt32(tokens[++j]); break;
                            case "tbhits": d.hashhits = Convert.ToUInt32(tokens[++j]); break;
                            case "nps": d.nps = Convert.ToUInt32(tokens[++j]); break;
                            case "bestmove": d.bestmove = tokens[++j]; break;
                            default: ++skipped; break;
                        }
                    }
                    if (skipped != tokens.Length - 1) History.Add(d);
                }
                return true;

            }
            catch (Exception e) // todo: ignore these
            {
                Log.WriteLine("..[Parser] exception parsing UCI line : {0}", e.Message);
                return false;
            }
        }
    }

    public class WinParser : ChessParser
    {
        public WinParser() { }
        public override bool ParseLine(string line)
        {
            return base.ParseLine(line);
        }
    }
}
