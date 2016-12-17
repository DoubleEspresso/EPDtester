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
        public EngineAnalysisControl()
        {
            InitializeComponent();
        }
        public void setEngine(Engine engine)
        {
            this.e = engine;
            e.ReadoutCallback += EngineStreamRecieved;
        }
        public void Clear()
        {
            engineAnalysis.Text = "";
        }
        public void EngineStreamRecieved(object sender, string s)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<string>(EngineStreamRecieved), sender, s);
                return;
            }
            engineAnalysis.Text += s + "\n";
        }
    }
}
