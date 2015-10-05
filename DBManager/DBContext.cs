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


        private bool SetVal = false;
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
            SyncDbEntity();
            Select = Activator.CreateInstance<T>();
        }

        private List<T> _Result;
        public List<T> Result
        {
            get
            {
                if (SetVal)
                {
                    return _Result;
                }
                else
                {
                    return FindResult();
                }
            }
            set
            {
                _Result = value;
                SetVal = true;
            }
        }


        private int _NextInrementID;
        public int NextIncrementID
        {
            get
            {
                return NextAutoID();
            }
            set
            {
                _NextInrementID = value;
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
            SetVal = true;
            Setproperty();
            var contxt = new DBMContext();
            using(contxt)
            {
            _Result = contxt.RetriveRecords<T>(Query);
             return _Result;
            }
        }
        private void UpdateRecords()
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
            if (Query == null || Query!="")
            {
                GENERI_CCLAUSE<T> mdl = new GENERI_CCLAUSE<T>();
                mdl.clause = Select;
                mdl.clauseOprtr = ClauseOperator.NONE;
                lstrecord = new List<GENERI_CCLAUSE<T>>();
                lstrecord.Add(mdl);
                Query = pro.CreateQuery<T>(lstrecord);
            }
            else 
            {
                Query += "SELECT * FROM [" + pro.TABLE_NAME + "] WHERE " + Query;
            }
        }
        private void AddRecords()
        {
            var context = new DBMContext();
            using (context)
            {
                context.SaveRecords(Result);
            }
            Message = context.Message;
        }
        
        public void SaveChanges()
        {
            IProduction pro = new InitializeProduction().Initalize<T>();
           

            List<T> updtLst=new List<T>();
            List<T> addLst=new List<T>();



                foreach(T itm in Result)
                {
                        List<T> adLst = new List<T>();
                        adLst.Add(itm);
                        String query = pro.ExistSelectQuery<T>(adLst);
                        List<T> lstTst = new List<T>();
                        if (query.Contains("WHERE"))
                        {
                            var contxt = new DBMContext();
                            using (contxt)
                            {
                                lstTst = contxt.RetriveRecords<T>(query);
                            }

                          if(lstTst.Count>0)
                          {
                              updtLst.Add(itm);
                          }
                          else
                          {
                              addLst.Add(itm);
                          }
                        }
                        else
                        {
                            addLst.Add(itm);
                        }
                    
                }

           
            
                if (updtLst.Count > 0)
                {
                    Result = updtLst;
                    UpdateRecords();
                
                }
                if(addLst.Count>0)
                {
                    Result = addLst;
                    AddRecords();
                }
            

        }

        private void SyncDbEntity()
        {
            List<Type> theList = new List<Type>();
            MediaConnection.Instance().AllEntity = theList;
            theList.Add(typeof(T));
            var context = new DBMContext();
            context.SyncEntity();
        }

        private int NextAutoID()
        {
            var contxt = new DBMContext();
            using(contxt)
            {
              return Convert.ToInt32(contxt.NextIncrementID<T>());
            }
        }


       

    }
}
