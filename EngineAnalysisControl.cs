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
        AutoResetEvent newEvalAvail = new AutoResetEvent(false); // signal to update Eval changes
        BackgroundWorker update_worker = new BackgroundWorker();
        float dx = 3.3f;
        float prev_eval = 0f;
        float x = 0f;
        bool updating = false;
        float eval = 0f;

        public EngineAnalysisControl()
        {
            InitializeComponent();
            button1.Enabled = false;
            gl.PaintGL += RenderEval;
        }
        public void Initialize(Engine engine, ChessBoard2 b)
        {
            this.e = engine;
            e.AnalysisUICallback += EngineStreamRecieved;
            analysisGroup.Text = e.Name + " analysis";
            this.cb = b;

            update_worker = new BackgroundWorker();
            update_worker.DoWork += new DoWorkEventHandler(update_doWork);
            update_worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(update_Finished);
            update_worker.WorkerReportsProgress = false;
            update_worker.WorkerSupportsCancellation = false;

            update_worker.RunWorkerAsync();
        }
        public void SetEvalDims(int w, int h)
        {
            gl.Width = w;
            gl.Height = h;
        }
        public void enableGoClick(bool en)
        {
            this.button1.Enabled = en;
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


            if (!updating)
            {
                Eval = (float)d.evals[d.evals.Count - 1]; // should be scaled -100 to 100 or so..
                newEvalAvail.Set();
            }
            //cb.UpdateAnalysisGraph(d.evals);
            //updateEvalGraph(d.evals);
        }

        void update_doWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                newEvalAvail.WaitOne();
                updating = true;
                List<float> vertices = new List<float> (Vertices);
                foreach (float v in vertices)
                {
                    x = v;
                    gl.SafeInvalidate(true);
                }
                newEvalAvail.Reset();
                updating = false;
            }
        }
        void update_Finished(object sender, RunWorkerCompletedEventArgs e)
        {
            Log.WriteLine("..closing eval update thread");
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
        float Eval
        {
            get
            {
                return eval;
            }
            set
            {
                float tmp = (value < -5f ? 0 : value > 5f ? 1.0f : value / 10.0f + 0.5f); // map to [0, 1] range
                eval = -gl.Width * 0.5f + gl.Width * tmp; // map to [-width/2, width/2]
            }
        }
        List<float> Vertices
        {
            get
            {
                List<float> res = new List<float>();
                float target = Eval;

                // abort if the eval change is < 10% of the width of the GL graphic
                float percent_change = (target - prev_eval) / gl.Width * 100f;
                if (Math.Abs(percent_change) <= 5) return res;

                float curr_pos = 0;
                float dist_left = target - curr_pos;
                float total_dist = dist_left;
                dx = 10f;

                int j = 1;
                while(Math.Abs(dist_left) > 1.1f * dx && j < 250)
                {
                    curr_pos += (total_dist < 0 ? -dx : dx);
                    dist_left = target - curr_pos;
                    res.Add(curr_pos);
                    ++j;
                }
                prev_eval = target;
                return res;
            }
        }
        void RenderEval()
        {
            GL.MatrixMode(GL.PROJECTION);
            GL.LoadIdentity();
            GL.Ortho(0, gl.Width, gl.Height, 0, 0, 1);
            GL.MatrixMode(GL.MODELVIEW);
            GL.Viewport(0, 0, gl.Width, gl.Height);
            GL.Clear(GL.DEPTH_BUFFER_BIT | GL.COLOR_BUFFER_BIT);
            GL.LoadIdentity();
            GL.ClearColor(0f, 0f, 0f, 1f);

            // draw background
            GL.Color3f(0.6f, 0.6f, 0.6f);

            GL.Begin(GL.QUADS);
            GL.Vertex2f(0, 0);
            GL.Vertex2f(gl.Width, 0);
            GL.Vertex2f(gl.Width, gl.Height);
            GL.Vertex2f(0, gl.Height);
            GL.End();
            
            {
                GL.Begin(GL.QUADS);
                GL.ClearColor(1f, 1f, 1f, 1f);
                GL.Color3f(0.2f, 0.2f, 0.2f);

                GL.Vertex2f(0.5f * gl.Width, 0);

                GL.Color3f((x < 0f ? 0.01f + Math.Abs(x / 100.0f) : 0), (x >= 0 ? 0.01f + Math.Abs(x / 100.0f) : 0), 0.01f);

                GL.Vertex2f(0.5f * gl.Width + x, 0);

                GL.Color3f((x < 0f ? 0.01f + Math.Abs(x / 100.0f) : 0), (x >= 0 ? 0.01f + Math.Abs(x / 100.0f) : 0), 0.01f);

                GL.Vertex2f(0.5f * gl.Width + x, gl.Height);
                GL.Vertex2f(0.5f * gl.Width, gl.Height);

                GL.End();
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
