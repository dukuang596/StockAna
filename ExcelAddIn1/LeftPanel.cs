using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestExcelAddin
{
    public partial class LeftPanel : UserControl
    {
        public LeftPanel()
        {
            InitializeComponent();
        }

        private void LeftPanel_Load(object sender, EventArgs e)
        {
            TreeNode j = new TreeNode("金融");
            this.treeView1.Nodes.Add(j);
            j.Nodes.Add("信用卡");
            this.treeView1.Nodes.Add("房产家居");
            //treeView1.Nodes.Add()
        }
    }
}
