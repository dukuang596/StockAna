namespace WebPageWatcher
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
            this.watchUrl = new System.Windows.Forms.TextBox();
            this.urllbl = new System.Windows.Forms.Label();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.groupxpath = new System.Windows.Forms.TextBox();
            this.xpath = new System.Windows.Forms.Label();
            this.spec = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hasEndTime = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // watchUrl
            // 
            this.watchUrl.Location = new System.Drawing.Point(93, 26);
            this.watchUrl.Name = "watchUrl";
            this.watchUrl.Size = new System.Drawing.Size(259, 21);
            this.watchUrl.TabIndex = 0;
            this.watchUrl.Text = "http://www.quchaogu.com/wealth/list";
            // 
            // urllbl
            // 
            this.urllbl.AutoSize = true;
            this.urllbl.Location = new System.Drawing.Point(34, 29);
            this.urllbl.Name = "urllbl";
            this.urllbl.Size = new System.Drawing.Size(47, 12);
            this.urllbl.TabIndex = 1;
            this.urllbl.Text = "监控url";
            // 
            // endTime
            // 
            this.endTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.endTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTime.Location = new System.Drawing.Point(138, 164);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(152, 21);
            this.endTime.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(36, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "开始监控";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupxpath
            // 
            this.groupxpath.Location = new System.Drawing.Point(93, 86);
            this.groupxpath.Name = "groupxpath";
            this.groupxpath.Size = new System.Drawing.Size(259, 21);
            this.groupxpath.TabIndex = 4;
            this.groupxpath.Text = "/html/body/div[3]/div/div[1]/div/div[1]/a";
            // 
            // xpath
            // 
            this.xpath.AutoSize = true;
            this.xpath.Location = new System.Drawing.Point(34, 62);
            this.xpath.Name = "xpath";
            this.xpath.Size = new System.Drawing.Size(95, 12);
            this.xpath.TabIndex = 6;
            this.xpath.Text = "组路径（xpath）";
            // 
            // spec
            // 
            this.spec.Location = new System.Drawing.Point(93, 122);
            this.spec.Name = "spec";
            this.spec.Size = new System.Drawing.Size(100, 21);
            this.spec.TabIndex = 7;
            this.spec.Text = "无忧理财S计划";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "特征";
            // 
            // hasEndTime
            // 
            this.hasEndTime.AutoSize = true;
            this.hasEndTime.Location = new System.Drawing.Point(51, 164);
            this.hasEndTime.Name = "hasEndTime";
            this.hasEndTime.Size = new System.Drawing.Size(72, 16);
            this.hasEndTime.TabIndex = 9;
            this.hasEndTime.Text = "结束时间";
            this.hasEndTime.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 435);
            this.Controls.Add(this.hasEndTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spec);
            this.Controls.Add(this.xpath);
            this.Controls.Add(this.groupxpath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.urllbl);
            this.Controls.Add(this.watchUrl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox watchUrl;
        private System.Windows.Forms.Label urllbl;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox groupxpath;
        private System.Windows.Forms.Label xpath;
        private System.Windows.Forms.TextBox spec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox hasEndTime;
    }
}

