using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class LogViewer : Form
    {
        List<LogTextbox> logViews = new List<LogTextbox>();
        List<TabPage> Tabs = new List<TabPage>();
        List<Engine> monitoredEngines = new List<Engine>();

        public LogViewer(List<Engine> engines)
        {
            InitializeComponent();
            checkNewLogs(engines);
        }
        private bool isMonitored(string filename)
        {
            foreach (LogTextbox view in logViews) if (view.Filename == filename) return true;
            return false;
        }
        private string SubLogName(string filename)
        {
            if (filename == null) return null;
            filename = StringUtils.BaseName(filename);
            string result = filename.Replace(Log.LogName, "").Replace(".log", "");
            if (result.StartsWith("-")) result = result.Substring(1, result.Length - 1);
            int idx = result.LastIndexOf("-");
            if (idx > 0) result = result.Substring(0, idx);
            if (string.IsNullOrWhiteSpace(result)) return null;
            return result;
        }
        private void checkNewLogs(List<Engine> engines)
        {
            List<string> log_files = new List<string>(Directory.GetFiles(Log.DirectoryName, string.Format("{0}*.log", Log.LogName))); // main log
            monitoredEngines.Clear();
            foreach (Engine e in engines)
            {
                if (e.EngineLogFilename == null) continue;
                List<string> engineLogs = new List<string>(Directory.GetFiles(Log.DirectoryName, e.EngineLogFilename));
                log_files.AddRange(engineLogs);
                monitoredEngines.Add(e);
            }
            if (log_files.Count == 0) return;
            if (log_files.Count == logViews.Count) return; // no new file ...

            TabPage selected = (tabControl.Controls.Count > 0 ? tabControl.SelectedTab : null);
            bool tab_added = false;
            foreach (string lf in log_files)
            {
                if (isMonitored(lf)) continue;

                LogTextbox view = new LogTextbox();
                view.Filename = lf;

                logViews.Add(view);

                string tn = SubLogName(lf);
                if (string.IsNullOrWhiteSpace(tn)) tn = "Main";
                TabPage lt = new TabPage(tn);
                lt.Location = new System.Drawing.Point(4, 22);
                lt.Padding = new System.Windows.Forms.Padding(3);
                lt.Size = new System.Drawing.Size(488, 277);
                lt.Size = new System.Drawing.Size(487, 276);
                lt.TabIndex = 0;
                lt.UseVisualStyleBackColor = true;
                lt.Controls.Add(view);
                tabControl.Controls.Add(lt);
                Tabs.Add(lt);
                tab_added = true;
            }
            if (tab_added)
            {
                if (selected == null)
                {
                    foreach (TabPage tab in Tabs)
                    {
                        if (tab.Text == "Main")
                        {
                            selected = tab;
                            break;
                        }
                    }
                    if (selected != null) tabControl.SelectedTab = selected; // make sure by default we display the "Main" tab
                }
                Invalidate(true);
            }
        }
        List<string> CommandHistory = new List<string>();
        int lastCommandIdx = -1;
        void textChanged(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter: issueEngineCommand(); e.Handled = true;  break;
                case Keys.Up: updateHistoryIdx(-1); e.Handled = true; break;
                case Keys.Down: updateHistoryIdx(1); e.Handled = true; break;
            }
        }
        void updateHistoryIdx(int d)
        {
            int newidx = lastCommandIdx + d;
            if (newidx < 0) lastCommandIdx = CommandHistory.Count - 1;
            else if (newidx > CommandHistory.Count - 1) lastCommandIdx = 0;

            if (newidx >= 0 && newidx < CommandHistory.Count - 1)
                commandText.Text = CommandHistory[newidx];
        }
        void issueEngineCommand()
        {
            try
            {
                int idx = tabControl.SelectedIndex;
                if (idx < 0 || idx > logViews.Count) return;
                LogTextbox current = logViews[idx];
                foreach (Engine engine in monitoredEngines)
                {
                    if (current.Filename.Contains(engine.EngineLogFilename))
                    {
                        // shorten the refresh rate
                        current.setRefreshRate(100);
                        engine.Command(commandText.Text);
                        CommandHistory.Add(commandText.Text);
                        ++lastCommandIdx;
                        commandText.Text = "";
                    }

                }
            }
            catch { } // ignore failed engine command attempts
        }
        public class LogTextbox : RichTextBox
        {
            Timer refresh = null;
            long last_displayed = 0;
            string filename = null;
            int last_displayed_line = 0;
            bool keepAtEnd = true;
            public LogTextbox()
            {
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
                DetectUrls = false;
                Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Location = new System.Drawing.Point(0, 0);
                ReadOnly = true;
                Size = new System.Drawing.Size(487, 276);
                TabIndex = 0;
                Text = "";
                WordWrap = false;
                ScrollBars = RichTextBoxScrollBars.Both;
            }
            public void update()
            {
                updateView();
            }
            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if (Visible) update();
                else suspend();
            }
            public void suspend()
            {
                refresh.Stop();
            }
            public void setRefreshRate(int r)
            {
                refresh.Interval = r;
            }
            public string Filename
            {
                get
                {
                    return filename;
                }
                set
                {
                    filename = value;
                    this.HideSelection = true;
                    refresh = new System.Windows.Forms.Timer();
                    refresh.Interval = 1500;
                    refresh.Tick += new EventHandler(refreshEvent);
                    keepAtEnd = true;
                }
            }
            void refreshEvent(object sender, EventArgs e)
            {
                refresh.Stop();
                updateView();
            }
            delegate void VoidFunc();
            void updateView()
            {
                refresh.Stop();
                if (this.InvokeRequired)
                {
                    try
                    {
                        if (!this.Disposing && !this.IsDisposed)
                        {
                            this.Invoke(new VoidFunc(updateView), null);
                        }
                    }
                    catch (Exception) { }
                    return;
                }

                if (!Visible) { refresh.Stop(); return; }
                if (File.Exists(filename))
                {
                    FileInfo nfo = new FileInfo(filename);
                    long flen = nfo.Length;
                    if (flen <= last_displayed)
                    {
                        //if (checkNewSublogs != null) checkNewSublogs();
                        refresh.Start();
                        refresh.Enabled = true; // no modified .. check again
                        return;
                    }
                    // update display
                    // last_display = DateTime.Now;
                    last_displayed = flen;
                    try
                    {
                        int prev_selection = SelectionStart;
                        int prev_selection_len = SelectionLength;


                        using (FileStream fsin = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                        using (StreamReader tr = new StreamReader(fsin))
                        {
                            for (int i = 0; i < last_displayed_line; ++i)
                            {
                                // skip already displayed lines
                                tr.ReadLine();
                            }
                            while (!tr.EndOfStream)
                            {
                                AppendText(tr.ReadLine() + "\n");
                                ++last_displayed_line;
                            }
                        }

                        if (keepAtEnd)
                        {
                            base.SelectionStart = base.Text.Length - 1;
                            base.SelectionLength = 1;
                            base.ScrollToCaret();
                        }

                    }
                    catch (Exception any)
                    {
                        Log.WriteLine("LogDialog Exception: {0}", any.Message);
                    }
                }
                //if (checkNewSublogs != null) checkNewSublogs();
                refresh.Start();
                refresh.Enabled = true;
            }
        }

        private void openLogFolder_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Log.DirectoryName);
            }
            catch (Exception) { }
        }
    }
}
