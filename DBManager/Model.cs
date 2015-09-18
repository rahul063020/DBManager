using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBManager
{
    public class Model
    {
    }
    public interface IFILES
    {
         String IDN { get; set; }
         string NAME { get; set; }
         string FILE_TYPE { get; set; }
         string FILE_PATH { get; set; }
         Byte[] FILE_DATA { get; set; }
    }

    [DataMember(ID_FIELD = "IDN", TABLE_NAME = "FILES", FILE_NAME_FIELD = "NAME", FILE_PATH_FIELD = "FILE_PATH")]
    public class FILES : IFILES {

        public string IDN { get; set; }

        public string NAME { get; set; }

        public string FILE_TYPE { get; set; }

        public string FILE_PATH { get; set; }

        public byte[] FILE_DATA { get; set; }
    }

    public class EMAIL
    {
        public String Message { get; set; }
        public String Message_Code { get; set; }
        public String MessageSubject { get; set; }
        public String Email { get; set; }
        public String ContactNo { get; set; }
        public String Sender { get; set; }
        public String SenderEmail { get; set; }
    }

    public class ERASE_FIELD
    {
        public String FIELD_NAME { get; set; }

    }

    public class SYNC_PROP
    {
        public string PRO_NAME { get; set; }
        public bool IS_SYNC { get; set; }
    }

    public class GENERI_CCLAUSE<T>
    {
         public T clause { get; set; }
         public  ClauseOperator clauseOprtr { get; set; }
    }

    public class TABLES_INFO
    {
        public String object_id { get; set; }
        public String name { get; set; }
        public String column_id { get; set; }
        public String system_type_id { get; set; }
        public String user_type_id { get; set; }
        public String max_length { get; set; }
        public String precision { get; set; }
        public String scale { get; set; }
        public String collation_name { get; set; }
        public String is_nullable { get; set; }
        public String is_ansi_padded { get; set; }
        public String is_rowguidcol { get; set; }
        public String is_identity { get; set; }
        public String is_computed { get; set; }
        public String is_filestream { get; set; }
        public String is_replicated { get; set; }
        public String is_non_sql_subscribed { get; set; }
        public String is_merge_published { get; set; }
        public String is_dts_replicated { get; set; }
        public String is_xml_document { get; set; }
        public String xml_collection_id { get; set; }
        public String default_object_id { get; set; }
        public String rule_object_id { get; set; }
        public String is_sparse { get; set; }
        public String is_column_set { get; set; }
        public String TYPE_NAME { get; set; }

    }
    
}