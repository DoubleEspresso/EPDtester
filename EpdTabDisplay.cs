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
    public partial class EpdTabDisplay : UserControl
    {
        List<EpdTestTab> testViews = null;
        List<TabPage> Tabs = new List<TabPage>();
        public EpdTabDisplay()
        {
            if (testViews == null) testViews = new List<EpdTestTab>();
            InitializeComponent();
        }
        public void AddTest(EpdFile epdfile)
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

            if (tab_added)
            {
                if (selected == null)
                {
                    foreach (TabPage tab in Tabs)
                    {
                        if (tab.Text == view.Name)
                        {
                            selected = tab;
                            break;
                        }
                    }
                    if (selected != null) tabControl.SelectedTab = selected; // make sure by default we display the last added test-tab
                }
                Invalidate(true);
            }
        }
        public class EpdTestTab : ListView
        {
            EpdFile epdfile = null;
            public string DisplayName = null;
            bool loaded = false;
            public EpdTestTab()
            {
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
                Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Location = new System.Drawing.Point(0, 0);
                Size = new System.Drawing.Size(487, 276);
                TabIndex = 0;
                Text = "";
                View = View.Details;
                
                Columns.Add("position", 256);
                Columns.Add("best move", 16);
                Columns.Add("test-id", 256);
            }
            public void update()
            {
                
            }
            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if (Visible) update();
            }
            void Load()
            {
                foreach (EpdFile.EPDPosition p in epdfile.Positions) Items.Add(new ListViewItem(new string[] { p.fen, p.bm, p.id }));
                
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
        }
    }
}
