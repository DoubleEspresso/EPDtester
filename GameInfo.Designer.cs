namespace epdTester
{
    partial class GameInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameInfo));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameCtrlNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameCtrlLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameCtrlSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.evaluationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engineAnalysisItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mvList = new epdTester.MoveList();
            this.Clock = new epdTester.ClockUI();
            this.engineAnalysisControl1 = new epdTester.EngineAnalysisControl();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(488, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameCtrlNToolStripMenuItem,
            this.loadGameCtrlLToolStripMenuItem,
            this.saveGameCtrlSToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newGameCtrlNToolStripMenuItem
            // 
            this.newGameCtrlNToolStripMenuItem.Name = "newGameCtrlNToolStripMenuItem";
            this.newGameCtrlNToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.newGameCtrlNToolStripMenuItem.Text = "New Game (Ctrl + N)";
            // 
            // loadGameCtrlLToolStripMenuItem
            // 
            this.loadGameCtrlLToolStripMenuItem.Name = "loadGameCtrlLToolStripMenuItem";
            this.loadGameCtrlLToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.loadGameCtrlLToolStripMenuItem.Text = "Load Game (Ctrl + L)";
            // 
            // saveGameCtrlSToolStripMenuItem
            // 
            this.saveGameCtrlSToolStripMenuItem.Name = "saveGameCtrlSToolStripMenuItem";
            this.saveGameCtrlSToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveGameCtrlSToolStripMenuItem.Text = "Save Game (Ctrl + S)";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clockToolStripMenuItem,
            this.moveListToolStripMenuItem,
            this.evaluationToolStripMenuItem,
            this.engineAnalysisItem,
            this.databaseToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // clockToolStripMenuItem
            // 
            this.clockToolStripMenuItem.Name = "clockToolStripMenuItem";
            this.clockToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.clockToolStripMenuItem.Text = "Clock";
            // 
            // moveListToolStripMenuItem
            // 
            this.moveListToolStripMenuItem.Name = "moveListToolStripMenuItem";
            this.moveListToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.moveListToolStripMenuItem.Text = "Move List";
            // 
            // evaluationToolStripMenuItem
            // 
            this.evaluationToolStripMenuItem.Name = "evaluationToolStripMenuItem";
            this.evaluationToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.evaluationToolStripMenuItem.Text = "Evaluation";
            // 
            // engineAnalysisItem
            // 
            this.engineAnalysisItem.Name = "engineAnalysisItem";
            this.engineAnalysisItem.Size = new System.Drawing.Size(156, 22);
            this.engineAnalysisItem.Text = "Engine Analysis";
            this.engineAnalysisItem.Click += new System.EventHandler(this.engineAnalysisItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.databaseToolStripMenuItem.Text = "Database";
            // 
            // mvList
            // 
            this.mvList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mvList.Location = new System.Drawing.Point(12, 123);
            this.mvList.Name = "mvList";
            this.mvList.Size = new System.Drawing.Size(464, 138);
            this.mvList.TabIndex = 2;
            // 
            // Clock
            // 
            this.Clock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Clock.Location = new System.Drawing.Point(12, 36);
            this.Clock.Name = "Clock";
            this.Clock.Size = new System.Drawing.Size(465, 81);
            this.Clock.TabIndex = 1;
            // 
            // engineAnalysisControl1
            // 
            this.engineAnalysisControl1.Location = new System.Drawing.Point(12, 267);
            this.engineAnalysisControl1.Name = "engineAnalysisControl1";
            this.engineAnalysisControl1.Size = new System.Drawing.Size(464, 217);
            this.engineAnalysisControl1.TabIndex = 3;
            // 
            // GameInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 496);
            this.Controls.Add(this.engineAnalysisControl1);
            this.Controls.Add(this.mvList);
            this.Controls.Add(this.Clock);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameInfo";
            this.Text = "GameInfo";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameCtrlNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGameCtrlLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGameCtrlSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem evaluationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem engineAnalysisItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private ClockUI Clock;
        private MoveList mvList;
        private EngineAnalysisControl engineAnalysisControl1;
    }
}