﻿using System;
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
        static List<Engine> engines = new List<Engine>(); // static so we can delete them on application exit.
        List<EpdFile> tests = new List<EpdFile>();
        public Engine ActiveEngine = null;
        public ChessBoard2 board2 = null;
        List<string> epd_filenames = new List<string>(); // for epd test selection
        public MoveList MoveList { get { return moveList; } }

        public mainWindow()
        {
            InitializeComponent();
            LoadEnginesFromSettings();
            LoadEPDSettings();
            EPDTestProgress_label.Text = "";
            EPDTestCorrect_label.Text = "";
            label7.Text = "";
            
            board2 = new ChessBoard2(null, cboard, gl_eval, this);
            moveList.SetBoard(board2);
        }
        protected override void OnLoad(EventArgs e)
        {
            Width = 1400;
            Height = 800;
            ResizeFinished(null, null);
        }
        private void ResizeFinished(object sender, EventArgs e)
        {
            SetAspectRatio(Width, Height);
        }
        private void SetAspectRatio(int w, int h)
        {
            w -= tabControl1.Width;
            float mind = (float)Math.Min(w, h);
            float nw = (w >= mind ? mind : w);
            float nh = (h >= mind ? mind : h);
            if (board2 != null) board2.SetDims((int)nw-40, (int)nh-65);
            Width = (int)(nw + tabControl1.Width); Height = (int)nh;
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
            for (int j = 0; j < count; ++j)
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
                    e.SaveSettings(engines.Count - 1);
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
            engineList.SelectedIndex = engineList.Items.Count - 1; // so one is selected by default.
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

            if (e.Running) status.Text = string.Format("Status: {0} currently running", e.Name);
            else status.Text = string.Format("Status: {0} not running", e.Name);
        }
        bool isDuplicateEngine(Engine e)
        {
            return false;
        }
        private void engineDeleteClick(object sender, EventArgs e)
        {

        }
        delegate void EngineSelectedChangeFunc(object sender, EventArgs args);
        private void engineList_SelectedEngineChanged(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new EngineSelectedChangeFunc(engineList_SelectedEngineChanged), sender, args);
                return;
            }
            int idx = engineList.SelectedIndex;
            if (idx >= 0 && idx < engines.Count)
            {
                Engine e = engines[idx];
                updateDisplay(e);
                Settings.set("Engine\\Selected", idx);
                ActiveEngine = e;
                this.useLogCheckbox.Checked = (e.WritingToLogFile());
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
            // close any active engines connected to the main board
            if (board2 != null) board2.CloseEngine();
            status.Text = "";

            int idx = engineList.SelectedIndex;
            if (idx >= 0 && idx < engines.Count)
            {
                Engine e = engines[idx];
                if (e.Running)
                {
                    Log.WriteLine("..[engine] {0} already running, ignored", e.Name);
                    return;
                }
                if (!e.Start(eCommandBox.Text))
                {
                    Log.WriteLine("..WARNING: enigne-{0} failed to start!", e.Name);
                }
                else if (board2 != null)
                {
                    board2.SetEngine(e);

                    // note: take engine out of "game" mode so it does not try to stop
                    // on best-move parsing (causes illegal engine moves/crashes)
                    // todo : if we toggle this option, we need to re-set the callback.
                    board2.ChessEngine.Parser.CallbackOnBestmove = null;

                    engineAnalysisControl.Initialize(board2.ChessEngine, board2);
                   
                    board2.mode = ChessBoard2.Mode.ANALYSIS;
                    engineAnalysisControl.enableGoClick(board2.hasEngine());
                    
                }
                updateDisplay(e);
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
        public void CommandFromEPDThread(object sender, string s)
        {
            if (ActiveEngine == null)
            {
                MessageBox.Show("..please load a chess engine and try again.");
                return;
            }
            ActiveEngine.Command(s); // multiple lines ok?
        }
        delegate void EpdBestMoveParsedFunc(object sender, EventArgs e);
        // fixme
        public void onEPDtestBestMoveParsed(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EpdBestMoveParsedFunc(onEPDtestBestMoveParsed), new object[] { sender, e });
                return;
            }
            Thread.Sleep(100);
            epdTabDisplay.SelectedTest.Grade(ActiveEngine.Parser);
            string progress = Convert.ToString(epdTabDisplay.SelectedTest.activeTestIdx);
            string nbCorrect = Convert.ToString(epdTabDisplay.SelectedTest.correct);
            string totalTests = Convert.ToString(epdTabDisplay.SelectedTest.totalTests);
            label7.Text = "solved correctly ..";
            EPDTestProgress_label.Text = "..searching " + progress + " / " + totalTests + " positions";
            EPDTestCorrect_label.Text = nbCorrect + " / " + totalTests;
            //epdTabDisplay.SelectedTest.BestMoveEvent.Set();
            Log.WriteLine("...set betsmove event");
        }        
        private void epdStart_Click(object sender, EventArgs e)
        {
            // fixme
            ActiveEngine.SetBestMoveCallback(onEPDtestBestMoveParsed);
            epdTabDisplay.SelectedTest.startTest(timePerPosition);
        }
        uint timePerPosition = 1000;
        private void epdFixedTimePosition_TextChanged(object sender, EventArgs e)
        {
            try
            {
                timePerPosition = (uint)Convert.ToInt32(epdFixedTimePosition.Text);
            }
            catch { }
        }
        private void stopEpdTest_Click(object sender, EventArgs e)
        {
            //fixme
            epdTabDisplay.SelectedTest.CancelTest();
        }
        public static void CloseEngineInstances()
        {
            int count = 0; 
            foreach (Engine e in engines)
            {
                e.SaveSettings(count); 
                e.Close();
                ++count;
            }
            Log.WriteLine("..all running engines closed");
        }
        private void useLogCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ActiveEngine.UseLog(useLogCheckbox.Checked);
        }

        private void engineAnalysisControl_Load(object sender, EventArgs e)
        {

        }
    }
}
