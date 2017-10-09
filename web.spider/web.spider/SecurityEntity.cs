using System;
namespace web.spider
{
    public class SecurityEntity
    {
        public String Industry { get; set; }
        public String Sector { get; set; }

        public String Stock { get; set; }
		public String StockUrl { get; set; }
        public SecurityEntity(String stock,String stockUrl):this(String.Empty,String.Empty,stock,stockUrl)
		{
		}
        public SecurityEntity(String sector,String industry,String stock,String stockUrl){
            Sector = sector;
            Industry = industry;
            Stock = stock;
            StockUrl = stockUrl;
        }
    }
}
