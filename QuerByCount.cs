using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using SevenArmsSeries.Repositories;
using SevenArmsSeries.Repositories.RDBMS.Core;

namespace SevenArmsSeries.Repositories.RDBMS
{
    public class QueryByCount :IQueryByCount 
    {
        #region ISingleQuery<TDto> Members        
        
        public int GetCount(QueryBySingleRequest request) 
        {
            return GetCount(request, "sqlByQueryCount");
        }

        public int GetCount(QueryBySingleRequest request, string itemKey) 
        {
            if (string.IsNullOrWhiteSpace(request.Guid) || !SQLEntityFactory.Items.ContainsKey(request.Guid))
                throw new Exception("Error: requestGuid is null or is not exists! ");
            SQLEntityFactory.SQLEntity sqlEntity = SQLEntityFactory.Items[request.Guid];
            return GetCount(request, sqlEntity, itemKey);
        }

        public int GetCount(QueryBySingleRequest request, SQLEntityFactory.SQLEntity sqlEntity, string itemKey = "sqlByQueryCount")
        {
            string sql = sqlEntity.Items[itemKey];

            StringBuilder sqlsort = new StringBuilder();
            ; foreach (var s in request.ParamSort)
            {
                sqlsort.AppendFormat("{0} {1},", s.Key, s.Value);
            }
            if (sqlsort.Length > 0) sqlsort.Length--;
            sql = string.Format(sql, request.ParamPlus, sqlsort.ToString());

            IList<SqlParams> sqlparams = new List<SqlParams>();
            foreach (var q in request.Params)
            {
                sqlparams.Add(new SqlParams()
                                  {
                                      ColumnName = q.Name,
                                      ColumnType = string.IsNullOrWhiteSpace(q.Type)
                                                       ? SQLHelper.GetDbType(q.Type)
                                                       : DbType.String,
                                      Value = q.Value
                                  });
            }
            object obj = SQLHelper.ExecuteScalar(sqlEntity.dbname, sql, sqlparams);
            return  int.Parse(obj.ToString());
        }
        #endregion
    }
}
