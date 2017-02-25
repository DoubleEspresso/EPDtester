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
            this.depth = new System.Windows.Forms.Label();
            this.pv = new System.Windows.Forms.Label();
            this.cpu = new System.Windows.Forms.Label();
            this.currmove = new System.Windows.Forms.Label();
            this.hashfull = new System.Windows.Forms.Label();
            this.nps = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.analysisGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // analysisGroup
            // 
            this.analysisGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.analysisGroup.Controls.Add(this.button1);
            this.analysisGroup.Controls.Add(this.depth);
            this.analysisGroup.Controls.Add(this.pv);
            this.analysisGroup.Controls.Add(this.cpu);
            this.analysisGroup.Controls.Add(this.currmove);
            this.analysisGroup.Controls.Add(this.hashfull);
            this.analysisGroup.Controls.Add(this.nps);
            this.analysisGroup.Location = new System.Drawing.Point(3, 3);
            this.analysisGroup.Name = "analysisGroup";
            this.analysisGroup.Size = new System.Drawing.Size(517, 78);
            this.analysisGroup.TabIndex = 0;
            this.analysisGroup.TabStop = false;
            this.analysisGroup.Text = "engine";
            // 
            // depth
            // 
            this.depth.AutoSize = true;
            this.depth.Location = new System.Drawing.Point(11, 23);
            this.depth.Name = "depth";
            this.depth.Size = new System.Drawing.Size(37, 13);
            this.depth.TabIndex = 6;
            this.depth.Text = "depth:";
            // 
            // pv
            // 
            this.pv.AutoSize = true;
            this.pv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pv.Location = new System.Drawing.Point(11, 50);
            this.pv.Name = "pv";
            this.pv.Size = new System.Drawing.Size(26, 16);
            this.pv.TabIndex = 5;
            this.pv.Text = "pv:";
            // 
            // cpu
            // 
            this.cpu.AutoSize = true;
            this.cpu.Location = new System.Drawing.Point(358, 23);
            this.cpu.Name = "cpu";
            this.cpu.Size = new System.Drawing.Size(28, 13);
            this.cpu.TabIndex = 4;
            this.cpu.Text = "cpu:";
            // 
            // currmove
            // 
            this.currmove.AutoSize = true;
            this.currmove.Location = new System.Drawing.Point(257, 23);
            this.currmove.Name = "currmove";
            this.currmove.Size = new System.Drawing.Size(54, 13);
            this.currmove.TabIndex = 3;
            this.currmove.Text = "currmove:";
            // 
            // hashfull
            // 
            this.hashfull.AutoSize = true;
            this.hashfull.Location = new System.Drawing.Point(158, 23);
            this.hashfull.Name = "hashfull";
            this.hashfull.Size = new System.Drawing.Size(46, 13);
            this.hashfull.TabIndex = 2;
            this.hashfull.Text = "hashfull:";
            // 
            // nps
            // 
            this.nps.AutoSize = true;
            this.nps.Location = new System.Drawing.Point(80, 23);
            this.nps.Name = "nps";
            this.nps.Size = new System.Drawing.Size(27, 13);
            this.nps.TabIndex = 1;
            this.nps.Text = "nps:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(478, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EngineAnalysisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.analysisGroup);
            this.Name = "EngineAnalysisControl";
            this.Size = new System.Drawing.Size(522, 85);
            this.analysisGroup.ResumeLayout(false);
            this.analysisGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox analysisGroup;
        private System.Windows.Forms.Label depth;
        private System.Windows.Forms.Label pv;
        private System.Windows.Forms.Label cpu;
        private System.Windows.Forms.Label currmove;
        private System.Windows.Forms.Label hashfull;
        private System.Windows.Forms.Label nps;
        private System.Windows.Forms.Button button1;
    }
}
