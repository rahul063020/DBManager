using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DBManager
{
    public class Production : IProduction
    {
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
                                pro.SetValue(objT,Convert.ToInt32(dataReader[pro.Name].ToString()), null);
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
        public String produceUpdateQuery<T>(List<T> records)
        {
            if (UPDATE_FIELD != null && !UPDATE_FIELD.Equals(""))
            {
                ID_FIELD = UPDATE_FIELD;              
            }
            String command = "";
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach (T itm in records)
            {
                String sql = "";
                String updateIno = "";
                String where = "";
                foreach (var pro in properties)
                {
                    if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).ToString().Equals(""))
                    {

                        if (pro.Name.Equals(ID_FIELD))
                        {
                            if (pro.PropertyType == typeof(System.Int32))
                            {
                                where += " WHERE [" + pro.Name + "]  = " + props[pro.Name].GetValue(itm) + "";
                            }
                            else if(pro.PropertyType == typeof(System.String))
                            {
                                where += " WHERE [" + pro.Name + "]  = '" + props[pro.Name].GetValue(itm) + "' ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                where += " WHERE [" + pro.Name + "]  = " + props[pro.Name].GetValue(itm) + "";
                            }
                           
                        }
                        else if (updateIno.Equals(""))
                        {
                            if (pro.PropertyType == typeof(System.DateTime))
                            {
                                DateTime dt = (DateTime)props[pro.Name].GetValue(itm);
                                if (dt.Year > 1900)
                                {
                                    updateIno += " [" + pro.Name + "] =" + " CONVERT(DATETIME,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                                }
                            }
                            else if (pro.PropertyType == typeof(System.Int32))
                            {
                                updateIno += " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                updateIno += " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                updateIno += " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                updateIno += " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(System.Decimal) || pro.PropertyType == typeof(Nullable<System.Decimal>))
                            {
                                updateIno += " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                updateIno += " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(System.Boolean))
                            {
                                updateIno += " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                            {
                                updateIno += " [" + pro.Name + "] =" + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else if (pro.PropertyType == typeof(System.Guid))
                            {
                                updateIno += " [" + pro.Name + "] =" + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else
                            {
                                updateIno += " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'";
                            }
                        }
                        else
                        {
                            if (pro.PropertyType == typeof(System.DateTime))
                            {
                                DateTime dt = (DateTime)props[pro.Name].GetValue(itm);
                                if (dt.Year > 1900)
                                {
                                    updateIno += " , " + " [" + pro.Name + "] =" + " CONVERT(DATETIME,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                                }
                            }
                            else if (pro.PropertyType == typeof(System.Int32))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(System.Boolean))
                            {
                                updateIno += "," + " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm).ToString() + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm) + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                updateIno += "," + " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + props[pro.Name].GetValue(itm).ToString() + " ";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else if (pro.PropertyType == typeof(System.Guid))
                            {
                                updateIno += "," + " [" + pro.Name + "] =" + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else
                            {
                                updateIno += "," + " [" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'";
                            }
                        }
                       
                    }
                }


                sql += " UPDATE [" + TABLE_NAME + "] SET " + updateIno + where;

                command += "" + sql;
            }

            return command;
        }
        public String produceDeleteQuery<T>(List<T> records)
        {
            String command = "";
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach (T itm in records)
            {
                foreach (var pro in properties)
                {
                    if (ERASE_FIELD == null || ERASE_FIELD.Count == 0)
                    {
                        if (pro.Name.Equals(ID_FIELD))
                        {
                            if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals(""))
                            {
                                String sql = "";
                                if (!command.Contains(props[pro.Name].GetValue(itm).ToString()))
                                {
                                    if (pro.PropertyType == typeof(System.Int32))
                                    {
                                        sql = " Delete From [" + TABLE_NAME + "] where  [" + ID_FIELD + "] = " + props[pro.Name].GetValue(itm) + " ";
                                    }
                                    else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                                    {
                                        sql = " Delete From [" + TABLE_NAME + "] where  [" + ID_FIELD + "] = " + props[pro.Name].GetValue(itm) + " ";
                                    }
                                    else if (pro.PropertyType == typeof(System.String))
                                    {
                                        sql = " Delete From [" + TABLE_NAME + "] where  [" + ID_FIELD + "] = '" + props[pro.Name].GetValue(itm) + "'  ";
                                    }
                                  
                                    command += "" + sql;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(ERASE_FIELD.FindAll(itms=>itms.FIELD_NAME.Equals(pro.Name)).Count>0)
                        {
                            if (pro.Name.Equals(pro.Name))
                            {
                                if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals(""))
                                {
                                    String sql = "";
                                    if (!command.Contains(props[pro.Name].GetValue(itm).ToString()))
                                    {
                                        if (command.Equals(""))
                                        {
                                            if (pro.PropertyType == typeof(System.Int32))
                                            {
                                                sql = " Delete From [" + TABLE_NAME + "] where  [" + pro.Name + "]  = " + props[pro.Name].GetValue(itm) + " ";
                                            }
                                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                                            {
                                                sql = " Delete From [" + TABLE_NAME + "] where  [" + ID_FIELD + "] = " + props[pro.Name].GetValue(itm) + " ";
                                            }
                                            else if (pro.PropertyType == typeof(System.String))
                                            {
                                                sql = " Delete From [" + TABLE_NAME + "] where  [" + pro.Name + "]  = '" + props[pro.Name].GetValue(itm) + "' ";
                                            }
                                        }
                                        else
                                        {
                                            if (pro.PropertyType == typeof(System.Int32))
                                            {
                                                sql += " OR  " + pro.Name + " = " + props[pro.Name].GetValue(itm) + " ";
                                            }
                                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                                            {
                                                sql += " OR  " + pro.Name + " = " + props[pro.Name].GetValue(itm) + " ";
                                            }
                                            else if(pro.PropertyType == typeof(System.String))
                                            {
                                                sql += " OR  " + pro.Name + " = '" + props[pro.Name].GetValue(itm) + "' ";
                                            }
                                            
                                        }
                                        command += "" + sql;
                                    }
                                }
                            }
                        }
                    }

                }
            }

           

            return command;
        }
        public DataSet produceDataSetRecords(SqlDataAdapter dataAdapter)
        {
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            return ds;
        
        }
        public string produceInsertQuery<T>(List<T> records)
        {
            String command = "";
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach (T itm in records)
            {
                String valueName = "";
                String value = "";
                foreach (var pro in properties)
                {
                    if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals(""))
                    {
                        if (pro.Name.Equals(ID_FIELD))
                            continue;

                        else if (valueName.Equals(""))
                        {
                            if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(Nullable<System.DateTime>))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " " + " CONVERT(DATETIME,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " " + props[pro.Name].GetValue(itm).ToString() + " ";
                            }
                            else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " '" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " '" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else if (pro.PropertyType == typeof(System.Byte[]))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += "@Data";
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>) || pro.PropertyType == typeof(System.Guid))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " " + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                            }
                            else if (pro.PropertyType == typeof(System.Decimal) || pro.PropertyType == typeof(Nullable<System.Decimal>))
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += " '" + props[pro.Name].GetValue(itm).ToString() + "' ";
                            }
                            else
                            {
                                valueName += " [" + pro.Name + "] ";
                                value += "  '" + props[pro.Name].GetValue(itm) + "'";
                            }
                        }
                        else if (pro.PropertyType == typeof(System.DateTime) || pro.PropertyType == typeof(Nullable<System.DateTime>))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ," + " CONVERT(DATETIME,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                        }
                        else if (pro.PropertyType == typeof(System.Int32) || pro.PropertyType == typeof(Nullable<System.Int32>))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ," + props[pro.Name].GetValue(itm).ToString() + " ";
                        }
                        else if (pro.PropertyType == typeof(System.Boolean) || pro.PropertyType == typeof(Nullable<System.Boolean>))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ,'" + props[pro.Name].GetValue(itm).ToString() + "' ";
                        }
                        else if (pro.PropertyType == typeof(System.Double) || pro.PropertyType == typeof(Nullable<System.Double>))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ," + props[pro.Name].GetValue(itm).ToString() + " ";
                        }
                        else if (pro.PropertyType == typeof(System.Byte[]))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += ",@Data";
                        }
                        else if (pro.PropertyType == typeof(Nullable<System.Guid>) || pro.PropertyType == typeof(System.Guid))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ," + " CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')";
                        }
                        else if (pro.PropertyType == typeof(System.Decimal) || pro.PropertyType == typeof(Nullable<System.Decimal>))
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += " ," + props[pro.Name].GetValue(itm).ToString() + " ";
                        }
                        else
                        {
                            valueName += " ,[" + pro.Name + "] ";
                            value += "  ,'" + props[pro.Name].GetValue(itm) + "'";
                        }
                    }

                }

                String sql = " INSERT INTO  [" + TABLE_NAME + "]  ( " + valueName + " ) Values( " + value + " )";

                command += "" + sql;
            }
            return command;
        }


        public string TABLE_NAME { get; set; }


        public string ID_FIELD { get; set; }

        public byte[] BytesArray<T>(List<T> file)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            Byte[] fileArray = null;
            foreach (T itm in file)
            {
                foreach (var pro in properties)
                {
                    if (pro.PropertyType.Equals(typeof(System.Byte[])))
                    {
                        fileArray = (Byte[])props[pro.Name].GetValue(itm);
                        //BinaryFormatter bf = new BinaryFormatter();
                        //MemoryStream ms = new MemoryStream();
                        //bf.Serialize(ms, props[pro.Name].GetValue(file));
                        //fileArray= ms.ToArray();
                    }
                }
            }
            return fileArray;
        }

        public List<T> FilePath<T>(List<T> file)
        {            
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            String pathString = "";
            foreach (T itm in file)
            {
                foreach (var pro in properties)
                {
                    if (pro.PropertyType.Equals(typeof(System.Byte[])))
                    {
                        string directoryname = "~/TempFile";
                        bool isExists = Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(directoryname));

                        if (!isExists)
                            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(directoryname));

                        String mapath = System.Web.HttpContext.Current.Server.MapPath(directoryname);
                        pathString = System.IO.Path.Combine(mapath, props[FILE_NAME_FIELD].GetValue(itm).ToString());
                        //System.IO.File.WriteAllBytes(pathString, (Byte[])props[pro.Name].GetValue(itm));
                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(pathString));
                        Writer.Write((Byte[])props[pro.Name].GetValue(itm));
                        Writer.Flush();
                        Writer.Close();
                        string ServerPathFile = directoryname + "/" + props[FILE_NAME_FIELD].GetValue(itm);
                        props[FILE_PATH_FIELD].SetValue(itm, ServerPathFile);
                    }
                }
            }
            return file;
        }

        public string UPDATE_FIELD { get; set; }
        
        public string GetSingleLineQuery()
        {
            return "SELECT * FROM [" + TABLE_NAME+"]";
        }

        public string produceAutoIncrementIDQuery()
        {
            return "Select IDENT_CURRENT('[" + TABLE_NAME + "]') +1 ";
        }

        public string FILE_NAME_FIELD { get; set; }

        public string FILE_PATH_FIELD { get; set; }

        public List<ERASE_FIELD> ERASE_FIELD { get; set; }


        public List<U> ConvertList<T, U>(List<T> provideLst)
        {
            List<U> lst = new List<U>();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            var properties = typeof(U).GetProperties();
            foreach (T itm in provideLst)
            {
                var objT = Activator.CreateInstance<U>();
                foreach (var pro in properties)
                {
                    try
                    {
                        if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals(""))
                        {
                            if (pro.PropertyType == typeof(System.DateTime))
                            {
                                pro.SetValue(objT, props[pro.Name].GetValue(itm), null);
                            }
                            else if (pro.PropertyType == typeof(System.Byte[]))
                            {
                                pro.SetValue(objT, (Byte[])props[pro.Name].GetValue(itm), null);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                pro.SetValue(objT, Convert.ToInt32(props[pro.Name].GetValue(itm).ToString()), null);
                            }
                            else if (pro.PropertyType == typeof(System.Int32))
                            {
                                pro.SetValue(objT, Convert.ToInt32(props[pro.Name].GetValue(itm).ToString()), null);
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                pro.SetValue(objT, Convert.ToDouble(props[pro.Name].GetValue(itm).ToString()), null);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                pro.SetValue(objT, Convert.ToDouble(props[pro.Name].GetValue(itm).ToString()), null);
                            }
                            else if (pro.PropertyType == typeof(System.Boolean))
                            {
                                pro.SetValue(objT, (Convert.ToBoolean(props[pro.Name].GetValue(itm))), null);
                            }
                            else if (pro.PropertyType == typeof(System.Decimal))
                            {
                                pro.SetValue(objT, (Convert.ToDecimal(props[pro.Name].GetValue(itm))), null);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Decimal>))
                            {
                                pro.SetValue(objT, (Convert.ToDecimal(props[pro.Name].GetValue(itm))), null);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                pro.SetValue(objT, Convert.ToInt32(props[pro.Name].GetValue(itm)), null);
                            }
                            else if (pro.PropertyType == typeof(System.Guid))
                            {
                                pro.SetValue(objT, (new Guid(props[pro.Name].GetValue(itm).ToString())), null);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                            {
                                pro.SetValue(objT, (new Guid(props[pro.Name].GetValue(itm).ToString())), null);
                            }
                            else
                            {
                                pro.SetValue(objT, props[pro.Name].GetValue(itm).ToString(), null);
                            }

                        }
                        else
                            pro.SetValue(objT, "", null);



                    }
                    catch (Exception ex)
                    {

                    }

                }
                lst.Add(objT);
            }

            return lst;
        }



        public string CreateQuery<T>(List<GENERI_CCLAUSE<T>> clause)
        {
            StringBuilder sb = new StringBuilder();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            foreach(GENERI_CCLAUSE<T>  mdle in clause)
            {
                String oprator = "";
                switch(mdle.clauseOprtr)
                {
                    case ClauseOperator.NONE:
                        oprator = "";
                        break;
                    case ClauseOperator.AND:
                        oprator = " and ";
                        break;
                    case ClauseOperator.OR:
                        oprator = " or ";
                        break;
                }
                T itm = mdle.clause;
               
                sb.Append(" SELECT * FROM ["+ TABLE_NAME+"] ");
                foreach (var pro in properties)
                {
                    try
                    {
                        if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals(""))
                        {
                            if (pro.PropertyType == typeof(System.DateTime))
                            {
                                DateTime dt = (DateTime)props[pro.Name].GetValue(itm);
                                if(!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + dt);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Int32))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Boolean))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm)+"'");
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'");
                            }
                            else if (pro.PropertyType == typeof(System.Decimal))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Decimal>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            
                            else if (pro.PropertyType == typeof(System.Guid))
                            {
                            if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                            sb.Append("[" + pro.Name + "] =CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')");
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')");
                            }
                            else
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'");
                            }

                        }
                        else
                        {

                        }



                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            return sb.ToString();
        }



        public string ExistSelectQuery<T>(List<T> Result)
        {
            StringBuilder sb = new StringBuilder();
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            var properties = typeof(T).GetProperties();
            sb.Append(" SELECT * FROM ["+ TABLE_NAME+"] ");
            foreach (T itm in Result)
            {
                foreach (var pro in properties)
                {
                    try
                    {
                        if (props[pro.Name].GetValue(itm) != null && !props[pro.Name].GetValue(itm).Equals("")  && pro.Name==ID_FIELD)
                        {
                            if (pro.PropertyType == typeof(System.DateTime))
                            {
                                DateTime dt = (DateTime)props[pro.Name].GetValue(itm);
                                if(!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + dt);
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Int32>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Int32))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Double>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(System.Boolean))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm)+"'");
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Boolean>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'");
                            }
                            else if (pro.PropertyType == typeof(System.Decimal))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Decimal>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =" + props[pro.Name].GetValue(itm));
                            }
                            
                            else if (pro.PropertyType == typeof(System.Guid))
                            {
                            if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                            sb.Append("[" + pro.Name + "] =CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')");
                            }
                            else if (pro.PropertyType == typeof(Nullable<System.Guid>))
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] =CONVERT(uniqueidentifier,'" + props[pro.Name].GetValue(itm).ToString() + "')");
                            }
                            else
                            {
                                if (!sb.ToString().Contains("WHERE"))
                                {
                                    sb.Append(" WHERE ");
                                }
                                else
                                {
                                    sb.Append(" and ");
                                }
                                sb.Append("[" + pro.Name + "] ='" + props[pro.Name].GetValue(itm) + "'");
                            }

                        }
                        else
                        {

                        }



                    }
                    catch (Exception ex)
                    {

                    }

                }
                
            }

            return sb.ToString();
        }

        public String produceDataSetInsertQuery(DataSet ds)
        {
            String sb = "";
          

            foreach(DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder value = new StringBuilder();
                    StringBuilder valueName = new StringBuilder();
                    int i = 1;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (!dc.Unique && dc.AllowDBNull && !dc.AutoIncrement)
                        {
                            if (dr[dc] != null && dr[dc].ToString() != "")
                            {
                                if (i == 1)
                                {
                                    valueName.Append(" [" + dc.ColumnName + "] ");
                                    if (dc.DataType == typeof(System.Int32))
                                    {
                                        value.Append(" " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Decimal))
                                    {
                                        value.Append(" " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Double))
                                    {
                                        value.Append(" " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Boolean))
                                    {
                                        value.Append(" '" + dr[dc].ToString() + "' ");
                                    }
                                    else if (dc.DataType == typeof(System.Guid))
                                    {
                                        value.Append("CONVERT(uniqueidentifier,'" + dr[dc].ToString() + "')");
                                    }
                                    else if (dc.DataType == typeof(System.DateTime))
                                    {
                                        value.Append("CONVERT(DATETIME,'" + dr[dc].ToString() + "')");
                                    }
                                    else
                                    {
                                        value.Append("'" + dr[dc].ToString() + "'");
                                    }

                                }
                                else
                                {
                                    valueName.Append(" ,[" + dc.ColumnName + "] ");
                                    if (dc.DataType == typeof(System.Int32))
                                    {
                                        value.Append(" ," + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Decimal))
                                    {
                                        value.Append(" ," + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Double))
                                    {
                                        value.Append(" ," + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Boolean))
                                    {
                                        value.Append(" ,'" + dr[dc].ToString() + "' ");
                                    }
                                    else if (dc.DataType == typeof(System.Guid))
                                    {
                                        value.Append(",CONVERT(uniqueidentifier,'" + dr[dc].ToString() + "')");
                                    }
                                    else if (dc.DataType == typeof(System.DateTime))
                                    {
                                        value.Append(",CONVERT(DATETIME,'" + dr[dc].ToString() + "')");
                                    }
                                    else
                                    {
                                        value.Append(",'" + dr[dc].ToString() + "'");
                                    }

                                }
                                i++;
                            }
                        }
                    }
                    sb += " INSERT INTO [" + dt.TableName + "] (" + valueName.ToString() + ") VALUES (" + value.ToString() + ")" + Environment.NewLine; 
                }
                
            }

            return sb;
            
        }

        public String produceDataSetUpdateQuery(DataSet ds)
        {
            String sb = "";


            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder value = new StringBuilder();
                    StringBuilder condition = new StringBuilder();
                    int i = 1;
                    foreach (DataColumn dc in dt.Columns)
                    {

                        if (!dc.Unique && dc.AllowDBNull && !dc.AutoIncrement)
                        {
                            if (dr[dc] != null && dr[dc].ToString() != "")
                            {
                                if (i == 1)
                                {
                                    if (dc.DataType == typeof(System.Int32))
                                    {
                                        value.Append("["+dc.ColumnName+"]= " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Decimal))
                                    {
                                        value.Append("[" + dc.ColumnName + "]= " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Double))
                                    {
                                        value.Append("[" + dc.ColumnName + "]= " + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Boolean))
                                    {
                                        value.Append("[" + dc.ColumnName + "]= '" + dr[dc].ToString() + "' ");
                                    }
                                    else if (dc.DataType == typeof(System.Guid))
                                    {
                                        value.Append("[" + dc.ColumnName + "]= CONVERT(uniqueidentifier,'" + dr[dc].ToString() + "')");
                                    }
                                    else if (dc.DataType == typeof(System.DateTime))
                                    {
                                        value.Append("[" + dc.ColumnName + "]= CONVERT(DATETIME,'" + dr[dc].ToString() + "')");
                                    }
                                    else
                                    {
                                        value.Append("[" + dc.ColumnName + "]='" + dr[dc].ToString() + "'");
                                    }

                                }
                                else
                                {
                                    if (dc.DataType == typeof(System.Int32))
                                    {
                                        value.Append(" ,[" + dc.ColumnName + "]=" + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Decimal))
                                    {
                                        value.Append(" ,[" + dc.ColumnName + "]=" + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Double))
                                    {
                                        value.Append(" ,[" + dc.ColumnName + "]=" + dr[dc].ToString() + " ");
                                    }
                                    else if (dc.DataType == typeof(System.Boolean))
                                    {
                                        value.Append(" ,[" + dc.ColumnName + "]='" + dr[dc].ToString() + "' ");
                                    }
                                    else if (dc.DataType == typeof(System.Guid))
                                    {
                                        value.Append(",[" + dc.ColumnName + "]= CONVERT(uniqueidentifier,'" + dr[dc].ToString() + "')");
                                    }
                                    else if (dc.DataType == typeof(System.DateTime))
                                    {
                                        value.Append(",[" + dc.ColumnName + "]= CONVERT(DATETIME,'" + dr[dc].ToString() + "')");
                                    }
                                    else
                                    {
                                        value.Append(",[" + dc.ColumnName + "]= '" + dr[dc].ToString() + "'");
                                    }

                                }
                                i++;
                            }
                        }
                        else
                        {
                            condition.Append(" WHERE [" + dc.ColumnName + "]='" + dr[dc] + "'");
                        }
                    }
                    sb += " UPDATE [" + dt.TableName + "] SET "+ value.ToString() +" "+condition.ToString()+" "+ Environment.NewLine;
                }

            }

            return sb;

        }

        public string ExistSelectQuery(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
           
            sb.Append(" SELECT * FROM [" + TABLE_NAME + "] ");
            
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (!dc.Unique && dc.AllowDBNull && !dc.AutoIncrement)
                    {
                        sb.Append(" WHERE [" + dc.ColumnName + "]=" +dr[dc]);
                    }
                }   

            }

            

            return sb.ToString();
        }

        public string produceDataSetEraseQuery(DataSet ds)
        {
            StringBuilder sb = new StringBuilder();

            foreach(DataTable dt in ds.Tables)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    foreach(DataColumn dc in dt.Columns)
                    {
                        if(dc.AutoIncrement && !dc.AllowDBNull && dc.Unique && dr[dc]!=null && dr[dc].ToString()!="")
                        {
                            sb.Append("DELETE FROM [" + dt.TableName + "] WHERE [" + dc.ColumnName + "]='" + dr[dc] + "'");
                        }
                    }
                }
            }
            return sb.ToString();
        }


     
    }
}