using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DBManager
{
    public class DBMContext :IDisposable
    {
        IDataManager dbManager;
        public String Message { get; set; }

        public DBMContext(String conString)
        {
            MediaConnection.Instance().ConnectionString = conString;          
            dbManager = MediaConnection.Instance().CreateConnection();                    
            OpenConnection();
        }
        public DBMContext()
        {
            dbManager = MediaConnection.Instance().CreateConnection();            
            OpenConnection();
        }

        /// <summary>
        /// Open a Data Manager Connection
        /// </summary>
        private void OpenConnection() 
        {
            try
            {
                if (!dbManager.IsTransectionOpen())
                {
                    dbManager.OpenConnection();
                    dbManager.DMCommandType = DbCommandType.CommnadTest;
                    dbManager.BeginTransection();
                }
            }
            catch (SqlException ex)
            {
                Message = "ERROR:" + Environment.NewLine + " CODE:" + ex.ErrorCode + Environment.NewLine +
                                    " Error Messaage:" +
                                    Environment.NewLine + ex.Message;
                dbManager.RollBack();
            }
        }

        /// <summary>
        /// Get Next Auto Generated ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public String NextIncrementID<T>() 
        {
            String ID = "";
            try 
            {
              ID=  dbManager.NextIncrementID<T>();
            }
            catch (Exception ex) 
            { 
            
            }
            return ID;
        }

        /// <summary>
        /// Retrive DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet RetriveDataSet(String sql)
        {
            DataSet ds = new DataSet();
            try
            {
                dbManager.Queries = sql;
                ds = dbManager.GetDataSet();                
            }
            catch (SqlException ex)
            {
                Message = "ERROR:" + Environment.NewLine + " CODE:" + ex.ErrorCode + Environment.NewLine +
                                    " Error Messaage:" +
                                    Environment.NewLine + ex.Message;
            }
            return ds;
        }

        /// <summary>
        /// Execute Stored Procedure Command
        /// </summary>
        /// <param name="dbmcommand"></param>
        public void ExecuteSpCommand(IDBMCommand dbmcommand) 
        {
            try 
            {
                dbManager.DMCommandType = dbmcommand.commandType;
                dbManager.paramLst = dbmcommand.paramLst;
                dbManager.CommandName = dbmcommand.CommandName;
            }
            catch (Exception ex) 
            {
            }
        }

        /// <summary>
        /// Retrive  Records with Query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">Pass Query</param>
        /// <returns></returns>
        public List<T> RetriveRecords<T>(String sql) 
        {
            List<T> retUnLst = new List<T>();
            try
            {
                dbManager.Queries = sql;
                retUnLst = dbManager.GetRecords<T>();
            }
            catch (SqlException ex)
            {
                Message = "ERROR:" + Environment.NewLine + " CODE:" + ex.ErrorCode + Environment.NewLine +
                                    " Error Messaage:" +
                                    Environment.NewLine + ex.Message;
            }
            return retUnLst;
        }


        /// <summary>
        /// Retrive All Records of a Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> RetriveRecords<T>()
        {
            List<T> retUnLst = new List<T>();
            try
            {
                retUnLst = dbManager.GetRecords<T>();
                            
            }
            catch (SqlException ex)
            {
                Message = "ERROR:" + Environment.NewLine + " CODE:" + ex.ErrorCode + Environment.NewLine +
                                    " Error Messaage:" +
                                    Environment.NewLine + ex.Message;
            }
            return retUnLst;
        }

        /// <summary>
        /// Save List of Records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        public void SaveRecords<T>(List<T> records)
        {
            dbManager.SaveRecords<T>(records);
        }

        /// <summary>
        /// Update List of Records
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records"></param>
        /// <param name="IDField"></param>
        public void UpdateRecords<T>(List<T> records)
        {
            dbManager.Update<T>(records);          
        }

        /// <summary>
        /// Delete Records From DataBase
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deltLst"></param>
        public void DeleteRecords<T>(List<T> deltLst)
        {
            dbManager.Erase<T>(deltLst);
        }

        /// <summary>
        /// Run Singel Line Sql Query 
        /// </summary>
        /// <param name="query"></param>
        public void RunLineQuery(String query)
        {
            dbManager.Queries +=" "+ query +" ";
        }

        /// <summary>
        /// Convert a List from One to another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="provideLst"></param>
        /// <returns></returns>
        public List<U> GenerateList<T,U>(List<T> provideLst)
        {
            IProduction pro = new Production();
            return pro.ConvertList<T, U>(provideLst);

        }

 
        /// <summary>
        /// SAVE CHANGES-- CALL AFTER CHANGING TO DB
        /// </summary>
        private void SaveChanges()
        {
            try
            {
                if (!dbManager.IsTransectionOpen())
                {
                    dbManager.OpenConnection();
                    dbManager.BeginTransection();
                }
                if (dbManager.Queries != null && !dbManager.Queries.Equals("") ) 
                {
                    dbManager.ExecuteQuery();                   
                    dbManager.Queries = "";
                }
                if (dbManager.CommandName != null && !dbManager.CommandName.Equals("") && dbManager.DMCommandType.Equals(DbCommandType.StoredProcedure))
                {
                    dbManager.ExecuteQuery();
                    dbManager.CommandName = "";
                }
                if (dbManager.reader!=null)
                {
                    dbManager.reader.Close();
                }

                dbManager.CommitTransection();
                    Message = "SUCCESS";
                 
                
            }
            catch (Exception ex) {
                Message = "ERROR:" + Environment.NewLine+  
                                    " Error Messaage:" + 
                                    Environment.NewLine+ ex.Message;
                dbManager.RollBack();
            }
            
        }
      
        /// <summary>
        /// Dispose DataManager
        /// </summary>
        public void Dispose()
        {
            SaveChanges();
            dbManager.Dispose();
        }

        /// <summary>
        /// Convert a DataTable to List Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> TableToModel<T>(DataTable dt)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            for (var j = 0; j < dt.Rows.Count; j++)
            {
                var objT = Activator.CreateInstance<T>();
                foreach (DataColumn dc in dt.Columns)
                {
                    var setObj = dt.Rows[j][dc.ColumnName].ToString();
                    foreach (var pro in properties)
                    {
                        try
                        {
                            if (pro.Name.Equals(dc.ColumnName))
                            {
                                pro.SetValue(objT, setObj, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                    lst.Add(objT);
                }
            }

            return lst;

        }

        /// <summary>
        /// To Save File 
        /// TABLE NAME: FILES
        /// </summary>
        /// <param name="file">Posted File</param>
        /// <returns>Success Message</returns>
        public String SaveFile(HttpPostedFile file)
        {
            String msg = "";
            List<FILES> fLst = new List<FILES>();
            bool isExists = Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/TempFiles"));
            if (!isExists)
                Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/TempFiles"));
            file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(file.FileName));
            FILES newFileMdl = new FILES();
            newFileMdl.NAME = file.FileName;
            newFileMdl.FILE_TYPE = file.ContentType;
            newFileMdl.FILE_DATA = File.ReadAllBytes(System.Web.HttpContext.Current.Server.MapPath(file.FileName));
            fLst.Add(newFileMdl);
            try
            {
                dbManager.OpenConnection();
                dbManager.BeginTransection();
                dbManager.StoredFiles<FILES>(fLst);
                dbManager.ExecuteQuery();
            }
            catch (Exception ex)
            {
                Message += "ERROR :: " +Environment.NewLine+ ex.Message;
            }
            finally
            {
            }

            return msg;
        }

        /// <summary>
        /// Retrive Files WIth Query 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public List<T> FilesRecords<T>()
        {
            List<T> passLst = new List<T>();
            String msg = "";
            try
            {
                passLst = dbManager.RetriveFiles<T>();
            }
            catch (Exception ex)
            {
                dbManager.RollBack();
                msg = ex.Message;
            }
            finally
            {
                dbManager.Dispose();
            }
            return passLst;
        }


        /// <summary>
        /// Send Email Notification
        /// </summary>
        /// <param name="emlLst">List of Email Model</param>
        /// <returns></returns>
        public String SendEmail(List<EMAIL> emlLst) 
        {
          return  dbManager.SendEmailMessage(emlLst);
        }

        /// <summary>
        /// Send Email Notification
        /// </summary>
        /// <param name="eml">Email Model</param>
        /// <returns></returns>
        public String SendEmail(EMAIL eml)
        {
            return dbManager.SendEmailMessage(eml);
        }

        /// <summary>
        /// Produce a List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">DataReaqder</param>
        /// <returns></returns>
        public List<T> FillUpList<T>(IDataReader reader) 
        {
            IProduction prduce = new InitializeProduction().Initalize<T>();
            return prduce.produceList<T>(reader);        
        }

        /// <summary>
        /// Produce DataSet
        /// </summary>
        /// <param name="dataAdapter">Sql DataAdapter</param>
        /// <returns></returns>
        public DataSet FillDataset(SqlDataAdapter dataAdapter)
        {
            IProduction prduce = new Production();
            return prduce.produceDataSetRecords(dataAdapter);
        }

       /// <summary>
        /// CREATE OR ALTER ENTITY(TABLES)
       /// </summary>
       /// <typeparam name="T"></typeparam>
        public void ExecuteEntity<T>()
        {
            if (dbManager.ChecktableAlreadyExist<T>())
                dbManager.AlterPreviousTable<T>();
            else
               dbManager.CreateNewtable<T>();
        }

        /// <summary>
        /// Synd Entity with Database
        /// </summary>
        public void SyncEntity()
        {
            List<Type> typeList = MediaConnection.Instance().AllEntity;
            var typeOfContext = this.GetType();
            String query = "";
            var method = typeOfContext.GetMethod("SqlQueryExecuteEntity");
                String htmlMsgHead = @"<table>
                                <thead> <th>Entity Name </th>
                                <th>  Message </th>
                                </thead>";
                String htmlMessageBody = "";

                String htmlMessageFooter = "</table>";
            foreach (Type tp in typeList)
            {
                    try
                    {
                        if (!dbManager.IsTransectionOpen())
                        {
                            if (dbManager.IsConnectionOpen())
                            {
                                dbManager.OpenConnection();
                                dbManager.BeginTransection();
                            }
                        }
                        else if (!dbManager.IsConnectionOpen())
                        {
                            dbManager.OpenConnection();
                            dbManager.BeginTransection();
                        }
                        var genericMethod = method.MakeGenericMethod(tp);
                        genericMethod.Invoke(this, null);
                        query = genericMethod.Invoke(this, null).ToString();
                        if (query != null && !query.Equals(""))
                        {
                            dbManager.ExecuteQuery(query);
                            dbManager.Queries = "";
                        }
                        if (dbManager.CommandName != null && !dbManager.CommandName.Equals("") && dbManager.DMCommandType.Equals(DbCommandType.StoredProcedure))
                        {
                            dbManager.ExecuteQuery();
                            dbManager.CommandName = "";
                        }
                        if (dbManager.reader != null)
                        {
                            dbManager.reader.Close();
                        }

                        htmlMessageBody += @"<tr>
                                            <td>"+tp.Name+@"</td>
                                            <td>SUCCESS</td>
                                            </tr>";
                        dbManager.CommitTransection();
                    }
                    catch (Exception ex)
                    {
                        //if (Message == null)
                        //{
                            Message = "ERROR:" + Environment.NewLine +
                                        "ENTITY:"+Environment.NewLine+
                                        tp.Name+" "+Environment.NewLine+
                                        " Error Messaage:" +
                                        Environment.NewLine + ex.Message;
                            String msg = ex.Message.Contains("[] are not allowed") ? " DATAMEMBER filed(s)  not provided " : ex.Message;
                       
                            htmlMessageBody += @"<tr>
                                            <td>" + tp.Name + @"</td>
                                            <td>" + msg + @"</td>
                                            </tr>";
                        //}
                        //else
                        //{
                        //    Message += "ERROR:" + Environment.NewLine +
                        //                    " Error Messaage:" +
                        //                    Environment.NewLine + ex.Message;
                        //}

                        dbManager.RollBack();
                    }
                    finally
                    {
                        dbManager.Dispose();

                    }

                    Message= htmlMsgHead + htmlMessageBody + htmlMessageFooter;
            }
            
        }

        public String SqlQueryExecuteEntity<T>()
        {
            if (dbManager.ChecktableAlreadyExist<T>())
               return dbManager.SqlQueryAlterPreviousTable<T>();
            else
                return dbManager.SqlQueryCreateNewtable<T>();
        }

       /// <summary>
        /// Synd Entity with Database
        /// </summary>
        public void SyncEntityDb<T>()
        {
            List<Type> theList = new List<Type>();
            MediaConnection.Instance().AllEntity = theList;
            theList.Add(typeof(T));
            var context = new DBMContext();
            context.SyncEntity();
        }
    }
}