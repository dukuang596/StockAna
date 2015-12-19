using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebPageWatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //控制日期或时间的显示格式 
            this.endTime.Value = DateTime.Today.AddDays(1).AddMinutes(-1);

        }
        private string GetDocument(string url)
        {
            HttpWebRequest request;
            url = url.Trim('"');
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return "";
            }

            request.Method = "GET";
            request.ContentType = "text/html";
            HttpWebResponse response;
            request.AllowAutoRedirect = true;
            //request.
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string content = stream.ReadToEnd();
                        response.Close();
                        return content;

                    }

                }
            }
            catch (System.Net.WebException ex)
            {

                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                request.Abort();
            }


            return "";

        }
        private delegate void InvokeCallback();
        private void setButtonEnable()
        {

            if (this.button1.InvokeRequired)
            {
                InvokeCallback d = new InvokeCallback(setButtonEnable);
                this.Invoke(d, null);
            }
            else
            {
                this.button1.Enabled = true;
            }
            return;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;

            ThreadPool.QueueUserWorkItem(obj =>
            {
                //Thread.Sleep(1000 * 10);
                while (!hasEndTime.Checked || DateTime.Now < endTime.Value)
                {
                    var contents = GetDocument(this.watchUrl.Text);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(contents);
                    var root = doc.DocumentNode;
                    ///html/body/div[3]/div/div[1]/div/div[1]
                    var nodes = root.SelectNodes(this.groupxpath.Text);
                    foreach (var node in nodes)
                    {
                        if (node.InnerHtml.Contains(this.spec.Text))
                        {
                            MessageBox.Show(this.spec.Text+"来了！！！");
                            setButtonEnable();
                        }
                    }
                    Thread.Sleep(1000 * 60);
                }
                button1.Enabled = true;
            }, null);

        }
    }
}
