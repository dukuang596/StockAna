using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Workflow.Activities.Rules;
using Autofac;
using Autofac.Configuration;
using Microsoft.VisualBasic;
using Stock.Common;
using Stock.DataProvider;
using Stock.RuleEngine;


namespace TestRuleEngine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RegisterStockTragedy();
        }

        private Autofac.IContainer container;
        private void RegisterStockTragedy()
        {
            var builder = new ContainerBuilder();        
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));   
            container = builder.Build();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
          
            try
            {
                var
              k =await  AccessTheWebAsync();
                button1.Text = k.ToString();
                button2.Text = "abc";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               // throw ex; 
            }
          

            //var b = 10%10;
            //Stock.ComplexFunc.Add("testk", (stockname,paramDict) => { return true; });
            //Stock s = new Stock { StockCode = "aapl", LYR = 13.88M, TTM =19.52M, MRQ = 5.97M };
            //s.Indictors.Add("sma", 130);

            //RuleValidation validation = new RuleValidation(typeof(Stock), null);
            //RuleParser parser = new RuleParser(validation);
            //RuleCondition condition = parser.ParseCondition("LYR <= 30 && Indictors[\"sma\"]<135 && CallFunc(\"testk\")");
            //RuleAction thenAction = parser.ParseAction("EngineResult = 100");
            //RuleAction elseAction = parser.ParseAction("EngineResult =101");


            //RuleSet rset=new RuleSet();
            //var r = new System.Workflow.Activities.Rules.Rule("LYR", condition, new List<RuleAction> { thenAction }, new List<RuleAction> { elseAction });
            //rset.Rules.Add(r);

            //RuleHelper<Stock> rhelper = new RuleHelper<Stock>();
            //rhelper.SetRules(rset);
            //rhelper.Execute(s, true);
            //RuleEngine engine = new RuleEngine(rset, validation);
            //engine.Execute(s);

        }
         Task<double> GetValueAsync(double num1, double num2)
        {
            return Task.Run(() =>
            {
                
               throw  new Exception("ttt");
                for (int i = 0; i < 1000000; i++)
                {
                    num1 = num1 / num2;
                }
                return num1;
            });
        }
        async Task<double> AccessTheWebAsync()
        {

            var result= await GetValueAsync(1234.5, 1.01);
            return result;

        }
         
        
        //ContractSamples cs = new ContractSamples();

        private void button2_Click(object sender, EventArgs e)
        {

            //ContractSamples.StartDataServer();
            //var start = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 8, 30, 0);
            //var end = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 8, 30, 0);
            //var symbols = rtbSymbols.Text.Split(new char[] {ControlChars.Cr,ControlChars.Lf},StringSplitOptions.RemoveEmptyEntries);

            ////var symbol = "baba";
            //while (start <= end)
            //{
            //    foreach (var symbol in symbols)
            //    {
            //        if(rbt_minute.Checked)
            //            container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).ReqMinuteHistaryData(symbol, start, start.AddHours(10));

            //        else
            //            container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).ReqSecondHistaryData(symbol, start, start.AddHours(10));

            //    }
            //    //DataSaver.SaveMinuteData(symbol, data);
            //    start = start.AddDays(1);
            //}
            DataWorkParameter dwp=new DataWorkParameter()
            {
                StartDay = new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 8, 30, 0),
                EndDay = new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 8, 30, 0),
                Scale = rbt_minute.Checked?1:(rbt15second.Checked?2:0),
                Symbols = rtbSymbols.Text.Split(new char[] { ControlChars.Cr, ControlChars.Lf }, StringSplitOptions.RemoveEmptyEntries)
            };
            backgroundWorker1.RunWorkerAsync(dwp);
            
            // data = container.Resolve<IStockDataProvider>(new NamedParameter("provider","IB")).GetDailyHistoryData("amzn", new DateTime(2015, 4, 14), new DateTime(2015, 5, 14));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).Connect();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            try {
                container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).Disconnet();
            }catch(Exception){
            
            }
            
            base.OnClosing(e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var data = container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).ReqTickData("aapl", new DateTime(2015, 3, 14), new DateTime(2015, 5, 14));

        }

        private void BtTimezone_Click(object sender, EventArgs e)
        {
            var est = Util.ConvertFromUtcIntToEst(1461245278);
            //var est=Util.ConvertDateTimeFromUTC2EST(Util.ConvertIntDateTime(1461245278, TimeZoneInfo.FindSystemTimeZoneById("UTC"))) ;
            Console.WriteLine(est);
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            start_date.Value = Util.ConvertDateTimeZoneByZoneId(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), "China Standard Time", "US Eastern Standard Time");
            end_date.Value = start_date.Value;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var para = (DataWorkParameter) e.Argument;
            var start = para.StartDay;//new DateTime(start_date.Value.Year, start_date.Value.Month, start_date.Value.Day, 8, 30, 0);
            var end = para.EndDay; //new DateTime(end_date.Value.Year, end_date.Value.Month, end_date.Value.Day, 8, 30, 0);
            var symbols = para.Symbols;// rtbSymbols.Text.Split(new char[] { ControlChars.Cr, ControlChars.Lf }, StringSplitOptions.RemoveEmptyEntries);
            var scale = para.Scale;
            //var symbol = "baba";
            while (start <= end)
            {
                foreach (var symbol in symbols)
                {
                    if (scale==1)
                        container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).ReqMinuteHistaryData(symbol, start, start.AddHours(10));
                    else if (scale == 2)
                        container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).Req15SecondHistaryData(symbol, start, start.AddHours(10));

                    else
                        container.Resolve<IStockDataProvider>(new NamedParameter("provider", "IB")).ReqSecondHistaryData(symbol, start, start.AddHours(10));

                }
                //DataSaver.SaveMinuteData(symbol, data);
                start = start.AddDays(1);
            }
        }
    }
}
