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
            uint depth;
            uint ms;
            uint eval;
            uint hashhits;
            uint nodes;
            uint nps;
            string pv;
            string bestmove;
            string pondermove;
        }
        public List<Data> History = null;

        public ChessParser() { }

        public void NewSearch()
        {
            if (History == null) History = new List<Data>();
            History.Clear();
        }

        public float BranchingFactor()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }

        public float AverageNPS()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }

        public float AverageNodesPerDepth()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }

        public float EvalVariance()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
        }

        public float EvalMean()
        {
            if (History == null || History.Count < 2) return -1;
            return -1;
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
        public UCIParser() { }
        public override bool ParseLine(string line)
        {
            return base.ParseLine(line);
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
