using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRuleEngine
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // let's check the context here
            //var context = SynchronizationContext.Current;
            //if (context == null)
            //    MessageBox.Show("No context for this thread");
            //else
            //    MessageBox.Show("We got a context");
            //if (context == null)
            //    MessageBox.Show("No context for this thread");
            //else
            //    MessageBox.Show("We got a context");

            // create a form
            Form1 form = new Form1();

            Application.Run(new Form1());
        }
    }
}
