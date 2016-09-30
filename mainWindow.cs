using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class mainWindow : Form
    {
        List<Engine> engines = null;
        List<EpdTest> tests = null;

        public mainWindow()
        {
            InitializeComponent();
            if (engines == null) engines = new List<Engine>();

            addListViewItem(new string[] { "title1", "title2", "title3" });
            Log.WriteLine("...");
        }

        private void addListViewItem(string[] row)
        {
            listView1.Items.Add(new ListViewItem(row));
        }

        /*engine management*/
        private void engineAddClick(object sender, EventArgs ea)
        {
            OpenFileDialog ofd = new OpenFileDialog(); ofd.DefaultExt = ".exe";
            if (ofd.ShowDialog() == DialogResult.OK && 
                !string.IsNullOrWhiteSpace(ofd.FileName) &&
                File.Exists(ofd.FileName))
            {
                Engine e = new Engine();
                e.FileName = ofd.FileName;
                if (e.open())
                {
                    engines.Add(e);
                    engines[0].Start();
                    e.Command("position startpos");
                    e.Command("go infinite");
                    System.Threading.Thread.Sleep(3000);
                    e.Command("stop");
                    e.stop();
                    // UI updates
                    // settings updates
                }
                else
                {
                    Log.WriteLine("..failed to open engine @{0}, not added to engine list", e.FileName);
                }
            }
        }
        private void engineDeleteClick(object sender, EventArgs e)
        {

        }

        private void showLog_Click(object sender, EventArgs e)
        {
            LogViewer logViewer = new LogViewer();
            logViewer.Show();
            logViewer.BringToFront();
        }
    }
}
