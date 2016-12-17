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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainWindow));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chessBoard = new System.Windows.Forms.Button();
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
            this.chessPlot1 = new epdTester.ChessPlot();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.EPDTestCorrect_label = new System.Windows.Forms.Label();
            this.EPDTestProgress_label = new System.Windows.Forms.Label();
            this.epdStart = new System.Windows.Forms.Button();
            this.stopEpdTest = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.epdFixedTotalNodes = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.epdFixedNodesDepth = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.epdFixedTotalTime = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.epdFixedTimePosition = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.epdFixedSearchDepth = new System.Windows.Forms.TextBox();
            this.epdDirLabel = new System.Windows.Forms.Label();
            this.setEPDdirectory = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.epdCombobox = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.epdTabDisplay = new epdTester.EpdTabDisplay();
            this.useLogCheckbox = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.engineDetailsGroupBox.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1662, 418);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage3.Controls.Add(this.chessBoard);
            this.tabPage3.Controls.Add(this.engineDetailsGroupBox);
            this.tabPage3.Controls.Add(this.engineList);
            this.tabPage3.Controls.Add(this.button8);
            this.tabPage3.Controls.Add(this.removeEngineFromList);
            this.tabPage3.Controls.Add(this.addEngineToList);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1654, 385);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "engine..";
            // 
            // chessBoard
            // 
            this.chessBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chessBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chessBoard.Location = new System.Drawing.Point(1446, 322);
            this.chessBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(86, 49);
            this.chessBoard.TabIndex = 10;
            this.chessBoard.Text = "..board";
            this.chessBoard.UseVisualStyleBackColor = true;
            this.chessBoard.Click += new System.EventHandler(this.chessBoard_Click);
            // 
            // engineDetailsGroupBox
            // 
            this.engineDetailsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.engineDetailsGroupBox.Controls.Add(this.useLogCheckbox);
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
            this.engineDetailsGroupBox.Location = new System.Drawing.Point(699, 8);
            this.engineDetailsGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.engineDetailsGroupBox.Name = "engineDetailsGroupBox";
            this.engineDetailsGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.engineDetailsGroupBox.Size = new System.Drawing.Size(938, 306);
            this.engineDetailsGroupBox.TabIndex = 9;
            this.engineDetailsGroupBox.TabStop = false;
            this.engineDetailsGroupBox.Text = "Engine setup..";
            // 
            // eEloBox
            // 
            this.eEloBox.Location = new System.Drawing.Point(142, 265);
            this.eEloBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eEloBox.Name = "eEloBox";
            this.eEloBox.Size = new System.Drawing.Size(376, 26);
            this.eEloBox.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 269);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "Elo";
            // 
            // updatePath
            // 
            this.updatePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updatePath.Location = new System.Drawing.Point(530, 220);
            this.updatePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updatePath.Name = "updatePath";
            this.updatePath.Size = new System.Drawing.Size(52, 35);
            this.updatePath.TabIndex = 20;
            this.updatePath.Text = "...";
            this.updatePath.UseVisualStyleBackColor = true;
            this.updatePath.Click += new System.EventHandler(this.updatePath_Click);
            // 
            // ePathBox
            // 
            this.ePathBox.Location = new System.Drawing.Point(142, 225);
            this.ePathBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ePathBox.Name = "ePathBox";
            this.ePathBox.Size = new System.Drawing.Size(376, 26);
            this.ePathBox.TabIndex = 21;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 229);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "Path";
            // 
            // findConfigFile
            // 
            this.findConfigFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findConfigFile.Location = new System.Drawing.Point(530, 180);
            this.findConfigFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.findConfigFile.Name = "findConfigFile";
            this.findConfigFile.Size = new System.Drawing.Size(52, 35);
            this.findConfigFile.TabIndex = 17;
            this.findConfigFile.Text = "...";
            this.findConfigFile.UseVisualStyleBackColor = true;
            this.findConfigFile.Click += new System.EventHandler(this.findConfigFile_Click);
            // 
            // eConfigfileBox
            // 
            this.eConfigfileBox.Location = new System.Drawing.Point(142, 185);
            this.eConfigfileBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eConfigfileBox.Name = "eConfigfileBox";
            this.eConfigfileBox.Size = new System.Drawing.Size(376, 26);
            this.eConfigfileBox.TabIndex = 18;
            // 
            // findOpeningBook
            // 
            this.findOpeningBook.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findOpeningBook.Location = new System.Drawing.Point(530, 95);
            this.findOpeningBook.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.findOpeningBook.Name = "findOpeningBook";
            this.findOpeningBook.Size = new System.Drawing.Size(52, 35);
            this.findOpeningBook.TabIndex = 16;
            this.findOpeningBook.Text = "...";
            this.findOpeningBook.UseVisualStyleBackColor = true;
            this.findOpeningBook.Click += new System.EventHandler(this.findOpeningBook_Click);
            // 
            // findEndgameBook
            // 
            this.findEndgameBook.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findEndgameBook.Location = new System.Drawing.Point(530, 138);
            this.findEndgameBook.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.findEndgameBook.Name = "findEndgameBook";
            this.findEndgameBook.Size = new System.Drawing.Size(52, 35);
            this.findEndgameBook.TabIndex = 10;
            this.findEndgameBook.Text = "...";
            this.findEndgameBook.UseVisualStyleBackColor = true;
            this.findEndgameBook.Click += new System.EventHandler(this.findEndgameBook_Click);
            // 
            // authorLabel
            // 
            this.authorLabel.AutoSize = true;
            this.authorLabel.Location = new System.Drawing.Point(615, 55);
            this.authorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.authorLabel.Name = "authorLabel";
            this.authorLabel.Size = new System.Drawing.Size(61, 20);
            this.authorLabel.TabIndex = 15;
            this.authorLabel.Text = "Author:";
            // 
            // protocolLabel
            // 
            this.protocolLabel.AutoSize = true;
            this.protocolLabel.Location = new System.Drawing.Point(615, 25);
            this.protocolLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.protocolLabel.Name = "protocolLabel";
            this.protocolLabel.Size = new System.Drawing.Size(71, 20);
            this.protocolLabel.TabIndex = 14;
            this.protocolLabel.Text = "Protocol:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 189);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Config file";
            // 
            // eEndgameBookBox
            // 
            this.eEndgameBookBox.Location = new System.Drawing.Point(142, 143);
            this.eEndgameBookBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eEndgameBookBox.Name = "eEndgameBookBox";
            this.eEndgameBookBox.Size = new System.Drawing.Size(376, 26);
            this.eEndgameBookBox.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 148);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Endgame Book";
            // 
            // startEngine
            // 
            this.startEngine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startEngine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startEngine.Location = new System.Drawing.Point(840, 248);
            this.startEngine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.startEngine.Name = "startEngine";
            this.startEngine.Size = new System.Drawing.Size(88, 49);
            this.startEngine.TabIndex = 10;
            this.startEngine.Text = "Start";
            this.startEngine.UseVisualStyleBackColor = true;
            this.startEngine.Click += new System.EventHandler(this.startEngine_Click);
            // 
            // eOpeningBookBox
            // 
            this.eOpeningBookBox.Location = new System.Drawing.Point(142, 100);
            this.eOpeningBookBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eOpeningBookBox.Name = "eOpeningBookBox";
            this.eOpeningBookBox.Size = new System.Drawing.Size(376, 26);
            this.eOpeningBookBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 105);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Opening Book";
            // 
            // eCommandBox
            // 
            this.eCommandBox.Location = new System.Drawing.Point(142, 60);
            this.eCommandBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eCommandBox.Name = "eCommandBox";
            this.eCommandBox.Size = new System.Drawing.Size(376, 26);
            this.eCommandBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Start command";
            // 
            // eNameBox
            // 
            this.eNameBox.Location = new System.Drawing.Point(142, 23);
            this.eNameBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.eNameBox.Name = "eNameBox";
            this.eNameBox.Size = new System.Drawing.Size(376, 26);
            this.eNameBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name";
            // 
            // engineList
            // 
            this.engineList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.engineList.FormattingEnabled = true;
            this.engineList.ItemHeight = 20;
            this.engineList.Location = new System.Drawing.Point(9, 8);
            this.engineList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.engineList.Name = "engineList";
            this.engineList.Size = new System.Drawing.Size(649, 304);
            this.engineList.TabIndex = 8;
            this.engineList.SelectedIndexChanged += new System.EventHandler(this.engineList_SelectedEngineChanged);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(1540, 322);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(86, 49);
            this.button8.TabIndex = 7;
            this.button8.Text = "..log";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.showLog_Click);
            // 
            // removeEngineFromList
            // 
            this.removeEngineFromList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeEngineFromList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeEngineFromList.Location = new System.Drawing.Point(608, 323);
            this.removeEngineFromList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.removeEngineFromList.Name = "removeEngineFromList";
            this.removeEngineFromList.Size = new System.Drawing.Size(51, 51);
            this.removeEngineFromList.TabIndex = 6;
            this.removeEngineFromList.Text = "-";
            this.removeEngineFromList.UseVisualStyleBackColor = true;
            this.removeEngineFromList.Click += new System.EventHandler(this.engineDeleteClick);
            // 
            // addEngineToList
            // 
            this.addEngineToList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addEngineToList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEngineToList.Location = new System.Drawing.Point(548, 323);
            this.addEngineToList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.addEngineToList.Name = "addEngineToList";
            this.addEngineToList.Size = new System.Drawing.Size(51, 51);
            this.addEngineToList.TabIndex = 3;
            this.addEngineToList.Text = "+";
            this.addEngineToList.UseVisualStyleBackColor = true;
            this.addEngineToList.Click += new System.EventHandler(this.engineAddClick);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage1.Controls.Add(this.chessPlot1);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.epdDirLabel);
            this.tabPage1.Controls.Add(this.setEPDdirectory);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.epdCombobox);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(1654, 385);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "epd testing..";
            // 
            // chessPlot1
            // 
            this.chessPlot1.Location = new System.Drawing.Point(1119, 45);
            this.chessPlot1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.chessPlot1.Name = "chessPlot1";
            this.chessPlot1.Size = new System.Drawing.Size(477, 325);
            this.chessPlot1.TabIndex = 43;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.EPDTestCorrect_label);
            this.groupBox3.Controls.Add(this.EPDTestProgress_label);
            this.groupBox3.Controls.Add(this.epdStart);
            this.groupBox3.Controls.Add(this.stopEpdTest);
            this.groupBox3.Location = new System.Drawing.Point(528, 37);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(582, 332);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "test progress..";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(177, 58);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 20);
            this.label7.TabIndex = 50;
            this.label7.Text = "solved correctly ..";
            // 
            // EPDTestCorrect_label
            // 
            this.EPDTestCorrect_label.AutoSize = true;
            this.EPDTestCorrect_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EPDTestCorrect_label.Location = new System.Drawing.Point(8, 38);
            this.EPDTestCorrect_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EPDTestCorrect_label.Name = "EPDTestCorrect_label";
            this.EPDTestCorrect_label.Size = new System.Drawing.Size(178, 55);
            this.EPDTestCorrect_label.TabIndex = 49;
            this.EPDTestCorrect_label.Text = "correct";
            // 
            // EPDTestProgress_label
            // 
            this.EPDTestProgress_label.AutoSize = true;
            this.EPDTestProgress_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EPDTestProgress_label.Location = new System.Drawing.Point(14, 98);
            this.EPDTestProgress_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EPDTestProgress_label.Name = "EPDTestProgress_label";
            this.EPDTestProgress_label.Size = new System.Drawing.Size(75, 20);
            this.EPDTestProgress_label.TabIndex = 48;
            this.EPDTestProgress_label.Text = "progress";
            // 
            // epdStart
            // 
            this.epdStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.epdStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epdStart.Location = new System.Drawing.Point(387, 274);
            this.epdStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdStart.Name = "epdStart";
            this.epdStart.Size = new System.Drawing.Size(96, 49);
            this.epdStart.TabIndex = 40;
            this.epdStart.Text = "Start";
            this.epdStart.UseVisualStyleBackColor = true;
            this.epdStart.Click += new System.EventHandler(this.epdStart_Click);
            // 
            // stopEpdTest
            // 
            this.stopEpdTest.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.stopEpdTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopEpdTest.Location = new System.Drawing.Point(492, 274);
            this.stopEpdTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stopEpdTest.Name = "stopEpdTest";
            this.stopEpdTest.Size = new System.Drawing.Size(86, 49);
            this.stopEpdTest.TabIndex = 41;
            this.stopEpdTest.Text = "Stop";
            this.stopEpdTest.UseVisualStyleBackColor = true;
            this.stopEpdTest.Click += new System.EventHandler(this.stopEpdTest_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.epdFixedTotalNodes);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.epdFixedNodesDepth);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.epdFixedTotalTime);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.epdFixedTimePosition);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.epdFixedSearchDepth);
            this.groupBox2.Location = new System.Drawing.Point(14, 82);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(506, 288);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "search conditions..";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(219, 251);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 20);
            this.label8.TabIndex = 47;
            this.label8.Text = "location";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(165, 243);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 35);
            this.button2.TabIndex = 40;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(9, 249);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(145, 24);
            this.checkBox2.TabIndex = 46;
            this.checkBox2.Text = "save test report";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 214);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(198, 24);
            this.checkBox1.TabIndex = 45;
            this.checkBox1.Text = "test all running engines";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // epdFixedTotalNodes
            // 
            this.epdFixedTotalNodes.Location = new System.Drawing.Point(9, 174);
            this.epdFixedTotalNodes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdFixedTotalNodes.Name = "epdFixedTotalNodes";
            this.epdFixedTotalNodes.Size = new System.Drawing.Size(121, 26);
            this.epdFixedTotalNodes.TabIndex = 44;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(141, 178);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(134, 20);
            this.label14.TabIndex = 43;
            this.label14.Text = "Fixed total nodes ";
            // 
            // epdFixedNodesDepth
            // 
            this.epdFixedNodesDepth.Location = new System.Drawing.Point(9, 137);
            this.epdFixedNodesDepth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdFixedNodesDepth.Name = "epdFixedNodesDepth";
            this.epdFixedNodesDepth.Size = new System.Drawing.Size(121, 26);
            this.epdFixedNodesDepth.TabIndex = 42;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(141, 142);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(140, 20);
            this.label13.TabIndex = 41;
            this.label13.Text = "Fixed nodes/depth";
            // 
            // epdFixedTotalTime
            // 
            this.epdFixedTotalTime.Location = new System.Drawing.Point(9, 102);
            this.epdFixedTotalTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdFixedTotalTime.Name = "epdFixedTotalTime";
            this.epdFixedTotalTime.Size = new System.Drawing.Size(121, 26);
            this.epdFixedTotalTime.TabIndex = 40;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(141, 106);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(151, 20);
            this.label12.TabIndex = 39;
            this.label12.Text = "Fixed total time (ms)";
            // 
            // epdFixedTimePosition
            // 
            this.epdFixedTimePosition.Location = new System.Drawing.Point(9, 29);
            this.epdFixedTimePosition.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdFixedTimePosition.Name = "epdFixedTimePosition";
            this.epdFixedTimePosition.Size = new System.Drawing.Size(121, 26);
            this.epdFixedTimePosition.TabIndex = 26;
            this.epdFixedTimePosition.TextChanged += new System.EventHandler(this.epdFixedTimePosition_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(141, 34);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(175, 20);
            this.label16.TabIndex = 25;
            this.label16.Text = "Fixed time/position (ms)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(141, 69);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 20);
            this.label11.TabIndex = 35;
            this.label11.Text = "Fixed search depth";
            // 
            // epdFixedSearchDepth
            // 
            this.epdFixedSearchDepth.Location = new System.Drawing.Point(9, 65);
            this.epdFixedSearchDepth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdFixedSearchDepth.Name = "epdFixedSearchDepth";
            this.epdFixedSearchDepth.Size = new System.Drawing.Size(121, 26);
            this.epdFixedSearchDepth.TabIndex = 36;
            // 
            // epdDirLabel
            // 
            this.epdDirLabel.AutoSize = true;
            this.epdDirLabel.Location = new System.Drawing.Point(86, 15);
            this.epdDirLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.epdDirLabel.Name = "epdDirLabel";
            this.epdDirLabel.Size = new System.Drawing.Size(69, 20);
            this.epdDirLabel.TabIndex = 7;
            this.epdDirLabel.Text = "directory";
            // 
            // setEPDdirectory
            // 
            this.setEPDdirectory.Location = new System.Drawing.Point(474, 37);
            this.setEPDdirectory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setEPDdirectory.Name = "setEPDdirectory";
            this.setEPDdirectory.Size = new System.Drawing.Size(45, 35);
            this.setEPDdirectory.TabIndex = 6;
            this.setEPDdirectory.Text = "...";
            this.setEPDdirectory.UseVisualStyleBackColor = true;
            this.setEPDdirectory.Click += new System.EventHandler(this.setEPDdirectory_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "EPD File";
            // 
            // epdCombobox
            // 
            this.epdCombobox.FormattingEnabled = true;
            this.epdCombobox.Location = new System.Drawing.Point(87, 40);
            this.epdCombobox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.epdCombobox.Name = "epdCombobox";
            this.epdCombobox.Size = new System.Drawing.Size(373, 28);
            this.epdCombobox.TabIndex = 0;
            this.epdCombobox.SelectedIndexChanged += new System.EventHandler(this.epdCombobox_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(1654, 385);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "regression..";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1654, 385);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tuning..";
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Menu;
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1654, 385);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "board..";
            // 
            // epdTabDisplay
            // 
            this.epdTabDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.epdTabDisplay.Location = new System.Drawing.Point(6, 428);
            this.epdTabDisplay.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.epdTabDisplay.Name = "epdTabDisplay";
            this.epdTabDisplay.Size = new System.Drawing.Size(1650, 466);
            this.epdTabDisplay.TabIndex = 7;
            // 
            // useLogCheckbox
            // 
            this.useLogCheckbox.AutoSize = true;
            this.useLogCheckbox.Location = new System.Drawing.Point(738, 269);
            this.useLogCheckbox.Name = "useLogCheckbox";
            this.useLogCheckbox.Size = new System.Drawing.Size(95, 24);
            this.useLogCheckbox.TabIndex = 25;
            this.useLogCheckbox.Text = "Use Log";
            this.useLogCheckbox.UseVisualStyleBackColor = true;
            this.useLogCheckbox.CheckedChanged += new System.EventHandler(this.useLogCheckbox_CheckedChanged);
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1662, 903);
            this.Controls.Add(this.epdTabDisplay);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "mainWindow";
            this.Text = "..EPD tester";
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.engineDetailsGroupBox.ResumeLayout(false);
            this.engineDetailsGroupBox.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button removeEngineFromList;
        private System.Windows.Forms.Button addEngineToList;
        private System.Windows.Forms.Button setEPDdirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox epdCombobox;
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox epdFixedTotalNodes;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox epdFixedNodesDepth;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox epdFixedTotalTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox epdFixedTimePosition;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox epdFixedSearchDepth;
        private System.Windows.Forms.Label epdDirLabel;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button epdStart;
        private System.Windows.Forms.Button stopEpdTest;
        private System.Windows.Forms.Label EPDTestProgress_label;
        private System.Windows.Forms.Label EPDTestCorrect_label;
        private ChessPlot chessPlot1;
        private EpdTabDisplay epdTabDisplay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button chessBoard;
        private System.Windows.Forms.CheckBox useLogCheckbox;
    }
}

