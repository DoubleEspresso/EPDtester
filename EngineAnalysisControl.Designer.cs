namespace epdTester
{
    partial class EngineAnalysisControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.analysisGroup = new System.Windows.Forms.GroupBox();
            this.chessPlot1 = new epdTester.ChessPlot();
            this.hashfull_box = new System.Windows.Forms.TextBox();
            this.hashfull_lbl = new System.Windows.Forms.Label();
            this.time_box = new System.Windows.Forms.TextBox();
            this.time_lbl = new System.Windows.Forms.Label();
            this.branch_box = new System.Windows.Forms.TextBox();
            this.branch_lbl = new System.Windows.Forms.Label();
            this.mem_box = new System.Windows.Forms.TextBox();
            this.mem_lbl = new System.Windows.Forms.Label();
            this.cpu_box = new System.Windows.Forms.TextBox();
            this.cpu_lbl = new System.Windows.Forms.Label();
            this.hashhits_box = new System.Windows.Forms.TextBox();
            this.hashhits_lbl = new System.Windows.Forms.Label();
            this.move_box = new System.Windows.Forms.TextBox();
            this.eval_box = new System.Windows.Forms.TextBox();
            this.depth_box = new System.Windows.Forms.TextBox();
            this.nps_box = new System.Windows.Forms.TextBox();
            this.eval_lbl = new System.Windows.Forms.Label();
            this.move_lbl = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.depth_lbl = new System.Windows.Forms.Label();
            this.nps_lbl = new System.Windows.Forms.Label();
            this.analysisGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // analysisGroup
            // 
            this.analysisGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.analysisGroup.Controls.Add(this.chessPlot1);
            this.analysisGroup.Controls.Add(this.hashfull_box);
            this.analysisGroup.Controls.Add(this.hashfull_lbl);
            this.analysisGroup.Controls.Add(this.time_box);
            this.analysisGroup.Controls.Add(this.time_lbl);
            this.analysisGroup.Controls.Add(this.branch_box);
            this.analysisGroup.Controls.Add(this.branch_lbl);
            this.analysisGroup.Controls.Add(this.mem_box);
            this.analysisGroup.Controls.Add(this.mem_lbl);
            this.analysisGroup.Controls.Add(this.cpu_box);
            this.analysisGroup.Controls.Add(this.cpu_lbl);
            this.analysisGroup.Controls.Add(this.hashhits_box);
            this.analysisGroup.Controls.Add(this.hashhits_lbl);
            this.analysisGroup.Controls.Add(this.move_box);
            this.analysisGroup.Controls.Add(this.eval_box);
            this.analysisGroup.Controls.Add(this.depth_box);
            this.analysisGroup.Controls.Add(this.nps_box);
            this.analysisGroup.Controls.Add(this.eval_lbl);
            this.analysisGroup.Controls.Add(this.move_lbl);
            this.analysisGroup.Controls.Add(this.button1);
            this.analysisGroup.Controls.Add(this.depth_lbl);
            this.analysisGroup.Controls.Add(this.nps_lbl);
            this.analysisGroup.Location = new System.Drawing.Point(3, 3);
            this.analysisGroup.Name = "analysisGroup";
            this.analysisGroup.Size = new System.Drawing.Size(516, 157);
            this.analysisGroup.TabIndex = 0;
            this.analysisGroup.TabStop = false;
            this.analysisGroup.Text = "engine";
            // 
            // chessPlot1
            // 
            this.chessPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chessPlot1.Location = new System.Drawing.Point(256, 38);
            this.chessPlot1.Name = "chessPlot1";
            this.chessPlot1.Size = new System.Drawing.Size(254, 107);
            this.chessPlot1.TabIndex = 27;
            // 
            // hashfull_box
            // 
            this.hashfull_box.Enabled = false;
            this.hashfull_box.Location = new System.Drawing.Point(143, 23);
            this.hashfull_box.Name = "hashfull_box";
            this.hashfull_box.Size = new System.Drawing.Size(58, 20);
            this.hashfull_box.TabIndex = 26;
            // 
            // hashfull_lbl
            // 
            this.hashfull_lbl.AutoSize = true;
            this.hashfull_lbl.Location = new System.Drawing.Point(207, 30);
            this.hashfull_lbl.Name = "hashfull_lbl";
            this.hashfull_lbl.Size = new System.Drawing.Size(43, 13);
            this.hashfull_lbl.TabIndex = 25;
            this.hashfull_lbl.Text = "hashfull";
            // 
            // time_box
            // 
            this.time_box.Enabled = false;
            this.time_box.Location = new System.Drawing.Point(143, 50);
            this.time_box.Name = "time_box";
            this.time_box.Size = new System.Drawing.Size(58, 20);
            this.time_box.TabIndex = 24;
            // 
            // time_lbl
            // 
            this.time_lbl.AutoSize = true;
            this.time_lbl.Location = new System.Drawing.Point(207, 57);
            this.time_lbl.Name = "time_lbl";
            this.time_lbl.Size = new System.Drawing.Size(26, 13);
            this.time_lbl.TabIndex = 23;
            this.time_lbl.Text = "time";
            // 
            // branch_box
            // 
            this.branch_box.Enabled = false;
            this.branch_box.Location = new System.Drawing.Point(143, 76);
            this.branch_box.Name = "branch_box";
            this.branch_box.Size = new System.Drawing.Size(58, 20);
            this.branch_box.TabIndex = 22;
            // 
            // branch_lbl
            // 
            this.branch_lbl.AutoSize = true;
            this.branch_lbl.Location = new System.Drawing.Point(207, 83);
            this.branch_lbl.Name = "branch_lbl";
            this.branch_lbl.Size = new System.Drawing.Size(40, 13);
            this.branch_lbl.TabIndex = 21;
            this.branch_lbl.Text = "branch";
            // 
            // mem_box
            // 
            this.mem_box.Enabled = false;
            this.mem_box.Location = new System.Drawing.Point(143, 128);
            this.mem_box.Name = "mem_box";
            this.mem_box.Size = new System.Drawing.Size(58, 20);
            this.mem_box.TabIndex = 20;
            // 
            // mem_lbl
            // 
            this.mem_lbl.AutoSize = true;
            this.mem_lbl.Location = new System.Drawing.Point(207, 135);
            this.mem_lbl.Name = "mem_lbl";
            this.mem_lbl.Size = new System.Drawing.Size(29, 13);
            this.mem_lbl.TabIndex = 19;
            this.mem_lbl.Text = "mem";
            // 
            // cpu_box
            // 
            this.cpu_box.Enabled = false;
            this.cpu_box.Location = new System.Drawing.Point(143, 102);
            this.cpu_box.Name = "cpu_box";
            this.cpu_box.Size = new System.Drawing.Size(58, 20);
            this.cpu_box.TabIndex = 18;
            // 
            // cpu_lbl
            // 
            this.cpu_lbl.AutoSize = true;
            this.cpu_lbl.Location = new System.Drawing.Point(207, 109);
            this.cpu_lbl.Name = "cpu_lbl";
            this.cpu_lbl.Size = new System.Drawing.Size(25, 13);
            this.cpu_lbl.TabIndex = 17;
            this.cpu_lbl.Text = "cpu";
            // 
            // hashhits_box
            // 
            this.hashhits_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.hashhits_box.Enabled = false;
            this.hashhits_box.Location = new System.Drawing.Point(9, 126);
            this.hashhits_box.Name = "hashhits_box";
            this.hashhits_box.Size = new System.Drawing.Size(58, 20);
            this.hashhits_box.TabIndex = 16;
            // 
            // hashhits_lbl
            // 
            this.hashhits_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.hashhits_lbl.AutoSize = true;
            this.hashhits_lbl.Location = new System.Drawing.Point(73, 133);
            this.hashhits_lbl.Name = "hashhits_lbl";
            this.hashhits_lbl.Size = new System.Drawing.Size(49, 13);
            this.hashhits_lbl.TabIndex = 15;
            this.hashhits_lbl.Text = "hash hits";
            // 
            // move_box
            // 
            this.move_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.move_box.Enabled = false;
            this.move_box.Location = new System.Drawing.Point(9, 25);
            this.move_box.Name = "move_box";
            this.move_box.Size = new System.Drawing.Size(58, 20);
            this.move_box.TabIndex = 14;
            // 
            // eval_box
            // 
            this.eval_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.eval_box.Enabled = false;
            this.eval_box.Location = new System.Drawing.Point(9, 51);
            this.eval_box.Name = "eval_box";
            this.eval_box.Size = new System.Drawing.Size(58, 20);
            this.eval_box.TabIndex = 13;
            // 
            // depth_box
            // 
            this.depth_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.depth_box.Enabled = false;
            this.depth_box.Location = new System.Drawing.Point(9, 76);
            this.depth_box.Name = "depth_box";
            this.depth_box.Size = new System.Drawing.Size(58, 20);
            this.depth_box.TabIndex = 12;
            // 
            // nps_box
            // 
            this.nps_box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nps_box.Enabled = false;
            this.nps_box.Location = new System.Drawing.Point(9, 100);
            this.nps_box.Name = "nps_box";
            this.nps_box.Size = new System.Drawing.Size(58, 20);
            this.nps_box.TabIndex = 11;
            // 
            // eval_lbl
            // 
            this.eval_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.eval_lbl.AutoSize = true;
            this.eval_lbl.Location = new System.Drawing.Point(73, 58);
            this.eval_lbl.Name = "eval_lbl";
            this.eval_lbl.Size = new System.Drawing.Size(27, 13);
            this.eval_lbl.TabIndex = 10;
            this.eval_lbl.Text = "eval";
            // 
            // move_lbl
            // 
            this.move_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.move_lbl.AutoSize = true;
            this.move_lbl.Location = new System.Drawing.Point(73, 32);
            this.move_lbl.Name = "move_lbl";
            this.move_lbl.Size = new System.Drawing.Size(33, 13);
            this.move_lbl.TabIndex = 9;
            this.move_lbl.Text = "move";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(477, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // depth_lbl
            // 
            this.depth_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.depth_lbl.AutoSize = true;
            this.depth_lbl.Location = new System.Drawing.Point(73, 83);
            this.depth_lbl.Name = "depth_lbl";
            this.depth_lbl.Size = new System.Drawing.Size(34, 13);
            this.depth_lbl.TabIndex = 6;
            this.depth_lbl.Text = "depth";
            // 
            // nps_lbl
            // 
            this.nps_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nps_lbl.AutoSize = true;
            this.nps_lbl.Location = new System.Drawing.Point(73, 107);
            this.nps_lbl.Name = "nps_lbl";
            this.nps_lbl.Size = new System.Drawing.Size(24, 13);
            this.nps_lbl.TabIndex = 1;
            this.nps_lbl.Text = "nps";
            // 
            // EngineAnalysisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.analysisGroup);
            this.Name = "EngineAnalysisControl";
            this.Size = new System.Drawing.Size(522, 163);
            this.analysisGroup.ResumeLayout(false);
            this.analysisGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox analysisGroup;
        private System.Windows.Forms.Label depth_lbl;
        private System.Windows.Forms.Label nps_lbl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox time_box;
        private System.Windows.Forms.Label time_lbl;
        private System.Windows.Forms.TextBox branch_box;
        private System.Windows.Forms.Label branch_lbl;
        private System.Windows.Forms.TextBox mem_box;
        private System.Windows.Forms.Label mem_lbl;
        private System.Windows.Forms.TextBox cpu_box;
        private System.Windows.Forms.Label cpu_lbl;
        private System.Windows.Forms.TextBox hashhits_box;
        private System.Windows.Forms.Label hashhits_lbl;
        private System.Windows.Forms.TextBox move_box;
        private System.Windows.Forms.TextBox eval_box;
        private System.Windows.Forms.TextBox depth_box;
        private System.Windows.Forms.TextBox nps_box;
        private System.Windows.Forms.Label eval_lbl;
        private System.Windows.Forms.Label move_lbl;
        private ChessPlot chessPlot1;
        private System.Windows.Forms.TextBox hashfull_box;
        private System.Windows.Forms.Label hashfull_lbl;
    }
}
