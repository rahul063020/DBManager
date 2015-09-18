using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public interface IDDLProduction
    {
        String ID_FIELD { get; set; }
        String TABLE_NAME { get; set; }
        String FILE_NAME_FIELD { get; set; }
        String FILE_PATH_FIELD { get; set; }
        String UPDATE_FIELD { get; set; }
        bool SYNC_TABLE { get; set; }
        List<SYNC_PROP> LIST_SYNC_PRO { get; set; }
        List<ERASE_FIELD> ERASE_FIELD { get; set; }
        String CreateNewTable<T>();
        String AlterTable<T>();
        String AvailibilityQuery();
        List<T> produceList<T>(IDataReader reader);
    }
}
