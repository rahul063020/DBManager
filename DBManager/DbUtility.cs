using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DbUtility
    {    
        public DateTime DateConverter(String Date,DateFormat dateFormat)
        {           
            DateTime dt = DateTime.Now;
            try
            {
                String[] splt = Date.Split('/');
                switch (dateFormat)
                {
                    case DateFormat.DDMMYYYY:
                        dt = new DateTime(Convert.ToInt32(splt[2]), Convert.ToInt32(splt[1]), Convert.ToInt32(splt[0]));
                        break;

                    case DateFormat.MMDDYYYY:
                        dt = new DateTime(Convert.ToInt32(splt[2]), Convert.ToInt32(splt[0]), Convert.ToInt32(splt[1]));
                        break;

                }

                return dt;
            }
            catch(Exception ex)
            {
                return dt;
            }
         
        }

        public bool IsColumnExist(String ColumnName,String TableName)
        {

            String columnOfTheTable = @"Select l.*,t.name TYPE_NAME from 
                             ( 
                             select * from sys.all_columns where object_id=
                            (select object_id from sys.tables where name='" + TableName + @"') )l 
                            Left outer join 
                           (select * from sys.types) t
                      on l.user_type_id=t.user_type_id ";
            DBMContext dbContxt = new DBMContext();
            List<TABLES_INFO> tblColumnlst = dbContxt.RetriveRecords<TABLES_INFO>(columnOfTheTable);
            if(tblColumnlst.Count>0)
            {
                if(tblColumnlst.FindAll(itm=>itm.name==ColumnName).Count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

    public enum DateFormat
    {
        DDMMYYYY=0,

        MMDDYYYY=1        


    }

    public enum ClauseOperator
    {
        NONE=0,
        AND=1,
        OR=2
    }
}
