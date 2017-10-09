using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace web.spider
{
    public class MySQLHelper
    {
        public MySQLHelper()
        {
        }
		/// <summary>  
		/// 数据库连接字符串  
		/// </summary>  
		private static String connectionString =ConfigurationManager.ConnectionStrings["conMysql"].ConnectionString;
		/// <summary>  
		/// 执行单条插入语句，并返回id，不需要返回id的用ExceuteNonQuery执行  
		/// </summary>  
		/// <param name="sql">SQL语句</param>  
		/// <param name="parameters">参数集合</param>  
		/// <returns>返回插入数据的id</returns>  
		public static int ExecuteInsert(string sql, MySqlParameter[] parameters)
		{
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, connection);
				try
				{
					connection.Open();
					if (parameters != null) cmd.Parameters.AddRange(parameters);
					cmd.ExecuteNonQuery();
					cmd.CommandText = @"select LAST_INSERT_ID()";
					int value = Int32.Parse(cmd.ExecuteScalar().ToString());
					return value;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
		public static int ExecuteInsert(string sql)
		{
			return ExecuteInsert(sql, null);
		}
		/// <summary>  
		/// 执行带参数的sql语句,返回影响的记录数（insert,update,delete)  
		/// </summary>  
		/// <param name="sql">SQL语句</param>  
		/// <param name="parameters">参数</param>  
		/// <returns>返回影响的记录数</returns>  
		public static int ExecuteNonQuery(string sql, MySqlParameter[] parameters)
		{
			//Debug.WriteLine(sql);  
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, connection);
				try
				{
					connection.Open();
					if (parameters != null) cmd.Parameters.AddRange(parameters);
					int rows = cmd.ExecuteNonQuery();
					return rows;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
		/// <summary>  
		/// 执行不带参数的sql语句，返回影响的记录数  
		/// </summary>  
		/// <param name="sql">SQL语句</param>  
		/// <returns>返回影响的记录数</returns>  
		public static int ExecuteNonQuery(string sql)
		{
			return ExecuteNonQuery(sql, null);
		}
		// <summary>  
		/// 执行单条语句返回第一行第一列,可以用来返回count(*)  
		/// </summary>  
		/// <param name="sql">SQL语句</param>  
		/// <param name="parameters">参数</param>  
		/// <returns>返回总数</returns>  
		public static int ExecuteScalar(string sql, MySqlParameter[] parameters)
		{
			//Debug.WriteLine(sql);  
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				MySqlCommand cmd = new MySqlCommand(sql, connection);
				try
				{
					connection.Open();
					if (parameters != null) cmd.Parameters.AddRange(parameters);
					int value = Int32.Parse(cmd.ExecuteScalar().ToString());
					return value;
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}
		/// <summary>  
		///  执行单条语句返回第一行第一列,可以用来返回count(*)  
		/// </summary>  
		/// <param name="sql">SQL语句</param>  
		/// <returns>   返回总数</returns>  
		public static int ExecuteScalar(string sql)
		{
			return ExecuteScalar(sql, null);
		}

		/// <summary>  
		/// 执行事务  
		/// </summary>  
		/// <param name="sqlList"></param>  
		/// <param name="paraList"></param>  
		public static void ExecuteTrans(List<string> sqlList, List<MySqlParameter[]> paraList)
		{
			//Debug.WriteLine(sql);  
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				MySqlCommand cmd = new MySqlCommand();
				MySqlTransaction transaction = null;
				cmd.Connection = connection;
				try
				{
					connection.Open();
					transaction = connection.BeginTransaction();
					cmd.Transaction = transaction;

					for (int i = 0; i < sqlList.Count; i++)
					{
						cmd.CommandText = sqlList[i];
						if (paraList != null && paraList[i] != null)
						{
							cmd.Parameters.Clear();
							cmd.Parameters.AddRange(paraList[i]);
						}
						cmd.ExecuteNonQuery();
					}
					transaction.Commit();

				}
				catch (Exception e)
				{
					try
					{
						transaction.Rollback();
					}
					catch
					{

					}
					throw e;
				}

			}
		}
		/// <summary>  
		/// 执行事务  
		/// </summary>  
		/// <param name="sqlList"></param>  
		public static void ExecuteTrans(List<string> sqlList)
		{
			ExecuteTrans(sqlList, null);
		}

		/// <summary>  
		/// 执行查询语句，返回dataset  
		/// </summary>  
		/// <param name="sql"></param>  
		/// <param name="parameters"></param>  
		/// <returns></returns>  
		public static DataSet ExecuteQuery(string sql, MySqlParameter[] parameters)
		{
			//Debug.WriteLine(sql);  
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				DataSet ds = new DataSet();
				try
				{
					connection.Open();

					MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
					if (parameters != null) da.SelectCommand.Parameters.AddRange(parameters);
					da.Fill(ds, "ds");
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return ds;
			}
		}
		/// <summary>  
		/// 执行查询语句，返回dataset  
		/// </summary>  
		/// <param name="sql"></param>  
		/// <returns></returns>  
		public static DataSet ExecuteQuery(string sql)
		{
			return ExecuteQuery(sql, null);
		}
	}
}
