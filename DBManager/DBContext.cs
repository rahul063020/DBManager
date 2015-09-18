using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBContext<T> 
    {

        public String Message { get; set; }
        public  void Where(String lineQuery)
        {
            Query = lineQuery;
        }

        private String Query { get; set; }

        private List<GENERI_CCLAUSE<T>> _lstrecord;
        private List<GENERI_CCLAUSE<T>> lstrecord
        {
            get
            {
                return _lstrecord;
            }
            set
            {
                _lstrecord = value;
            }
        }

        public DBContext()
        {
            Select = Activator.CreateInstance<T>();
        }

        private List<T> _Result;
        public List<T> Result
        {
            get
            {                
                return FindResult();
            }
            set
            {
                _Result = value;
            }
        }

        private T _Select;
        public  T Select
        {
            get
            {
                return _Select;
            }
            set
            {
                _Select = value;
            }
        }


        private List<T> FindResult()
        {
            Setproperty();
            return new List<T>();
        }
        public void UpdateRecords()
        {

            var context = new DBMContext();
            using(context)
            {
                context.UpdateRecords(Result);
            }
            Message = context.Message;
        }
        private void Setproperty()
        {
            IProduction pro = new InitializeProduction().Initalize<T>();
            if (Query != null)
            {
                GENERI_CCLAUSE<T> mdl = new GENERI_CCLAUSE<T>();
                mdl.clause = Select;
                mdl.clauseOprtr = ClauseOperator.NONE;
                lstrecord = new List<GENERI_CCLAUSE<T>>();
                lstrecord.Add(mdl);                
                String query = pro.CreateQuery<T>(lstrecord);
            }
            else
            {
                Query += "SELECT * FROM [" + pro.TABLE_NAME + "] WHERE " + Query;
            }
        }
        public void AddRecords()
        {
            var context = new DBMContext();
            using (context)
            {
                context.SaveRecords(Result);
            }
            Message = context.Message;
        }
        

    }
}
