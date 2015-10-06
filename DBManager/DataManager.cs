using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DBManager;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace DBManager
{
    public class DataManager : IDataManager
    {
        SqlTransaction transection;
        SqlConnection connection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        public String ConnectionString { get; set; }
        public List<SqlParameter> paramLst { get; set; }
        public String Queries { get; set; }
        public string CommandName { get; set; }
        public IDataReader reader { get; set; }
        public DbCommandType DMCommandType { get; set; }
        public IDataManager dbManger {get;set;}

        public void OpenConnection()
        {
            connection.ConnectionString = ConnectionString;
            connection.Open();
        }
        public void BeginTransection()
        {
            transection = connection.BeginTransaction("SQL TRANSECTION");
            command.Connection = connection;
            command.Transaction = transection;
            if (DMCommandType.Equals(DbCommandType.CommnadTest))
                command.CommandType = CommandType.Text;
            else if(DMCommandType.Equals(DbCommandType.StoredProcedure))
                command.CommandType = CommandType.StoredProcedure;
        }
        
        public String SaveRecords<T>(List<T> data)
        {
            IProduction produc = new InitializeProduction().Initalize<T>();
            Queries +=" "+ produc.produceInsertQuery<T>(data);
            return "";
        }
        public String Update<T>(List<T> data)
        {
            IProduction produc = new InitializeProduction().Initalize<T>();
            Queries += " " + produc.produceUpdateQuery<T>(data);
            return "";
        }
        public String Erase<T>(List<T> data)
        {
            IProduction produc = new InitializeProduction().Initalize<T>();
            Queries += " " + produc.produceDeleteQuery<T>(data);
            return "";
        }

        public string SaveDsRecords(DataSet data)
        {
            IProduction produc = new InitializeDSProduction().Initalize();
            Queries += " " + produc.produceDataSetInsertQuery(data);
            return "";
        }

        public string UpdateDs(DataSet data)
        {
            IProduction produc = new InitializeDSProduction().Initalize();
            Queries += " " + produc.produceDataSetUpdateQuery(data);
            return "";
        }

        public string EraseDs(DataSet data)
        {
            IProduction produc = new InitializeDSProduction().Initalize();
            Queries += " " + produc.produceDataSetEraseQuery(data);
            return "";
        }

        public void Commit()
        {
            transection.Commit();
        }
        public void RollBack()
        {
            transection.Rollback();
        }
       

       public void Dispose()
       {
           command.Dispose();
           transection.Dispose();
           connection.Dispose();
       }



       public void ExecuteQuery()
       {
           if (DMCommandType.Equals(DbCommandType.StoredProcedure))
           {
               CommandExecute();
           }

           else if (DMCommandType.Equals(DbCommandType.CommnadTest))
               command.CommandText = Queries;
          
               command.ExecuteNonQuery();
         
       }


       public bool IsTransectionOpen()
       {
           if (transection!=null)
               return true;
           return false;
       }

        public bool  IsConnectionOpen()
        {
           if (connection.State == ConnectionState.Open)
               return true;
           else
               return false;
        }
   

       public List<T> GetRecords<T>()
       {       
           IProduction produc = new InitializeProduction().Initalize<T>();
           Queries =(Queries!=null?Queries: produc.GetSingleLineQuery());
           CommandExecute();
           ExecuteReader();    
           return produc.produceList<T>(reader);
       }

       public DataSet GetDataSet()
       {
           CommandExecute();
           SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
           IProduction produc = new Production();
           return produc.produceDataSetRecords(dataAdapter);
       }
       public void StoredFiles<T>(List<T> file)
       {
           CommandExecute();
           IProduction produc = new InitializeProduction().Initalize<T>();
           String commands = produc.produceInsertQuery<T>(file);
           command.CommandType = CommandType.Text;
           command.CommandText = commands;
           command.Parameters.Add("@Data", SqlDbType.Binary).Value = produc.BytesArray<T>(file);

       }

       public List<T> RetriveFiles<T>()
       {
           CommandExecute();
           ExecuteReader();
           IProduction produc = new InitializeProduction().Initalize<T>();
           return produc.FilePath<T>(produc.produceList<T>(reader));
       }

       


       public void CommitTransection()
       {
           transection.Commit();
       }


       public byte[] RetriveFiles(string fileId)
       {
           throw new NotImplementedException();
       }


       public string SendEmailMessage(List<EMAIL> msgLst)
       {          
           foreach (EMAIL msg in msgLst)
           {
               StringBuilder sb = new StringBuilder();
               MailMessage mail = new MailMessage();
               mail.Subject = msg.MessageSubject;
               mail.From = new MailAddress(msg.SenderEmail,msg.Sender);
               mail.To.Add(msg.Email);

               if (msg.Message_Code != null && msg.Message_Code != "")
                   sb.Append("<br/> CODE :" + msg.Message_Code +"<br/>");

               sb.Append( msg.Message);
               mail.Body = sb.ToString();
               mail.IsBodyHtml = true;
               //SmtpClient smtp = new SmtpClient("smtp.aonbd.net");
              SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
               smtp.EnableSsl = true;
               NetworkCredential netCre = new NetworkCredential("noreplaybg@gmail.com", "maamaamaa");
               smtp.Credentials = netCre;

               try
               {
                   //Send message to email address
                   smtp.Send(mail);
                   mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

               }
               catch (Exception ex)
               {
               }
           }
           return "OK";
       }



       public String NextIncrementID<T>()
       {           
           IProduction produc = new InitializeProduction().Initalize<T>();
           Queries=produc.produceAutoIncrementIDQuery();
           CommandExecute();
           ExecuteReader();
           String id="";
           while(reader.Read())
           {
               id=reader[0].ToString();
           }
           return id;
       }

       public string SendEmailMessage(EMAIL msgEmail)
       {
            StringBuilder sb = new StringBuilder();
            MailMessage mail = new MailMessage();
            mail.Subject = msgEmail.MessageSubject;
            mail.From = new MailAddress(msgEmail.SenderEmail,msgEmail.Sender);
            mail.To.Add(msgEmail.Email);

            if (msgEmail.Message_Code != null && msgEmail.Message_Code != "")
                sb.Append("<br/> CODE :" + msgEmail.Message_Code +"<br/>");

            sb.Append( msgEmail.Message);
            mail.Body = sb.ToString();
            mail.IsBodyHtml = true;
           SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
           smtp.EnableSsl = true;
           NetworkCredential netCre = new NetworkCredential("noreplaybg@gmail.com", "maamaamaa");
           smtp.Credentials = netCre;

            try
            {
                //Send message to email address
                smtp.Send(mail);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

            }
            catch (Exception ex)
            {
            }
           
            return "OK";
       }

       private void CommandExecute() 
       {
           if (DMCommandType.Equals(DbCommandType.CommnadTest))
               command.CommandText = Queries;
           else if (DMCommandType.Equals(DbCommandType.StoredProcedure))
           {
               command.CommandType = CommandType.StoredProcedure;
               command.CommandText = CommandName;
               foreach (SqlParameter prms in paramLst)
               {
                   command.Parameters.Add(prms);
               }

           }
           
       }

       public void ExecuteReader()
       {
           reader = command.ExecuteReader();
           Queries = "";
           CommandName = "";
       }

        



       public bool ChecktableAlreadyExist<T>()
       {
           IDDLProduction produc = new InitializeDDLProduction().Initalize<T>();
           Queries += " " + produc.AvailibilityQuery();
           List<TABLES_INFO> tblLst = new List<TABLES_INFO>();
           CommandExecute();
           ExecuteReader();
           tblLst = produc.produceList<TABLES_INFO>(reader);
           reader.Close();
           if (tblLst.Count > 0)
              return true;
           return false;
       }

       public void CreateNewtable<T>()
       {
           IDDLProduction produc = new InitializeDDLProduction().Initalize<T>();
           Queries += " " + produc.CreateNewTable<T>();
           
       }

       public void AlterPreviousTable<T>()
       {
           IDDLProduction produc = new InitializeDDLProduction().Initalize<T>();
           Queries += " " + produc.AlterTable<T>();
       }


       public string SqlQueryCreateNewtable<T>()
       {
           IDDLProduction produc = new InitializeDDLProduction().Initalize<T>();
           return " " + produc.CreateNewTable<T>();
       }

       public string SqlQueryAlterPreviousTable<T>()
       {
           IDDLProduction produc = new InitializeDDLProduction().Initalize<T>();
           return " " + produc.AlterTable<T>();
       }


       public void ExecuteQuery(string Query)
       {
           if (DMCommandType.Equals(DbCommandType.StoredProcedure))
           {
               CommandExecute();
           }

           else if (DMCommandType.Equals(DbCommandType.CommnadTest))
               command.CommandText = Query;

           command.ExecuteNonQuery();
       }


      
    }
}