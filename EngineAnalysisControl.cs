using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace epdTester
{
    public partial class EngineAnalysisControl : UserControl
    {
        Engine e = null;
        ChessBoard2 cb = null;

        public EngineAnalysisControl()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        public void Initialize(Engine engine, ChessBoard2 b)
        {
            this.e = engine;
            e.AnalysisUICallback += EngineStreamRecieved;
            analysisGroup.Text = e.Name + " analysis";
            this.cb = b;

        }
        public void enableGoClick(bool en)
        {
            this.button1.Enabled = en;
        }
        public void Clear()
        {
            depth_lbl.Text = "";
            nps_lbl.Text = "";
            //hashfull.Text = "";
            //currmove.Text = "";
            //cpu.Text = "";
            //pv.Text = "";
        }
        public void EngineStreamRecieved(object sender, Engine.AnalysisUIData d)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<Engine.AnalysisUIData>(EngineStreamRecieved), sender, d);
                return;
            }
            depth_box.Text = d.depth;
            nps_box.Text = d.nps;
            move_box.Text = d.pv;
            eval_box.Text = string.Format("{0:F3}", d.evals[d.evals.Count - 1]);
            //hashfull.Text = d.hashfull;
            //currmove.Text = d.currmove;
            //cpu.Text = d.cpu;

            if (cb != null)
            {
                float eval = (float)d.evals[d.evals.Count - 1]; // should be scaled -100 to 100 or so..
                cb.updateEval(eval);
            }
            //cb.UpdateAnalysisGraph(d.evals);
            //updateEvalGraph(d.evals);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cb == null) return;
            if (cb.ChessEngine.Thinking)
            {
                cb.ChessEngine.Command("stop");
                button1.Text = "go";
            }
            else
            {
                cb.ChessEngine.Command("go infinite");
                button1.Text = "stop";
            }
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
