using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Web;
using System.Data.SqlClient;


    public class AccessHelper
    {
        protected static SqlConnection conn = new SqlConnection();
        protected static SqlCommand comm = new SqlCommand();

        public AccessHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        private static void openConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                ///----conn.ConnectionString = "Provider=Microsoft.Jet.Sql.4.0;Data Source= "+HttpContext.Current.Server.MapPath("res\\DB\\stu133.mdb")+" ";

                conn.ConnectionString = "server=;database=SmartAlumnl;uid=sa;pwd=sa";
                comm.Connection = conn;
                try
                {
                    conn.Open();
                }
                catch (Exception e)
                { throw new Exception(e.Message); }

            }

        }
        /// <summary>
        /// 关闭数据库
        /// </summary>
        private static void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                comm.Dispose();
            }
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sqlstr"></param>
        public static void excuteSql(string sqlstr)
        {
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            { closeConnection(); }
        }
        /// <summary>
        /// 返回指定sql语句的SqlDataReader对象，使用时请注意关闭这个对象。
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static SqlDataReader dataReader(string sqlstr)
        {
            SqlDataReader dr = null;
            try
            {
                openConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;

                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                    closeConnection();
                }
                catch { }
            }
            return dr;
        }
        /// <summary>
        /// 返回指定sql语句的SqlDataReader对象,使用时请注意关闭
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="dr"></param>
        public static void dataReader(string sqlstr, ref SqlDataReader dr)
        {
            try
            {
                openConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                }
                catch
                {
                }
                finally
                {
                    closeConnection();
                }
            }
        }
        /// <summary>
        /// 返回指定sql语句的dataset
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataSet dataSet(string sqlstr)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return ds;
        }
        /// <summary>
        /// 返回指定sql语句的dataset
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="ds"></param>
        public static void dataSet(string sqlstr, ref DataSet ds)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
        }
        /// <summary>
        /// 返回指定sql语句的datatable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataTable dataTable(string sqlstr)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return dt;
        }
        /// <summary>
        /// 返回指定sql语句的datatable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="dt"></param>
        public static void dataTable(string sqlstr, ref DataTable dt)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
        }
        /// <summary>
        /// 返回指定sql语句的dataview
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataView dataView(string sqlstr)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataView dv = new DataView();
            DataSet ds = new DataSet();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);
                dv = ds.Tables[0].DefaultView;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return dv;
        }
    }
