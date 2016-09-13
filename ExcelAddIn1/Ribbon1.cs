using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Tools;
using CustomTaskPane = Microsoft.Office.Tools.CustomTaskPane;
using Microsoft.Office.Core;
using Common.Container.Management;

namespace TestExcelAddin
{
    public partial class Ribbon1
    {
        CustomTaskPane customPanel;
        LeftPanel left;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            this.tbtnAccountPanel.Checked = false;
        }
        void customPanel_VisibleChanged(object sender, EventArgs e)
        {
            //CustomTaskPane customPanel = customPanel;
            this.tbtnAccountPanel.Checked = customPanel.Visible;
        }
        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            using (var form = new PvUvSelector())
            {
                AdditionalServiceContainer.GetService<DialogService>().ShowDialog(form);
            }
 
        }

        private void tbtnAccountPanel_Click(object sender, RibbonControlEventArgs e)
        {
            if (left == null) {
                left=new LeftPanel();
                customPanel = ThisAddIn.GetInstanceObject().CustomTaskPanes.Add(left, "账户");
                customPanel.Width = 250;
                customPanel.DockPosition = MsoCTPDockPosition.msoCTPDockPositionLeft;
                customPanel.DockPositionRestrict = MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
                customPanel.VisibleChanged += new EventHandler(customPanel_VisibleChanged);
            }
            //this.tbtnAccountPanel.Checked = !this.tbtnAccountPanel.Checked;
            customPanel.Visible = this.tbtnAccountPanel.Checked;
        }
    }
}
