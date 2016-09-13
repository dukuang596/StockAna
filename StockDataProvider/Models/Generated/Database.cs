



















// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `stock`
//     Provider:               `MySql.Data.MySqlClient`
//     Connection String:      `server=104.238.130.222;User Id=stock;password=**zapped**;default command timeout=8000`
//     Schema:                 ``
//     Include Views:          `False`



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace stock
{

	public partial class stockDB : Database
	{
		public stockDB() 
			: base("stock")
		{
			CommonConstruct();
		}

		public stockDB(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			stockDB GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static stockDB GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new stockDB();
        }

		[ThreadStatic] static stockDB _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        

		public class Record<T> where T:new()
		{
			public static stockDB repo { get { return stockDB.GetInstance(); } }
			public bool IsNew() { return repo.IsNew(this); }
			public object Insert() { return repo.Insert(this); }

			public void Save() { repo.Save(this); }
			public int Update() { return repo.Update(this); }
            public static object BatchInsert(List<T> objs) { return repo.BatchInsert<T>(objs); }
            public int Update(IEnumerable<string> columns) { return repo.Update(this, columns); }
			public static int Update(string sql, params object[] args) { return repo.Update<T>(sql, args); }
			public static int Update(Sql sql) { return repo.Update<T>(sql); }
			public int Delete() { return repo.Delete(this); }
			public static int Delete(string sql, params object[] args) { return repo.Delete<T>(sql, args); }
			public static int Delete(Sql sql) { return repo.Delete<T>(sql); }
			public static int Delete(object primaryKey) { return repo.Delete<T>(primaryKey); }
			public static bool Exists(object primaryKey) { return repo.Exists<T>(primaryKey); }
			public static bool Exists(string sql, params object[] args) { return repo.Exists<T>(sql, args); }
			public static T SingleOrDefault(object primaryKey) { return repo.SingleOrDefault<T>(primaryKey); }
			public static T SingleOrDefault(string sql, params object[] args) { return repo.SingleOrDefault<T>(sql, args); }
			public static T SingleOrDefault(Sql sql) { return repo.SingleOrDefault<T>(sql); }
			public static T FirstOrDefault(string sql, params object[] args) { return repo.FirstOrDefault<T>(sql, args); }
			public static T FirstOrDefault(Sql sql) { return repo.FirstOrDefault<T>(sql); }
			public static T Single(object primaryKey) { return repo.Single<T>(primaryKey); }
			public static T Single(string sql, params object[] args) { return repo.Single<T>(sql, args); }
			public static T Single(Sql sql) { return repo.Single<T>(sql); }
			public static T First(string sql, params object[] args) { return repo.First<T>(sql, args); }
			public static T First(Sql sql) { return repo.First<T>(sql); }
			public static List<T> Fetch(string sql, params object[] args) { return repo.Fetch<T>(sql, args); }
			public static List<T> Fetch(Sql sql) { return repo.Fetch<T>(sql); }
			public static List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args) { return repo.Fetch<T>(page, itemsPerPage, sql, args); }
			public static List<T> Fetch(long page, long itemsPerPage, Sql sql) { return repo.Fetch<T>(page, itemsPerPage, sql); }
			public static List<T> SkipTake(long skip, long take, string sql, params object[] args) { return repo.SkipTake<T>(skip, take, sql, args); }
			public static List<T> SkipTake(long skip, long take, Sql sql) { return repo.SkipTake<T>(skip, take, sql); }
			public static Page<T> Page(long page, long itemsPerPage, string sql, params object[] args) { return repo.Page<T>(page, itemsPerPage, sql, args); }
			public static Page<T> Page(long page, long itemsPerPage, Sql sql) { return repo.Page<T>(page, itemsPerPage, sql); }
			public static IEnumerable<T> Query(string sql, params object[] args) { return repo.Query<T>(sql, args); }
			public static IEnumerable<T> Query(Sql sql) { return repo.Query<T>(sql); }

		}

	}
	



    

	[TableName("stock.stock_history_second_bar")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class stock_history_second_bar : stockDB.Record<stock_history_second_bar>  
    {



		[Column] public long id { get; set; }





		[Column] public string symbol { get; set; }





		[Column] public uint? tick { get; set; }





		[Column] public decimal? high { get; set; }





		[Column] public decimal? low { get; set; }





		[Column] public decimal? open { get; set; }





		[Column] public decimal? close { get; set; }





		[Column] public long? volume { get; set; }





		[Column] public int? count { get; set; }





		[Column] public decimal? wap { get; set; }





		[Column] public int? hasgap { get; set; }



	}

    

	[TableName("stock.stock_snapshot_raw")]



	[PrimaryKey("id")]




	[ExplicitColumns]

    public partial class stock_snapshot_raw : stockDB.Record<stock_snapshot_raw>  
    {



		[Column] public long id { get; set; }





		[Column] public uint? dt { get; set; }





		[Column] public string symbol { get; set; }





		[Column] public ulong? last_tick { get; set; }





		[Column] public decimal? last_price { get; set; }





		[Column] public ulong? last_volume { get; set; }





		[Column] public ulong? day_volume { get; set; }





		[Column] public decimal? day_wap { get; set; }





		[Column] public int? is_one { get; set; }



	}


}
