using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace DBManager
{
    public interface IDBMCommand
    {
        List<SqlParameter> paramLst { get; set; }
        DbCommandType commandType { get; set; }
        String CommandName { get; set; }
    }
}
