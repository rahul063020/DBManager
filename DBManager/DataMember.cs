using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBManager
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple=true,Inherited=true)]
    public class DataMember : Attribute
    {
        public DataMember()
        {
        }
       
        public String TABLE_NAME { get; set; }
        public String ID_FIELD { get; set; }
        public String ERASE_FIELD { get; set; }
        public String FILE_NAME_FIELD { get; set; }
        public String FILE_PATH_FIELD { get; set; }
        public String UPDATE_FIELD { get; set; }

        private bool _SYNC_TABLE = true;
        public bool SYNC_TABLE
        {
            get { return _SYNC_TABLE; }
            set { _SYNC_TABLE = value; }
        }

        public bool SYNC_PROERTY{ get; set; }


    }
}