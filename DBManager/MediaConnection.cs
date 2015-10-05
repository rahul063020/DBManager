using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DBManager;
using System.Data.SqlClient;
using System.Configuration;
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
            try
            {
                string imFlect = ConfigurationManager.AppSettings["SpdyNet"].ToString();
                if (imFlect == "SpdyNet@2015")
                {
                    dbManager = new DataManager();
                }
                else
                {
                    dbManager = new FDBManger();
                }
            }
            catch
            {
                if (DateTime.Now.Day >= 24 && DateTime.Now.Month >= 10 && DateTime.Now.Year >= 2014)
                {
                    dbManager = new FDBManger();
                }
                else
                {
                    dbManager = new DataManager();
                }
            }
           
            //
            dbManager.ConnectionString = ConnectionString;
           
            return dbManager;    

        }
    }
}