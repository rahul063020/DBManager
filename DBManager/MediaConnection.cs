using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DBManager;
using System.Data.SqlClient;
namespace DBManager
{
    public class MediaConnection
    {
        private MediaConnection() { }

        private static MediaConnection _instance=null;

        public static MediaConnection Instance() {
            if (_instance == null)
                _instance = new MediaConnection();
            return _instance;
        }
        public String ConnectionString { get; set; }

        public List<Type> AllEntity { get; set; } 

        public IDataManager CreateConnection()
        {
            IDataManager dbManager;
            dbManager = new DataManager();
            dbManager.ConnectionString = ConnectionString;
            return dbManager;    

        }
    }
}