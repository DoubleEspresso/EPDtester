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
                Invoke(new EventHandler<Engine.AnalysisUIData>(EngineStreamRecieved), sender, d);
                return;
            }
            depth.Text = d.depth;
            nps.Text = d.nps;
            hashfull.Text = d.hashfull;
            currmove.Text = d.currmove;
            cpu.Text = d.cpu;
            pv.Text = d.pv;
            //cb.UpdateAnalysisGraph(d.evals);
            //updateEvalGraph(d.evals);
        }
        //private void updateEvalGraph(List<double> evals)
        //{

        //    evalGraph.ForeColor = Color.Green;
        //    if (evals == null || evals.Count <= 0) return;
        //    for (int j = 0; j < evals.Count; ++j)
        //    {
        //        int v = (int)(100 * evals[j]);
        //        if (v > 100) v = 100;
        //        if (v < 0)
        //        {
        //            evalGraph.ForeColor = Color.Red;
        //            v = -v;
        //        }
        //        evalGraph.Value = v;
        //    }
        //}
    }
}
