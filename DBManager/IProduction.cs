using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBManager
{
   public interface IProduction
    {
        List<T> produceList<T>(IDataReader reader);
        String produceInsertQuery<T>(List<T> records);
        String produceUpdateQuery<T>(List<T> records);
        String produceDeleteQuery<T>(List<T> records);
        String produceAutoIncrementIDQuery();
        DataSet produceDataSetRecords(SqlDataAdapter dataAdapter);
        Byte[] BytesArray<T>(List<T> file);
        List<T> FilePath<T>(List<T> file);
        List<U> ConvertList<T, U>(List<T> provideLst);
        String GetSingleLineQuery();
        String ID_FIELD { get; set; }
        String TABLE_NAME { get; set; }
        String FILE_NAME_FIELD { get; set; }
        String FILE_PATH_FIELD { get; set; }
        String UPDATE_FIELD { get; set; }
        List<ERASE_FIELD> ERASE_FIELD { get; set; }
        String CreateQuery<T>(List<GENERI_CCLAUSE<T>> clause);
    }
}