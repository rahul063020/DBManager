using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace DBManager
{
   public class DBMCommand : IDBMCommand
    {
       public DBMCommand() 
       {
           commandType = DbCommandType.StoredProcedure;
       }

       public List<SqlParameter> paramLst { get; set; }

       public DbCommandType commandType { get; set; }

       public string CommandName { get; set; }
    }
}
