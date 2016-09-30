using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            int curr_selected = -1; Settings.get("Engine\\Selected", ref curr_selected);
            if (curr_selected >= 0 && curr_selected < engines.Count)
            {
                engineList.SelectedIndex = curr_selected;
                engineList_SelectedEngineChanged(null, null);
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
            if (isDuplicateEngine(e))
            {
                Log.WriteLine("..[engine] duplicate engine already in list, ignored.");
                return;
            }
            // textbox update
            engineList.Items.Add(e.Name);
            updateDisplay(e);
        }
        void updateDisplay(Engine e)
        {
            eNameBox.Text = e.Name;
            ePathBox.Text = e.FileName;
            eOpeningBookBox.Text = e.OpeningBook;
            eEndgameBookBox.Text = e.TableBase;
            eConfigfileBox.Text = e.ConfigFile;
            eEloBox.Text = e.Elo.ToString();
            authorLabel.Text = "Author: " + e.Author;
            protocolLabel.Text = "Protocol: " + (e.EngineProtocol == Engine.Type.UCI ? "UCI" :
                    e.EngineProtocol == Engine.Type.WINBOARD ? "WinBoard" : "Unknown");
        }

        bool isDuplicateEngine(Engine e)
        {
            return false;
        }
        private void engineDeleteClick(object sender, EventArgs e)
        {

        }
        private void engineList_SelectedEngineChanged(object sender, EventArgs args)
        {
            int idx = engineList.SelectedIndex;
            if (idx >=0 && idx < engines.Count)
            {
                Engine e = engines[idx];
                updateDisplay(e);
                Settings.set("Engine\\Selected", idx);
            }
        }
        private void showLog_Click(object sender, EventArgs e)
        {
            // fixme : opens multiple log views
            LogViewer logViewer = new LogViewer(engines);
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

        private void startEngine_Click(object sender, EventArgs args)
        {
            int idx = engineList.SelectedIndex;
            if (idx >= 0 && idx < engines.Count)
            {
                Engine e = engines[idx];
                if (e.Running)
                {
                    Log.WriteLine("..[engine] {0} already running, ignored", e.Name);
                    return;
                }
                e.Start(eCommandBox.Text);
            }
        }
    }
}
