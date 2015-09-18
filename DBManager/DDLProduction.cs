using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
   public class DDLProduction : IDDLProduction
    {
       public string ID_FIELD { get; set; }

       public string TABLE_NAME { get; set; }

       public string FILE_NAME_FIELD { get; set; }

       public string FILE_PATH_FIELD { get; set; }

       public string UPDATE_FIELD { get; set; }

       public List<ERASE_FIELD> ERASE_FIELD { get; set; }

       public bool SYNC_TABLE { get; set; }

       public string CreateNewTable<T>()
       {
           //String tableexist = "select object_id from sys.tables where name='"+TABLE_NAME+"'";
           //DataSet ds = new DBMContext().RetriveDataSet(tableexist);
           //if(ds.Tables.Count==0)
           //{ }
           if (SYNC_TABLE)
           {
               StringBuilder sb = new StringBuilder();
               sb.Append(" CREATE TABLE [" + TABLE_NAME + "]" + Environment.NewLine);
               sb.Append("(" + Environment.NewLine);
               sb.Append("[" + ID_FIELD + "] int IDENTITY(1,1) NOT NULL," + Environment.NewLine);
               PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
               var properties = typeof(T).GetProperties();

               foreach (var pro in properties)
               {
                   if (ISPropertySynced(pro.Name))
                   {
                       if (!pro.Name.Equals(ID_FIELD))
                       {
                           if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(Nullable<System.DateTime>))
                           {
                               sb.Append("[" + pro.Name + "] [datetime] NULL," + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(Nullable<System.Int32>))
                           {
                               sb.Append("[" + pro.Name + "] [int] NULL," + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(Nullable<System.Double>))
                           {
                               sb.Append("[" + pro.Name + "] [decimal](18, 2) NULL," + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(Nullable<System.Boolean>))
                           {
                               sb.Append("[" + pro.Name + "] [bit] NULL," + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Guid) || pro.PropertyType == typeof(Nullable<System.Guid>))
                           {
                               sb.Append("[" + pro.Name + "] [uniqueidentifier] NULL," + Environment.NewLine);
                           }
                           else
                           {
                               sb.Append("[" + pro.Name + "] [varchar](500) NULL," + Environment.NewLine);
                           }
                       }
                   }
               }

               sb.Append("PRIMARY KEY (" + ID_FIELD + ")" + Environment.NewLine);
               sb.Append(")" + Environment.NewLine);
               return sb.ToString();
           }
           else
           {
               return "";
           }
       }


       public string AlterTable<T>()
       {
           if (SYNC_TABLE)
           {
               StringBuilder sb = new StringBuilder();

               String columnOfTheTable = @"Select l.*,t.name TYPE_NAME from 
                             ( 
                             select * from sys.all_columns where object_id=
                            (select object_id from sys.tables where name='" + TABLE_NAME + @"') )l 
                            Left outer join 
                           (select * from sys.types) t
                      on l.user_type_id=t.user_type_id ";
               DBMContext dbContxt = new DBMContext();
               List<TABLES_INFO> tblColumnlst = dbContxt.RetriveRecords<TABLES_INFO>(columnOfTheTable);

               PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
               var properties = typeof(T).GetProperties();

               foreach (var pro in properties)
               {
                   if (ISPropertySynced(pro.Name))
                   {
                       if (tblColumnlst.FindAll(itms => itms.name == pro.Name).Count == 0)
                       {
                           if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(Nullable<System.DateTime>))
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [datetime] NULL " + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(Nullable<System.Int32>))
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [int] NULL " + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(Nullable<System.Double>))
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [decimal](18, 2) NULL " + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(Nullable<System.Boolean>))
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [bit] NULL " + Environment.NewLine);
                           }
                           else if (pro.PropertyType == typeof(System.Guid) || pro.PropertyType == typeof(Nullable<System.Guid>))
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [uniqueidentifier] NULL " + Environment.NewLine);
                           }
                           else
                           {
                               sb.Append(" ALTER TABLE [" + TABLE_NAME + "] ADD " + Environment.NewLine);
                               sb.Append(" [" + pro.Name + "] [varchar](500) NULL " + Environment.NewLine);
                           }
                       }
                       else
                       {
                           if (!pro.Name.Equals(ID_FIELD))
                           {
                               if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(Nullable<System.DateTime>))
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "] " + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [datetime] NULL " + Environment.NewLine);
                               }
                               else if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(Nullable<System.Int32>))
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "]" + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [int] NULL " + Environment.NewLine);
                               }
                               else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(Nullable<System.Double>))
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "] " + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [decimal](18, 2) NULL " + Environment.NewLine);
                               }
                               else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(Nullable<System.Boolean>))
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "] " + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [bit] NULL " + Environment.NewLine);
                               }
                               else if (pro.PropertyType == typeof(System.Guid) || pro.PropertyType == typeof(Nullable<System.Guid>))
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "] " + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [uniqueidentifier] NULL " + Environment.NewLine);
                               }
                               else
                               {
                                   sb.Append(" ALTER TABLE [" + TABLE_NAME + "] " + Environment.NewLine + " ALTER COLUMN " + Environment.NewLine);
                                   sb.Append(" [" + pro.Name + "] [varchar](500) NULL " + Environment.NewLine);
                               }
                           }
                       }
                   }

               }
               return sb.ToString();
           }
           else
           {
               return "";
           }
	
       }


       public string AvailibilityQuery()
       {
           return "select object_id from sys.tables where name='" + TABLE_NAME + "'";
       }

       public List<T> produceList<T>(IDataReader dataReader)
       {
           PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

           var properties = typeof(T).GetProperties();

           List<T> lst = new List<T>();
           while (dataReader.Read())
           {

               var objT = Activator.CreateInstance<T>();
               foreach (var pro in properties)
               {
                   try
                   {
                       if (dataReader[pro.Name] != null && !dataReader[pro.Name].ToString().Equals(""))
                       {
                           if (pro.PropertyType == typeof(System.DateTime))
                           {
                               pro.SetValue(objT, dataReader[pro.Name], null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.DateTime>))
                           {
                               pro.SetValue(objT, dataReader[pro.Name], null);
                           }
                           else if (pro.PropertyType == typeof(System.Byte[]))
                           {
                               pro.SetValue(objT, (Byte[])dataReader[pro.Name], null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                           {
                               pro.SetValue(objT, Convert.ToInt32(dataReader[pro.Name].ToString()), null);
                           }
                           else if (pro.PropertyType == typeof(System.Int32))
                           {
                               pro.SetValue(objT, Convert.ToInt32(dataReader[pro.Name].ToString()), null);
                           }
                           else if (pro.PropertyType == typeof(System.Double))
                           {
                               pro.SetValue(objT, Convert.ToDouble(dataReader[pro.Name].ToString()), null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.Double>))
                           {
                               pro.SetValue(objT, Convert.ToDouble(dataReader[pro.Name].ToString()), null);
                           }
                           else if (pro.PropertyType == typeof(System.Boolean))
                           {
                               pro.SetValue(objT, (Convert.ToBoolean(dataReader[pro.Name])), null);
                           }
                           else if (pro.PropertyType == typeof(System.Decimal))
                           {
                               pro.SetValue(objT, (Convert.ToDecimal(dataReader[pro.Name])), null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.Decimal>))
                           {
                               pro.SetValue(objT, (Convert.ToDecimal(dataReader[pro.Name])), null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                           {
                               pro.SetValue(objT, Convert.ToBoolean(dataReader[pro.Name]), null);
                           }
                           else if (pro.PropertyType == typeof(System.Guid))
                           {
                               pro.SetValue(objT, (new Guid(dataReader[pro.Name].ToString())), null);
                           }
                           else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                           {
                               pro.SetValue(objT, (new Guid(dataReader[pro.Name].ToString())), null);
                           }
                           else
                           {
                               pro.SetValue(objT, dataReader[pro.Name].ToString(), null);
                           }
                       }
                       else
                           pro.SetValue(objT, "", null);
                   }
                   catch (Exception ex)
                   {
                       //pro.SetValue(objT, "", null);
                   }
               }

               lst.Add(objT);

           }
           return lst;
       }

       public List<SYNC_PROP> LIST_SYNC_PRO { get; set; }

       private bool ISPropertySynced(String proName)
       {
           if(LIST_SYNC_PRO.Count>0)
           {
               if (LIST_SYNC_PRO.FindAll(itm => itm.PRO_NAME == proName).Count > 0)
               {
                   if(LIST_SYNC_PRO.FindAll(itm => itm.PRO_NAME == proName && itm.IS_SYNC == true).Count>0)
                       return true;
                   else
                       return false;
               }
               else
               {
                   return true;
               }
           }
           else
           {
               return true;
           }
       }
    }
}
