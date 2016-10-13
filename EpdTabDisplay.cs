using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace epdTester
{
    public partial class EpdTabDisplay : UserControl
    {
        List<EpdTestTab> testViews = null;
        List<TabPage> Tabs = new List<TabPage>();
        public TabPage ActiveTab = null;
        public EpdTestTab SelectedTest = null;
        public EpdTabDisplay()
        {
            if (testViews == null) testViews = new List<EpdTestTab>();
            InitializeComponent();
        }
        public void AddTest(EpdFile epdfile, mainWindow mw)
        {
            foreach (EpdTestTab t in testViews)
            {
                if (t.DisplayName == epdfile.Name)
                {
                    Log.WriteLine("..[epd-file] duplicate epd file selected, ignored.");
                    return;
                }
            }

            TabPage selected = (tabControl.Controls.Count > 0 ? tabControl.SelectedTab : null);
            bool tab_added = false;

            EpdTestTab view = new EpdTestTab();
            view.EpdTest = epdfile;
            testViews.Add(view);

            if (string.IsNullOrWhiteSpace(view.DisplayName)) view.DisplayName = "(Unknown test id)";
            TabPage lt = new TabPage(view.DisplayName);
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
            view.EpdEngineCommand += mw.CommandFromEPDThread;

            if (tab_added)
            {
                if (selected == null)
                {
                    foreach (TabPage tab in Tabs)
                    {
                        if (tab.Text == view.DisplayName)
                        {
                            selected = tab;
                            SelectedTest = view;
                            break;
                        }
                    }
                    if (selected != null)
                    {
                        ActiveTab = tabControl.SelectedTab;
                        tabControl.SelectedTab = selected; // make sure by default we display the last added test-tab
                    }
                }
                Invalidate(true);
            }
        }
        private void SelectedTabChanged(object sender, EventArgs e)
        {
            ActiveTab = tabControl.SelectedTab;
            foreach (EpdTestTab test in testViews)
            {
                if (ActiveTab.Text == test.DisplayName)
                {
                    SelectedTest = test;
                    break;
                }
            }
        }

        public class EpdTestTab : ListView
        {
            BackgroundWorker epdWorkerThread;
            EpdFile epdfile = null;
            public string DisplayName = null;
            bool loaded = false;
            Position position = null;
            uint searchTime = 1000;
            public int totalTests;
            public int activeTestIdx;
            //public float nodesPerPosition;
            public int correct;

            public EpdTestTab()
            {
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
                Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Location = new System.Drawing.Point(0, 0);
                Size = new System.Drawing.Size(487, 276);
                FullRowSelect = true;
                TabIndex = 0;
                Text = "";
                View = View.Details;
                
                Columns.Add("position", 256);
                Columns.Add("best move", 16);
                Columns.Add("test-id", 256);
                Columns.Add("Engine move", 256);

                Click += EpdTestTab_Click;
                MouseDown += new MouseEventHandler(ItemMouseDown);
                epdWorkerThread = new BackgroundWorker();
                epdWorkerThread.WorkerSupportsCancellation = true;
                epdWorkerThread.DoWork += new DoWorkEventHandler(epdWorker_doWork);
                //epdWorkerThread.ProgressChanged += new ProgressChangedEventHandler(epdWorker_ProgressChanged);
                epdWorkerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(epdWorker_RunWorkerCompleted);
                epdWorkerThread.WorkerReportsProgress = false;
            }
            private void EpdTestTab_Click(object sender, EventArgs e)
            {
            }

            // mouse-right-click on fen string
            private void ItemMouseDown(object sender, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Right:
                        {
                            ContextMenu cm = new ContextMenu();
                            cm.MenuItems.Add("Analyze (see log)", AnalyzeEvent);
                            ContextMenu = cm;
                            cm.Show(this, new Point(e.X, e.Y));
                        }
                        break;
                }
            }
            public event EventHandler<string> EpdEngineCommand = null;
            private void AnalyzeEvent(object sender, EventArgs args)
            {
                int i = SelectedIndices[0]; // only the fen string is clickable
                string fen = Items[i].Text;
                string command = "position fen " + fen + "\ngo movetime 15000";
                if (EpdEngineCommand != null) EpdEngineCommand(this, command);
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
            }
            void Load()
            {
                foreach (EpdFile.EPDPosition p in epdfile.Positions) Items.Add(new ListViewItem(new string[] { p.fen, (p.bm != null ? p.bm : p.am != null ? "(avoid)"+p.am : "none"), p.id }));
                foreach (ColumnHeader c in Columns)
                {
                    c.Width = -2;
                }
                loaded = true;
            }
            public EpdFile EpdTest
            {
                get
                {
                    return epdfile;
                }
                set
                {
                    epdfile = value;
                    DisplayName = epdfile.Name;
                    if (!loaded) Load();
                }
            }
            bool movesEqual(string emove, string testMove)
            {
                string[] testmoves = testMove.Split(' ');
                foreach (string testmove in testmoves)
                {
                    if (testmove.Replace(" ", "").ToLower() == emove.ToLower()) return true;
                }
                return false;
            }
            
            // epd test worker
            delegate void GradeFunc(ChessParser cp);
            public void Grade(ChessParser cp)
            {
                try
                {
                    if (cp == null) return;
                    string bestmove = position.toSan(cp.SearchBestMove());
                    if (movesEqual(bestmove.ToLower(), activeTest.SubItems[1].Text))
                    {
                        activeTest.BackColor = Color.LawnGreen;
                        correct += 1;
                    }
                    else
                    {
                        activeTest.BackColor = Color.PaleVioletRed;
                    }
                    activeTest.SubItems.Add( new ListViewItem.ListViewSubItem(activeTest, bestmove));
                }
                catch (Exception e) { Log.WriteLine("..[epd-test] exception parsing bestmove : {0}", e.Message);  }
            }

            public void startTest(uint timePerPosition)
            {
                totalTests = Items.Count;
                correct = 0;
                searchTime = timePerPosition;
                if (epdWorkerThread != null) epdWorkerThread.RunWorkerAsync();
            }
            private void epdWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                try
                {
                    string command = "stop\n";
                    if (EpdEngineCommand != null) EpdEngineCommand(this, command);
                    if (e.Cancelled) MessageBox.Show("EPD test cancelled\n");
                }
                catch { };

            }
            private void epdWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
            }
            public AutoResetEvent BestMoveEvent = new AutoResetEvent(false);
            private delegate void SelectTestFunc(int i);
            void SelectNextTest(int i)
            {
                if (InvokeRequired)
                {
                    Invoke(new SelectTestFunc(SelectNextTest), i);
                    return;
                }
                Items[0].Selected = true;
                activeTest = Items[i];
                activeTest.BackColor = Color.LightGray;
            }
            ListViewItem activeTest = null;
            private void epdWorker_doWork(object sender, DoWorkEventArgs e)
            {
                for (int i=0; i<Items.Count; ++i)
                {
                    activeTestIdx = i + 1;
                    if (epdWorkerThread.CancellationPending) break;
                    SelectNextTest(i);
                    string fen = activeTest.Text;
                    string command = "position fen " + fen + "\ngo movetime " + Convert.ToString(searchTime);
                    position = new Position(fen);
                    if (EpdEngineCommand != null) EpdEngineCommand(this, command);
                    BestMoveEvent.WaitOne();
                    Log.WriteLine("...on to next puzzle");
                }
            }
            public void CancelTest()
            {
                epdWorkerThread.CancelAsync();
            }
        }
    }
}
