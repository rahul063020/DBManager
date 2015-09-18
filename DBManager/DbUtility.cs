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
