using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class EngineAnalysisControl : UserControl
    {
        Engine e = null;
        ChessBoard cb = null;
        public EngineAnalysisControl()
        {
            InitializeComponent();
        }
        public void Initialize(Engine engine, ChessBoard b)
        {
            this.e = engine;
            e.AnalysisUICallback += EngineStreamRecieved;
            analysisGroup.Text = e.Name + " analysis";
            this.cb = b;
        }
        public void Clear()
        {
            depth.Text = "";
            eval.Text = "";
            nps.Text = "";
            hashfull.Text = "";
            currmove.Text = "";
            cpu.Text = "";
            pv.Text = "";
        }
        public void EngineStreamRecieved(object sender, Engine.AnalysisUIData d)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<Engine.AnalysisUIData>(EngineStreamRecieved), sender,d);
                return;
            }
            depth.Text = d.depth;
            eval.Text = d.eval;
            nps.Text = d.nps;
            hashfull.Text = d.hashfull;
            currmove.Text = d.currmove;
            cpu.Text = d.cpu;
            pv.Text = d.pv;
        }
        
    }
}
