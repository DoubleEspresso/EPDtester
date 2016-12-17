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
            this.engineAnalysis = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // engineAnalysis
            // 
            this.engineAnalysis.Location = new System.Drawing.Point(10, 0);
            this.engineAnalysis.Multiline = true;
            this.engineAnalysis.Name = "engineAnalysis";
            this.engineAnalysis.ReadOnly = true;
            this.engineAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.engineAnalysis.Size = new System.Drawing.Size(460, 216);
            this.engineAnalysis.TabIndex = 0;
            // 
            // EngineAnalysisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.engineAnalysis);
            this.Name = "EngineAnalysisControl";
            this.Size = new System.Drawing.Size(473, 222);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox engineAnalysis;
    }
}
