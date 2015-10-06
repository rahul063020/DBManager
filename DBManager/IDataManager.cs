using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBManager
{
   public interface IDataManager
    {        
        void OpenConnection();
        void BeginTransection();
        String SaveRecords<T>(List<T> data);
        String Update<T>(List<T> data);
        String Erase<T>(List<T> data);
        String SaveDsRecords(DataSet data);
        String UpdateDs(DataSet data);
        String EraseDs(DataSet data);
        String ConnectionString { get; set; }
        String Queries { get; set; }
        bool IsTransectionOpen();
        List<T> GetRecords<T>();
        DataSet GetDataSet();
        List<T> RetriveFiles<T>();
        void StoredFiles<T>(List<T> file);
        Byte[] RetriveFiles(String fileId);
        String SendEmailMessage(List<EMAIL> msgLst);
        String SendEmailMessage(EMAIL msgEmail);
        List<SqlParameter> paramLst { get; set; }
        void ExecuteReader();
        void ExecuteQuery(String Query);
        String CommandName { get;set;}
        IDataReader reader { get; set; }
        void CommitTransection();
        DbCommandType DMCommandType { get; set; }
        String NextIncrementID<T>();
        bool ChecktableAlreadyExist<T>();
        void CreateNewtable<T>();
        void AlterPreviousTable<T>();
        String SqlQueryCreateNewtable<T>();
        String SqlQueryAlterPreviousTable<T>();
        bool IsConnectionOpen();
        void ExecuteQuery();
        void Commit();
        void RollBack();
        void Dispose();
    }
}