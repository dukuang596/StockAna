namespace TestRuleEngine
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.BtTimezone = new System.Windows.Forms.Button();
            this.rbt_second = new System.Windows.Forms.RadioButton();
            this.rbt_minute = new System.Windows.Forms.RadioButton();
            this.start_date = new System.Windows.Forms.DateTimePicker();
            this.rtbSymbols = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbt15second = new System.Windows.Forms.RadioButton();
            this.end_date = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbt5second = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(528, 300);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(35, 407);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 1;
            this.button2.Text = "getHistory";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(35, 27);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 29);
            this.button3.TabIndex = 2;
            this.button3.Text = "connect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(143, 27);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 29);
            this.button4.TabIndex = 3;
            this.button4.Text = "disconnect";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(528, 175);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "GetTickData";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // BtTimezone
            // 
            this.BtTimezone.Enabled = false;
            this.BtTimezone.Location = new System.Drawing.Point(528, 222);
            this.BtTimezone.Name = "BtTimezone";
            this.BtTimezone.Size = new System.Drawing.Size(75, 23);
            this.BtTimezone.TabIndex = 5;
            this.BtTimezone.Text = "timezone";
            this.BtTimezone.UseVisualStyleBackColor = true;
            this.BtTimezone.Visible = false;
            this.BtTimezone.Click += new System.EventHandler(this.BtTimezone_Click);
            // 
            // rbt_second
            // 
            this.rbt_second.AutoSize = true;
            this.rbt_second.Checked = true;
            this.rbt_second.Location = new System.Drawing.Point(8, 35);
            this.rbt_second.Name = "rbt_second";
            this.rbt_second.Size = new System.Drawing.Size(76, 19);
            this.rbt_second.TabIndex = 6;
            this.rbt_second.TabStop = true;
            this.rbt_second.Text = "second";
            this.rbt_second.UseVisualStyleBackColor = true;
            // 
            // rbt_minute
            // 
            this.rbt_minute.AutoSize = true;
            this.rbt_minute.Location = new System.Drawing.Point(106, 78);
            this.rbt_minute.Name = "rbt_minute";
            this.rbt_minute.Size = new System.Drawing.Size(76, 19);
            this.rbt_minute.TabIndex = 7;
            this.rbt_minute.Text = "minute";
            this.rbt_minute.UseVisualStyleBackColor = true;
            // 
            // start_date
            // 
            this.start_date.CustomFormat = "yyyy-MM-dd";
            this.start_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.start_date.Location = new System.Drawing.Point(110, 80);
            this.start_date.Name = "start_date";
            this.start_date.Size = new System.Drawing.Size(133, 25);
            this.start_date.TabIndex = 8;
            // 
            // rtbSymbols
            // 
            this.rtbSymbols.Location = new System.Drawing.Point(35, 261);
            this.rtbSymbols.Name = "rtbSymbols";
            this.rtbSymbols.Size = new System.Drawing.Size(208, 120);
            this.rtbSymbols.TabIndex = 9;
            this.rtbSymbols.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbt5second);
            this.groupBox1.Controls.Add(this.rbt15second);
            this.groupBox1.Controls.Add(this.rbt_minute);
            this.groupBox1.Controls.Add(this.rbt_second);
            this.groupBox1.Location = new System.Drawing.Point(35, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 103);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "scale";
            // 
            // rbt15second
            // 
            this.rbt15second.AutoSize = true;
            this.rbt15second.Checked = true;
            this.rbt15second.Location = new System.Drawing.Point(8, 78);
            this.rbt15second.Name = "rbt15second";
            this.rbt15second.Size = new System.Drawing.Size(92, 19);
            this.rbt15second.TabIndex = 8;
            this.rbt15second.TabStop = true;
            this.rbt15second.Text = "15second";
            this.rbt15second.UseVisualStyleBackColor = true;
            // 
            // end_date
            // 
            this.end_date.CustomFormat = "yyyy-MM-dd";
            this.end_date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.end_date.Location = new System.Drawing.Point(110, 116);
            this.end_date.Name = "end_date";
            this.end_date.Size = new System.Drawing.Size(133, 25);
            this.end_date.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "开始";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "结束";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // rbt5second
            // 
            this.rbt5second.AutoSize = true;
            this.rbt5second.Checked = true;
            this.rbt5second.Location = new System.Drawing.Point(106, 37);
            this.rbt5second.Name = "rbt5second";
            this.rbt5second.Size = new System.Drawing.Size(84, 19);
            this.rbt5second.TabIndex = 9;
            this.rbt5second.TabStop = true;
            this.rbt5second.Text = "5second";
            this.rbt5second.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 496);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.end_date);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbSymbols);
            this.Controls.Add(this.start_date);
            this.Controls.Add(this.BtTimezone);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button BtTimezone;
        private System.Windows.Forms.RadioButton rbt_second;
        private System.Windows.Forms.RadioButton rbt_minute;
        private System.Windows.Forms.DateTimePicker start_date;
        private System.Windows.Forms.RichTextBox rtbSymbols;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker end_date;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.RadioButton rbt15second;
        private System.Windows.Forms.RadioButton rbt5second;
    }
}

