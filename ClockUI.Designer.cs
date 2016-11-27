namespace epdTester
{
    partial class ClockUI
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
            this.whitePlayer = new System.Windows.Forms.GroupBox();
            this.wtime = new System.Windows.Forms.Label();
            this.blackPlayer = new System.Windows.Forms.GroupBox();
            this.btime = new System.Windows.Forms.Label();
            this.whitePlayer.SuspendLayout();
            this.blackPlayer.SuspendLayout();
            this.SuspendLayout();
            // 
            // whitePlayer
            // 
            this.whitePlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.whitePlayer.Controls.Add(this.wtime);
            this.whitePlayer.Location = new System.Drawing.Point(3, 3);
            this.whitePlayer.Name = "whitePlayer";
            this.whitePlayer.Size = new System.Drawing.Size(225, 81);
            this.whitePlayer.TabIndex = 0;
            this.whitePlayer.TabStop = false;
            this.whitePlayer.Text = "White";
            // 
            // wtime
            // 
            this.wtime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wtime.AutoSize = true;
            this.wtime.Font = new System.Drawing.Font("Trebuchet MS", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wtime.Location = new System.Drawing.Point(6, 16);
            this.wtime.Name = "wtime";
            this.wtime.Size = new System.Drawing.Size(213, 61);
            this.wtime.TabIndex = 0;
            this.wtime.Text = "00:00.00";
            // 
            // blackPlayer
            // 
            this.blackPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blackPlayer.Controls.Add(this.btime);
            this.blackPlayer.Location = new System.Drawing.Point(236, 3);
            this.blackPlayer.Name = "blackPlayer";
            this.blackPlayer.Size = new System.Drawing.Size(225, 81);
            this.blackPlayer.TabIndex = 1;
            this.blackPlayer.TabStop = false;
            this.blackPlayer.Text = "Black";
            // 
            // btime
            // 
            this.btime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btime.AutoSize = true;
            this.btime.Font = new System.Drawing.Font("Trebuchet MS", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btime.Location = new System.Drawing.Point(6, 16);
            this.btime.Name = "btime";
            this.btime.Size = new System.Drawing.Size(213, 61);
            this.btime.TabIndex = 0;
            this.btime.Text = "00:00.00";
            // 
            // ClockUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.blackPlayer);
            this.Controls.Add(this.whitePlayer);
            this.Name = "ClockUI";
            this.Size = new System.Drawing.Size(466, 91);
            this.whitePlayer.ResumeLayout(false);
            this.whitePlayer.PerformLayout();
            this.blackPlayer.ResumeLayout(false);
            this.blackPlayer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox whitePlayer;
        private System.Windows.Forms.Label wtime;
        private System.Windows.Forms.GroupBox blackPlayer;
        private System.Windows.Forms.Label btime;
    }
}
