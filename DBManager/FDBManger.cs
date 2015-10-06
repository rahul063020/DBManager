using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
   public class FDBManger:IDataManager
    {
        public void OpenConnection()
        {
           
        }

        public void BeginTransection()
        {
            
        }

        public string SaveRecords<T>(List<T> data)
        {
            return "";
        }

        public string Update<T>(List<T> data)
        {
            return "";
        }

        public string Erase<T>(List<T> data)
        {
            return "";
        }
        public string SaveDsRecords(DataSet data)
        {
            return "";
        }

        public string UpdateDs(DataSet data)
        {
            return "";
        }

        public string EraseDs(DataSet data)
        {
            return "";
        }
        public string ConnectionString { get; set; }

        public string Queries { get; set; }

        public bool IsTransectionOpen()
        {
            return true;
        }

        public List<T> GetRecords<T>()
        {
            return new List<T>();
        }

        public System.Data.DataSet GetDataSet()
        {
            return new DataSet();
        }

        public List<T> RetriveFiles<T>()
        {
            return new List<T>();
        }

        public void StoredFiles<T>(List<T> file)
        {
            
        }

        public byte[] RetriveFiles(string fileId)
        {
            byte[] bt=null;
            return bt;
        }

        public string SendEmailMessage(List<EMAIL> msgLst)
        {
            return "";
        }

        public string SendEmailMessage(EMAIL msgEmail)
        {
            return "";
        }

        public List<System.Data.SqlClient.SqlParameter> paramLst { get; set; }

        public void ExecuteReader()
        {
            
        }

        public void ExecuteQuery(string Query)
        {
            
        }

        public string CommandName { get; set; }

        public System.Data.IDataReader reader { get; set; }

        public void CommitTransection()
        {
           
        }

        public DbCommandType DMCommandType { get; set; }

        public string NextIncrementID<T>()
        {
            return "";
        }

        public bool ChecktableAlreadyExist<T>()
        {
            return true;
        }

        public void CreateNewtable<T>()
        {
           
        }

        public void AlterPreviousTable<T>()
        {
           
        }

        public string SqlQueryCreateNewtable<T>()
        {
            return "";
        }

        public string SqlQueryAlterPreviousTable<T>()
        {
            return "";
        }

        public bool IsConnectionOpen()
        {
            return true;
        }

        public void ExecuteQuery()
        {
           
        }

        public void Commit()
        {
           
        }

        public void RollBack()
        {
            
        }

        public void Dispose()
        {
           
        }


      
    }
}
