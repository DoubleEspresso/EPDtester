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
        List<EpdFile> tests = new List<EpdFile>();
        public Engine ActiveEngine = null;
        List<string> epd_filenames = new List<string>(); // for epd test selection
        public mainWindow()
        {
            InitializeComponent();
            LoadEnginesFromSettings();
            LoadEPDSettings();
        }
        /*EPD file management*/
        private void LoadEPDSettings()
        {
            string tmp = "";
            Settings.get(string.Format("EPD\\Base directory"), ref tmp);            
            try
            {
                if (Directory.Exists(tmp))
                {
                    epd_filenames = new List<string>(Directory.GetFiles(tmp, "*.epd"));
                    foreach (string file in epd_filenames)
                    {
                        epdCombobox.Items.Add(StringUtils.RemoveExtension(StringUtils.BaseName(file)));
                    }
                    epdDirLabel.Text = "directory " + tmp;
                }
                else epdDirLabel.Text = "directory " + "(none selected)";
            }
            catch (Exception any)
            {
                Log.WriteLine("..[epd] exception loading epd-directory ({0}) from settings: {1}", tmp, any.Message);
            }
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
                ActiveEngine = e;
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
        private void setEPDdirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog(); 
            if (fd.ShowDialog() == DialogResult.OK &&
                !string.IsNullOrWhiteSpace(fd.SelectedPath) &&
                Directory.Exists(fd.SelectedPath))
            {
                try
                {
                    epd_filenames = new List<string>();
                    epdDirLabel.Text = "directory " + fd.SelectedPath;
                    epd_filenames = new List<string>(Directory.GetFiles(fd.SelectedPath, "*.epd"));
                    foreach (string file in epd_filenames)
                    {
                        epdCombobox.Items.Add(StringUtils.RemoveExtension(StringUtils.BaseName(file)));
                    }
                    Settings.set(string.Format("EPD\\Base directory"), fd.SelectedPath);
                }
                catch (Exception any)
                {
                    Log.WriteLine("..[epd] exception loading epd-files from directory ({0}), {1}", fd.SelectedPath, any.Message);
                }
            }
        }
        // todo: avoid adding/parsing the same epd file multiple times
        private void epdCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (epdCombobox.SelectedIndex >= 0 && epdCombobox.SelectedIndex < epd_filenames.Count)
            {
                tests.Add(new EpdFile(epd_filenames[epdCombobox.SelectedIndex]));
                epdTabDisplay.AddTest(tests[tests.Count - 1], this); // last added

            }
        }
        public void SendEngineCommand(object sender, string s)
        {
            if (ActiveEngine == null)
            {
                MessageBox.Show("..please load a chess engine and try again.");
                return;
            }
            ActiveEngine.Command(s); // multiple lines ok?
        }
    }
}
