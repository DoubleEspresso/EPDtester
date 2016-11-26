namespace epdTester
{
    partial class ChessBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessBoard));
            this.boardPane = new epdTester.GL();
            this.SuspendLayout();
            // 
            // boardPane
            // 
            this.boardPane.AutoSize = true;
            this.boardPane.BackColor = System.Drawing.SystemColors.ControlDark;
            this.boardPane.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.boardPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boardPane.Location = new System.Drawing.Point(0, 0);
            this.boardPane.Name = "boardPane";
            this.boardPane.Size = new System.Drawing.Size(649, 470);
            this.boardPane.TabIndex = 0;
            // 
            // ChessBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 470);
            this.Controls.Add(this.boardPane);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ChessBoard";
            this.Text = "ChessBoard";
            this.ResizeEnd += new System.EventHandler(this.ResizeFinished);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GL boardPane;
    }
}