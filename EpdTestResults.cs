using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public class EpdTest
    {
        public List<Position> Positions = null;
        public List<Result> EngineResults = null;
        public string Name = null; // classifier, if any
        
        /*entry data for epd test*/
        public struct Position
        {
            public string fen;
            public string bestmove;
        }

        public struct Result
        {                     
            public string enginemove;
            public uint depth;
            public uint time;
            public uint nodes;
            public uint nps;
            public uint hHits;
        }
        
        public EpdTest() { }


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
