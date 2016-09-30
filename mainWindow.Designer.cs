namespace epdTester
{
    partial class mainWindow
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.engineDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.eEloBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.updatePath = new System.Windows.Forms.Button();
            this.ePathBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.findConfigFile = new System.Windows.Forms.Button();
            this.eConfigfileBox = new System.Windows.Forms.TextBox();
            this.findOpeningBook = new System.Windows.Forms.Button();
            this.findEndgameBook = new System.Windows.Forms.Button();
            this.authorLabel = new System.Windows.Forms.Label();
            this.protocolLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.eEndgameBookBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.startEngine = new System.Windows.Forms.Button();
            this.eOpeningBookBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.eCommandBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.eNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.engineList = new System.Windows.Forms.ListBox();
            this.button8 = new System.Windows.Forms.Button();
            this.removeEngineFromList = new System.Windows.Forms.Button();
            this.addEngineToList = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.engineDetailsGroupBox.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(18, 297);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1079, 218);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "title-1";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "title 2";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "title 3";
            this.columnHeader3.Width = 120;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 278);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1103, 248);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Epd results";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1107, 272);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage3.Controls.Add(this.engineDetailsGroupBox);
            this.tabPage3.Controls.Add(this.engineList);
            this.tabPage3.Controls.Add(this.button8);
            this.tabPage3.Controls.Add(this.removeEngineFromList);
            this.tabPage3.Controls.Add(this.addEngineToList);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1099, 246);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "engine..";
            // 
            // engineDetailsGroupBox
            // 
            this.engineDetailsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.engineDetailsGroupBox.Controls.Add(this.eEloBox);
            this.engineDetailsGroupBox.Controls.Add(this.label10);
            this.engineDetailsGroupBox.Controls.Add(this.updatePath);
            this.engineDetailsGroupBox.Controls.Add(this.ePathBox);
            this.engineDetailsGroupBox.Controls.Add(this.label9);
            this.engineDetailsGroupBox.Controls.Add(this.findConfigFile);
            this.engineDetailsGroupBox.Controls.Add(this.eConfigfileBox);
            this.engineDetailsGroupBox.Controls.Add(this.findOpeningBook);
            this.engineDetailsGroupBox.Controls.Add(this.findEndgameBook);
            this.engineDetailsGroupBox.Controls.Add(this.authorLabel);
            this.engineDetailsGroupBox.Controls.Add(this.protocolLabel);
            this.engineDetailsGroupBox.Controls.Add(this.label6);
            this.engineDetailsGroupBox.Controls.Add(this.eEndgameBookBox);
            this.engineDetailsGroupBox.Controls.Add(this.label5);
            this.engineDetailsGroupBox.Controls.Add(this.startEngine);
            this.engineDetailsGroupBox.Controls.Add(this.eOpeningBookBox);
            this.engineDetailsGroupBox.Controls.Add(this.label4);
            this.engineDetailsGroupBox.Controls.Add(this.eCommandBox);
            this.engineDetailsGroupBox.Controls.Add(this.label3);
            this.engineDetailsGroupBox.Controls.Add(this.eNameBox);
            this.engineDetailsGroupBox.Controls.Add(this.label2);
            this.engineDetailsGroupBox.Location = new System.Drawing.Point(465, 5);
            this.engineDetailsGroupBox.Name = "engineDetailsGroupBox";
            this.engineDetailsGroupBox.Size = new System.Drawing.Size(625, 199);
            this.engineDetailsGroupBox.TabIndex = 9;
            this.engineDetailsGroupBox.TabStop = false;
            this.engineDetailsGroupBox.Text = "Engine setup..";
            // 
            // eEloBox
            // 
            this.eEloBox.Location = new System.Drawing.Point(95, 172);
            this.eEloBox.Name = "eEloBox";
            this.eEloBox.Size = new System.Drawing.Size(252, 20);
            this.eEloBox.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 175);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Elo";
            // 
            // updatePath
            // 
            this.updatePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatePath.Location = new System.Drawing.Point(353, 143);
            this.updatePath.Name = "updatePath";
            this.updatePath.Size = new System.Drawing.Size(35, 23);
            this.updatePath.TabIndex = 20;
            this.updatePath.Text = "...";
            this.updatePath.UseVisualStyleBackColor = true;
            this.updatePath.Click += new System.EventHandler(this.updatePath_Click);
            // 
            // ePathBox
            // 
            this.ePathBox.Location = new System.Drawing.Point(95, 146);
            this.ePathBox.Name = "ePathBox";
            this.ePathBox.Size = new System.Drawing.Size(252, 20);
            this.ePathBox.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Path";
            // 
            // findConfigFile
            // 
            this.findConfigFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findConfigFile.Location = new System.Drawing.Point(353, 117);
            this.findConfigFile.Name = "findConfigFile";
            this.findConfigFile.Size = new System.Drawing.Size(35, 23);
            this.findConfigFile.TabIndex = 17;
            this.findConfigFile.Text = "...";
            this.findConfigFile.UseVisualStyleBackColor = true;
            this.findConfigFile.Click += new System.EventHandler(this.findConfigFile_Click);
            // 
            // eConfigfileBox
            // 
            this.eConfigfileBox.Location = new System.Drawing.Point(95, 120);
            this.eConfigfileBox.Name = "eConfigfileBox";
            this.eConfigfileBox.Size = new System.Drawing.Size(252, 20);
            this.eConfigfileBox.TabIndex = 18;
            // 
            // findOpeningBook
            // 
            this.findOpeningBook.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findOpeningBook.Location = new System.Drawing.Point(353, 62);
            this.findOpeningBook.Name = "findOpeningBook";
            this.findOpeningBook.Size = new System.Drawing.Size(35, 23);
            this.findOpeningBook.TabIndex = 16;
            this.findOpeningBook.Text = "...";
            this.findOpeningBook.UseVisualStyleBackColor = true;
            this.findOpeningBook.Click += new System.EventHandler(this.findOpeningBook_Click);
            // 
            // findEndgameBook
            // 
            this.findEndgameBook.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findEndgameBook.Location = new System.Drawing.Point(353, 90);
            this.findEndgameBook.Name = "findEndgameBook";
            this.findEndgameBook.Size = new System.Drawing.Size(35, 23);
            this.findEndgameBook.TabIndex = 10;
            this.findEndgameBook.Text = "...";
            this.findEndgameBook.UseVisualStyleBackColor = true;
            this.findEndgameBook.Click += new System.EventHandler(this.findEndgameBook_Click);
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Location = new System.Drawing.Point(410, 36);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(41, 13);
            this.authorLabel.TabIndex = 15;
            this.authorLabel.Text = "Author:";
            // 
            // protocolLabel
            // 
            this.protocolLabel.AutoSize = true;
            this.protocolLabel.Location = new System.Drawing.Point(410, 16);
            this.protocolLabel.Name = "protocolLabel";
            this.protocolLabel.Size = new System.Drawing.Size(49, 13);
            this.protocolLabel.TabIndex = 14;
            this.protocolLabel.Text = "Protocol:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Config file";
            // 
            // eEndgameBookBox
            // 
            this.eEndgameBookBox.Location = new System.Drawing.Point(95, 93);
            this.eEndgameBookBox.Name = "eEndgameBookBox";
            this.eEndgameBookBox.Size = new System.Drawing.Size(252, 20);
            this.eEndgameBookBox.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Endgame Book";
            // 
            // startEngine
            // 
            this.startEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startEngine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startEngine.Location = new System.Drawing.Point(560, 161);
            this.startEngine.Name = "startEngine";
            this.startEngine.Size = new System.Drawing.Size(59, 32);
            this.startEngine.TabIndex = 10;
            this.startEngine.Text = "Start";
            this.startEngine.UseVisualStyleBackColor = true;
            this.startEngine.Click += new System.EventHandler(this.startEngine_Click);
            // 
            // eOpeningBookBox
            // 
            this.eOpeningBookBox.Location = new System.Drawing.Point(95, 65);
            this.eOpeningBookBox.Name = "eOpeningBookBox";
            this.eOpeningBookBox.Size = new System.Drawing.Size(252, 20);
            this.eOpeningBookBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Opening Book";
            // 
            // eCommandBox
            // 
            this.eCommandBox.Location = new System.Drawing.Point(95, 39);
            this.eCommandBox.Name = "eCommandBox";
            this.eCommandBox.Size = new System.Drawing.Size(252, 20);
            this.eCommandBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Command";
            // 
            // eNameBox
            // 
            this.eNameBox.Location = new System.Drawing.Point(95, 15);
            this.eNameBox.Name = "eNameBox";
            this.eNameBox.Size = new System.Drawing.Size(252, 20);
            this.eNameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name";
            // 
            // engineList
            // 
            this.engineList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.engineList.FormattingEnabled = true;
            this.engineList.Location = new System.Drawing.Point(6, 5);
            this.engineList.Name = "engineList";
            this.engineList.Size = new System.Drawing.Size(433, 199);
            this.engineList.TabIndex = 8;
            this.engineList.SelectedIndexChanged += new System.EventHandler(this.engineList_SelectedEngineChanged);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(1026, 209);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(57, 32);
            this.button8.TabIndex = 7;
            this.button8.Text = "..log";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.showLog_Click);
            // 
            // removeEngineFromList
            // 
            this.removeEngineFromList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeEngineFromList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeEngineFromList.Location = new System.Drawing.Point(404, 210);
            this.removeEngineFromList.Name = "removeEngineFromList";
            this.removeEngineFromList.Size = new System.Drawing.Size(34, 33);
            this.removeEngineFromList.TabIndex = 6;
            this.removeEngineFromList.Text = "-";
            this.removeEngineFromList.UseVisualStyleBackColor = true;
            this.removeEngineFromList.Click += new System.EventHandler(this.engineDeleteClick);
            // 
            // addEngineToList
            // 
            this.addEngineToList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addEngineToList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEngineToList.Location = new System.Drawing.Point(364, 210);
            this.addEngineToList.Name = "addEngineToList";
            this.addEngineToList.Size = new System.Drawing.Size(34, 33);
            this.addEngineToList.TabIndex = 3;
            this.addEngineToList.Text = "+";
            this.addEngineToList.UseVisualStyleBackColor = true;
            this.addEngineToList.Click += new System.EventHandler(this.engineAddClick);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1019, 246);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "epd testing..";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(316, 18);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "EPD File";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(60, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(250, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1019, 246);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tournament..";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1107, 530);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "mainWindow";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.engineDetailsGroupBox.ResumeLayout(false);
            this.engineDetailsGroupBox.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button removeEngineFromList;
        private System.Windows.Forms.Button addEngineToList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ListBox engineList;
        private System.Windows.Forms.GroupBox engineDetailsGroupBox;
        private System.Windows.Forms.Label authorLabel;
        private System.Windows.Forms.Label protocolLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox eEndgameBookBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button startEngine;
        private System.Windows.Forms.TextBox eOpeningBookBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox eCommandBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox eNameBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button updatePath;
        private System.Windows.Forms.TextBox ePathBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button findConfigFile;
        private System.Windows.Forms.TextBox eConfigfileBox;
        private System.Windows.Forms.Button findOpeningBook;
        private System.Windows.Forms.Button findEndgameBook;
        private System.Windows.Forms.TextBox eEloBox;
        private System.Windows.Forms.Label label10;
    }
}

