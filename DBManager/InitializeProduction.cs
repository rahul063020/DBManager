using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DBManager
{
    public class InitializeProduction
    {
        public IProduction Initalize<T>() {
            IProduction prduc;
            String id = "";
            String TableNm = "";
            String FileNameField = "";
            String FilePathField = "";
            String UpdateField="";
            List<ERASE_FIELD> Erase_Field = new List<ERASE_FIELD>();
            Type type = typeof(T);
            DataMember dM;
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                dM = attr as DataMember;
                if (null != dM)
                {
                    id = dM.ID_FIELD;
                    TableNm = dM.TABLE_NAME;
                    FileNameField=(dM.FILE_NAME_FIELD!=null?dM.FILE_NAME_FIELD:"");
                    FilePathField = (dM.FILE_PATH_FIELD != null ? dM.FILE_PATH_FIELD : "");
                    if(dM.ERASE_FIELD!=null)
                    Erase_Field.Add(new ERASE_FIELD { FIELD_NAME = dM.ERASE_FIELD });
                    UpdateField = dM.UPDATE_FIELD;
                }
            }
            //Querying Class-Field (only public) Attributes
            foreach (FieldInfo field in type.GetFields())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    dM = attr as DataMember;
                    if (null != dM)
                    {
                        Erase_Field.Add(new ERASE_FIELD { FIELD_NAME = dM.ERASE_FIELD });
                    }
                }
            }


            prduc = new Production();
            prduc.ID_FIELD = id;
            prduc.TABLE_NAME = TableNm;
            if(prduc.ERASE_FIELD!=null)
            prduc.ERASE_FIELD = new List<ERASE_FIELD>();
            prduc.ERASE_FIELD = Erase_Field;
            prduc.UPDATE_FIELD = UpdateField;            
            return prduc;
        }


    }


    public class InitializeDDLProduction{
        public IDDLProduction Initalize<T>()
        {
            IDDLProduction prduc;
            String id = "";
            String TableNm = "";
            String FileNameField = "";
            String FilePathField = "";
            String UpdateField = "";
            bool Sync_Table = true;
            bool Sync_Pro = true;
            List<ERASE_FIELD> Erase_Field = new List<ERASE_FIELD>();
            List<SYNC_PROP> lstSynPro = new List<SYNC_PROP>();
            Type type = typeof(T);
            DataMember dM;
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                dM = attr as DataMember;
                if (null != dM)
                {
                    id = dM.ID_FIELD;
                    TableNm = dM.TABLE_NAME;
                    FileNameField = (dM.FILE_NAME_FIELD != null ? dM.FILE_NAME_FIELD : "");
                    FilePathField = (dM.FILE_PATH_FIELD != null ? dM.FILE_PATH_FIELD : "");
                    if (dM.ERASE_FIELD != null)
                        Erase_Field.Add(new ERASE_FIELD { FIELD_NAME = dM.ERASE_FIELD });
                    UpdateField = dM.UPDATE_FIELD;
                    Sync_Table = dM.SYNC_TABLE;
                }
            }
            //Querying Class-Field (only public) Attributes
            foreach (FieldInfo field in type.GetFields())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    dM = attr as DataMember;
                    if (null != dM)
                    {
                        Erase_Field.Add(new ERASE_FIELD { FIELD_NAME = dM.ERASE_FIELD });
                    }
                }
            }


            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    dM = attr as DataMember;
                    if (null != dM)
                    {
                        lstSynPro.Add(new SYNC_PROP { IS_SYNC = dM.SYNC_PROERTY, PRO_NAME = field.Name });
                    }
                }
            }

            prduc = new DDLProduction();
            prduc.ID_FIELD = id;
            prduc.TABLE_NAME = TableNm;
            if (prduc.ERASE_FIELD != null)
                prduc.ERASE_FIELD = new List<ERASE_FIELD>();
            prduc.ERASE_FIELD = Erase_Field;
            prduc.UPDATE_FIELD = UpdateField;
            prduc.SYNC_TABLE = Sync_Table;
            prduc.LIST_SYNC_PRO = lstSynPro;
            return prduc;
        }
    }
}