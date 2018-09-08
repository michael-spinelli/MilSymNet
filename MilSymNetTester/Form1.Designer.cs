namespace MilSymNetTester
{
    partial class Form1
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
            this.btnTest = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbAffiliation = new System.Windows.Forms.ComboBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.cbModifiers = new System.Windows.Forms.ComboBox();
            this.cbSize = new System.Windows.Forms.ComboBox();
            this.btnDrawTG = new System.Windows.Forms.Button();
            this.btnDrawFE = new System.Windows.Forms.Button();
            this.btnSpeedTest = new System.Windows.Forms.Button();
            this.tbSpeedTestCount = new System.Windows.Forms.TextBox();
            this.cbSpeedTestType = new System.Windows.Forms.ComboBox();
            this.cbDoubleBuffer = new System.Windows.Forms.CheckBox();
            this.cbDrawModifiers = new System.Windows.Forms.CheckBox();
            this.cbOutlineType = new System.Windows.Forms.ComboBox();
            this.lbTGs = new System.Windows.Forms.ListBox();
            this.lbFEs = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 581);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(93, 583);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(284, 20);
            this.textBox1.TabIndex = 2;
            // 
            // cbAffiliation
            // 
            this.cbAffiliation.FormattingEnabled = true;
            this.cbAffiliation.Items.AddRange(new object[] {
            "F",
            "H",
            "U",
            "N",
            "S",
            "L",
            "P",
            "G",
            "W",
            "A",
            "D",
            "M",
            "J",
            "K"});
            this.cbAffiliation.Location = new System.Drawing.Point(597, 634);
            this.cbAffiliation.Name = "cbAffiliation";
            this.cbAffiliation.Size = new System.Drawing.Size(77, 21);
            this.cbAffiliation.TabIndex = 3;
            // 
            // cbStatus
            // 
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "A",
            "P",
            "C",
            "D",
            "X",
            "F"});
            this.cbStatus.Location = new System.Drawing.Point(680, 634);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(76, 21);
            this.cbStatus.TabIndex = 4;
            // 
            // cbModifiers
            // 
            this.cbModifiers.FormattingEnabled = true;
            this.cbModifiers.Items.AddRange(new object[] {
            "--",
            "-A",
            "-B",
            "-C",
            "-D",
            "-E",
            "-F",
            "-G",
            "-H",
            "-I",
            "-J",
            "-K",
            "-L",
            "-M",
            "-N",
            "A-",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE",
            "AF",
            "AG",
            "AH",
            "AI",
            "AJ",
            "AK",
            "AL",
            "AM",
            "AN",
            "B-",
            "BA",
            "BB",
            "BC",
            "BD",
            "BE",
            "BF",
            "BG",
            "BH",
            "BI",
            "BJ",
            "BK",
            "BL",
            "BM",
            "BN",
            "C-",
            "CA",
            "CB",
            "CC",
            "CD",
            "CE",
            "CF",
            "CG",
            "CH",
            "CI",
            "CJ",
            "CK",
            "CL",
            "CM",
            "CN",
            "D-",
            "DA",
            "DB",
            "DC",
            "DD",
            "DE",
            "DF",
            "DG",
            "DH",
            "DI",
            "DJ",
            "DK",
            "DL",
            "DM",
            "DN",
            "E-",
            "EA",
            "EB",
            "EC",
            "ED",
            "EE",
            "EF",
            "EG",
            "EH",
            "EI",
            "EJ",
            "EK",
            "EL",
            "EM",
            "EN",
            "F-",
            "FA",
            "F",
            "FC",
            "FD",
            "FE",
            "FF",
            "FG",
            "FH",
            "FI",
            "FJ",
            "FK",
            "FL",
            "FM",
            "FN",
            "G-",
            "GA",
            "GB",
            "GC",
            "GD",
            "GE",
            "GF",
            "GG",
            "GH",
            "GI",
            "GJ",
            "GK",
            "GL",
            "GM",
            "GN",
            "H-",
            "HB",
            "NS",
            "NL",
            "MO",
            "MP",
            "MQ",
            "MR",
            "MS",
            "MT",
            "MU",
            "MV",
            "MW",
            "MX",
            "MY"});
            this.cbModifiers.Location = new System.Drawing.Point(762, 634);
            this.cbModifiers.Name = "cbModifiers";
            this.cbModifiers.Size = new System.Drawing.Size(77, 21);
            this.cbModifiers.TabIndex = 5;
            // 
            // cbSize
            // 
            this.cbSize.FormattingEnabled = true;
            this.cbSize.Items.AddRange(new object[] {
            "50",
            "45",
            "35",
            "25",
            "15"});
            this.cbSize.Location = new System.Drawing.Point(762, 607);
            this.cbSize.Name = "cbSize";
            this.cbSize.Size = new System.Drawing.Size(77, 21);
            this.cbSize.TabIndex = 6;
            // 
            // btnDrawTG
            // 
            this.btnDrawTG.Location = new System.Drawing.Point(12, 635);
            this.btnDrawTG.Name = "btnDrawTG";
            this.btnDrawTG.Size = new System.Drawing.Size(75, 20);
            this.btnDrawTG.TabIndex = 7;
            this.btnDrawTG.Text = "Draw TG";
            this.btnDrawTG.UseVisualStyleBackColor = true;
            this.btnDrawTG.Click += new System.EventHandler(this.btnDrawTG_Click);
            // 
            // btnDrawFE
            // 
            this.btnDrawFE.Location = new System.Drawing.Point(12, 610);
            this.btnDrawFE.Name = "btnDrawFE";
            this.btnDrawFE.Size = new System.Drawing.Size(75, 20);
            this.btnDrawFE.TabIndex = 8;
            this.btnDrawFE.Text = "Draw FE";
            this.btnDrawFE.UseVisualStyleBackColor = true;
            this.btnDrawFE.Click += new System.EventHandler(this.btnDrawFE_Click);
            // 
            // btnSpeedTest
            // 
            this.btnSpeedTest.Location = new System.Drawing.Point(93, 635);
            this.btnSpeedTest.Name = "btnSpeedTest";
            this.btnSpeedTest.Size = new System.Drawing.Size(75, 20);
            this.btnSpeedTest.TabIndex = 9;
            this.btnSpeedTest.Text = "Speed Test";
            this.btnSpeedTest.UseVisualStyleBackColor = true;
            this.btnSpeedTest.Click += new System.EventHandler(this.btnSpeedTest_Click);
            // 
            // tbSpeedTestCount
            // 
            this.tbSpeedTestCount.Location = new System.Drawing.Point(175, 638);
            this.tbSpeedTestCount.Name = "tbSpeedTestCount";
            this.tbSpeedTestCount.Size = new System.Drawing.Size(100, 20);
            this.tbSpeedTestCount.TabIndex = 10;
            this.tbSpeedTestCount.Text = "1000";
            // 
            // cbSpeedTestType
            // 
            this.cbSpeedTestType.FormattingEnabled = true;
            this.cbSpeedTestType.Items.AddRange(new object[] {
            "Mixed",
            "Units",
            "Tactical"});
            this.cbSpeedTestType.Location = new System.Drawing.Point(282, 637);
            this.cbSpeedTestType.Name = "cbSpeedTestType";
            this.cbSpeedTestType.Size = new System.Drawing.Size(105, 21);
            this.cbSpeedTestType.TabIndex = 11;
            // 
            // cbDoubleBuffer
            // 
            this.cbDoubleBuffer.AutoSize = true;
            this.cbDoubleBuffer.Location = new System.Drawing.Point(93, 612);
            this.cbDoubleBuffer.Name = "cbDoubleBuffer";
            this.cbDoubleBuffer.Size = new System.Drawing.Size(91, 17);
            this.cbDoubleBuffer.TabIndex = 12;
            this.cbDoubleBuffer.Text = "Double Buffer";
            this.cbDoubleBuffer.UseVisualStyleBackColor = true;
            // 
            // cbDrawModifiers
            // 
            this.cbDrawModifiers.AutoSize = true;
            this.cbDrawModifiers.Location = new System.Drawing.Point(191, 613);
            this.cbDrawModifiers.Name = "cbDrawModifiers";
            this.cbDrawModifiers.Size = new System.Drawing.Size(68, 17);
            this.cbDrawModifiers.TabIndex = 13;
            this.cbDrawModifiers.Text = "Modifiers";
            this.cbDrawModifiers.UseVisualStyleBackColor = true;
            // 
            // cbOutlineType
            // 
            this.cbOutlineType.FormattingEnabled = true;
            this.cbOutlineType.Items.AddRange(new object[] {
            "None",
            "ColorFill",
            "Outline",
            "OutlineQ"});
            this.cbOutlineType.Location = new System.Drawing.Point(266, 613);
            this.cbOutlineType.Name = "cbOutlineType";
            this.cbOutlineType.Size = new System.Drawing.Size(121, 21);
            this.cbOutlineType.TabIndex = 14;
            // 
            // lbTGs
            // 
            this.lbTGs.FormattingEnabled = true;
            this.lbTGs.HorizontalScrollbar = true;
            this.lbTGs.Location = new System.Drawing.Point(12, 12);
            this.lbTGs.Name = "lbTGs";
            this.lbTGs.Size = new System.Drawing.Size(172, 186);
            this.lbTGs.TabIndex = 15;
            // 
            // lbFEs
            // 
            this.lbFEs.FormattingEnabled = true;
            this.lbFEs.HorizontalScrollbar = true;
            this.lbFEs.Location = new System.Drawing.Point(680, 12);
            this.lbFEs.Name = "lbFEs";
            this.lbFEs.Size = new System.Drawing.Size(159, 186);
            this.lbFEs.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkCyan;
            this.ClientSize = new System.Drawing.Size(851, 667);
            this.Controls.Add(this.lbFEs);
            this.Controls.Add(this.lbTGs);
            this.Controls.Add(this.cbOutlineType);
            this.Controls.Add(this.cbDrawModifiers);
            this.Controls.Add(this.cbDoubleBuffer);
            this.Controls.Add(this.cbSpeedTestType);
            this.Controls.Add(this.tbSpeedTestCount);
            this.Controls.Add(this.btnSpeedTest);
            this.Controls.Add(this.btnDrawFE);
            this.Controls.Add(this.btnDrawTG);
            this.Controls.Add(this.cbSize);
            this.Controls.Add(this.cbModifiers);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.cbAffiliation);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox cbAffiliation;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.ComboBox cbModifiers;
        private System.Windows.Forms.ComboBox cbSize;
        private System.Windows.Forms.Button btnDrawTG;
        private System.Windows.Forms.Button btnDrawFE;
        private System.Windows.Forms.Button btnSpeedTest;
        private System.Windows.Forms.TextBox tbSpeedTestCount;
        private System.Windows.Forms.ComboBox cbSpeedTestType;
        private System.Windows.Forms.CheckBox cbDoubleBuffer;
        private System.Windows.Forms.CheckBox cbDrawModifiers;
        private System.Windows.Forms.ComboBox cbOutlineType;
        private System.Windows.Forms.ListBox lbTGs;
        private System.Windows.Forms.ListBox lbFEs;
    }
}

