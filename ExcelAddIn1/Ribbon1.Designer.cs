namespace TestExcelAddin
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Analysis = this.Factory.CreateRibbonTab();
            this.Stock = this.Factory.CreateRibbonGroup();
            this.Traffic = this.Factory.CreateRibbonButton();
            this.tbtnAccountPanel = this.Factory.CreateRibbonToggleButton();
            this.Analysis.SuspendLayout();
            this.Stock.SuspendLayout();
            this.SuspendLayout();
            // 
            // Analysis
            // 
            this.Analysis.Groups.Add(this.Stock);
            this.Analysis.Label = "Analysis";
            this.Analysis.Name = "Analysis";
            // 
            // Stock
            // 
            this.Stock.Items.Add(this.Traffic);
            this.Stock.Items.Add(this.tbtnAccountPanel);
            this.Stock.Label = "标签";
            this.Stock.Name = "Stock";
            // 
            // Traffic
            // 
            this.Traffic.Label = "用户量查询";
            this.Traffic.Name = "Traffic";
            this.Traffic.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
            // 
            // tbtnAccountPanel
            // 
            this.tbtnAccountPanel.Label = "分类体系";
            this.tbtnAccountPanel.Name = "tbtnAccountPanel";
            this.tbtnAccountPanel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.tbtnAccountPanel_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.Analysis);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.Analysis.ResumeLayout(false);
            this.Analysis.PerformLayout();
            this.Stock.ResumeLayout(false);
            this.Stock.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonGroup Stock;
        public Microsoft.Office.Tools.Ribbon.RibbonTab Analysis;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Traffic;
        internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton tbtnAccountPanel;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
