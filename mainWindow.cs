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
        List<Engine> engines = new List<Engine>();
        List<EpdTest> tests = new List<EpdTest>();

        public mainWindow()
        {
            InitializeComponent();
            LoadEnginesFromSettings();
            
            //addListViewItem(new string[] { "title1", "title2", "title3" });
        }

        private void addListViewItem(string[] row)
        {
            listView1.Items.Add(new ListViewItem(row));
        }


        /*engine management*/
        private void LoadEnginesFromSettings()
        {
            int count = 0; Settings.get("Engine\\Count", ref count); if (count == 0) return;
            for (int j=0; j< count; ++j)
            {
                Engine e = new Engine();
                if (e.ReadSettings(j))
                {
                    engines.Add(e);
                    AddEngineToUI(e);
                }
            }
        }
        private void engineAddClick(object sender, EventArgs ea)
        {
            OpenFileDialog ofd = new OpenFileDialog(); ofd.DefaultExt = ".exe";
            if (ofd.ShowDialog() == DialogResult.OK && 
                !string.IsNullOrWhiteSpace(ofd.FileName) &&
                File.Exists(ofd.FileName))
            {
                Engine e = new Engine();
                e.FileName = ofd.FileName;
                if (e.canOpen())
                {
                    engines.Add(e);
                    AddEngineToUI(e);
                    e.SaveSettings(engines.Count-1);
                    Settings.set("Engine\\Count", engines.Count);
                    Settings.set("Engine\\Selected", engines.Count);
                }
                else
                {
                    Log.WriteLine("..failed to open engine @{0}, not added to engine list", e.FileName);
                }
            }
        }
        private void AddEngineToUI(Engine e)
        {
            if (String.IsNullOrWhiteSpace(e.Name) && !String.IsNullOrWhiteSpace(e.FileName))
            {
                e.Name = StringUtils.RemoveExtension(StringUtils.BaseName(e.FileName));
            }
            else
            {
                Log.WriteLine("..[engine] attempt to add engine to UI failed, abort.");
                return;
            }
            if (isDuplicateEngine(e))
            {
                Log.WriteLine("..[engine] duplicate engine already in list, ignored.");
                return;
            }
            // textbox update
            engineList.Items.Add(e.Name);
        }
        bool isDuplicateEngine(Engine e)
        {
            return false;
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

        private void findOpeningBook_Click(object sender, EventArgs e)
        {

        }

        private void findEndgameBook_Click(object sender, EventArgs e)
        {

        }

        private void findConfigFile_Click(object sender, EventArgs e)
        {

        }

        private void updatePath_Click(object sender, EventArgs e)
        {

        }
    }
}
