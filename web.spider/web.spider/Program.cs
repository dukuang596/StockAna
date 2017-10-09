using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Collections.Generic;

namespace web.spider
{
    class Program
    {
        static String domain = @"http://www.macrotrends.net";

        static void Main(string[] args)
        {

            testDb();
            //int res = CheckIsGoodProxy(doc); //这是我解析的函数，还没到那一步。不解释了。

        }
        static void testDb(){
            MySQLHelper.ExecuteInsert("insert into test values(1,'abc'");
        }
        static void getMarketMETAData(){
			var url = domain + @"/stocks/research";
			HtmlWeb web = new HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc = web.Load(url);


			HtmlNode rootnode = doc.DocumentNode;
			HtmlNodeCollection r = rootnode.SelectNodes("//div[@class='col-xs-6'][4]/table/tbody/tr");
			var first = false;
			var result = new List<SecurityEntity>();
			foreach (var node in r)
			{
				var t = node.SelectNodes("./td[1]").Nodes().ElementAt(0);
				var sectorName = t.InnerHtml;
				var sectorUrl = t.Attributes.ElementAt(0).Value;

				Console.WriteLine(sectorName);
				Console.WriteLine(sectorUrl);
				if (!first)
				{
					//var json=getJsData(sectorUrl);
					var l = getIndustryStockInfo(sectorName, sectorUrl);
					first = true;
					result.AddRange(l);
					break;
				}


			}

			int c = r.Count;
        }

        static String getJsData(String url){
            url = domain + url;
			HtmlWeb web = new HtmlWeb();
			HtmlAgilityPack.HtmlDocument doc = web.Load(url);


			HtmlNode rootnode = doc.DocumentNode;
            var r = rootnode.SelectNodes("//script").Nodes();
            foreach (var node in r)
            {
                if (node.InnerHtml.Contains("var data"))
                    return node.InnerHtml;
            }
            return String.Empty;
        }
        static String getHtmlString(String t){
			IWebDriver driver;
			//driver = new OpenQA.Selenium.PhantomJS.PhantomJSDriver();
            using (driver = new OpenQA.Selenium.PhantomJS.PhantomJSDriver()){
            //using(driver=new OpenQA.Selenium.Chrome.ChromeDriver()){
               
                driver.Navigate().GoToUrl(t);
                var result = "<html>" + driver.FindElement(By.TagName("html")).GetAttribute("innerHTML") + "</html>";
                driver.Quit();
                return result;

           
            }
			
			
		}
        static List<SecurityEntity> getStockInfo(String sector,String industry,String b){
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			doc.LoadHtml(getHtmlString(b));
			HtmlNode rootnode = doc.DocumentNode;
			HtmlNodeCollection rows = rootnode.SelectNodes("//div[@role='row']");
            var resutl = new List<SecurityEntity>();
			foreach (var row in rows)
			{
				var t = row.SelectNodes("./div[@role='gridcell'][1]/div").Nodes().ElementAt(0);
				var stockName = t.InnerHtml;
				var stockUrl = t.Attributes.ElementAt(0).Value;
                resutl.Add(new SecurityEntity(sector,industry,stockName,stockUrl));
				//Console.WriteLine(stockName);
				//Console.WriteLine(stockUrl);
			}
            return resutl;
        }
        static List<SecurityEntity> getIndustryStockInfo(String sector,String a){
			HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
			doc.LoadHtml(getHtmlString(domain+a));
			HtmlNode rootnode = doc.DocumentNode;
			HtmlNodeCollection rows = rootnode.SelectNodes("//div[@role='row']");
            var  resutl= new List<SecurityEntity>();
            var first = true;
			foreach (var row in rows)
			{
				var t = row.SelectNodes("./div[@role='gridcell'][1]/div").Nodes().ElementAt(0);
				var industryName = t.InnerHtml;
                var industryUrl = t.Attributes.ElementAt(0).Value;
                if(first){
                    var l=getStockInfo(sector,industryName,industryUrl);
                    resutl.AddRange(l);
                    //first = false;
                }
				//Console.WriteLine(industryName);
				//Console.WriteLine(industryUrl);
			}

            return resutl;
		}
    }
}
