using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;
using SevenArmsSeries.Repositories;
using SevenArmsSeries.Repositories.RDBMS.Core;

namespace SevenArmsSeries.Repositories.RDBMS
{
    public class QueryByMore<TDto> :QueryByCount, IQueryByMore<TDto>
    {

        #region IMoreQuery<TDto> Members

        public QueryByMoreResponse<TDto> Gets(QueryByMoreRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Guid) || !SQLEntityFactory.Items.ContainsKey(request.Guid))
                throw new Exception("Error: requestGuid is null or is not exists! ");
            SQLEntityFactory.SQLEntity sqlEntity = SQLEntityFactory.Items[request.Guid];
            return Gets(request, sqlEntity);
        }

        public QueryByMoreResponse<TDto> Gets(QueryByMoreRequest request, SQLEntityFactory.SQLEntity sqlEntity)
        {
            string sqlByQueryMore = sqlEntity.sqlByQueryMore;
            string sqlByQueryMoreCount = sqlEntity.sqlByQueryMoreCount;

            StringBuilder sqlsort = new StringBuilder();
            ; foreach (var s in request.ParamSort)
            {
                sqlsort.AppendFormat("{0} {1},", s.Key, s.Value);
            }
            if (sqlsort.Length > 0) sqlsort.Length--;
            sqlByQueryMore = string.Format(sqlByQueryMore, request.ParamPlus, sqlsort.ToString());
            sqlByQueryMoreCount = string.Format(sqlByQueryMoreCount, request.ParamPlus, sqlsort.ToString());

            IList<SqlParams> sqlparams = new List<SqlParams>();
            foreach (var q in request.Params)
            {
                sqlparams.Add(new SqlParams()
                {
                    ColumnName = q.Name,
                    ColumnType =string.IsNullOrWhiteSpace(q.Type)
                                     ? SQLHelper.GetDbType(q.Type)
                                     : DbType.String,
                    Value = q.Value
                });
            }

            QueryByMoreResponse<TDto> result = new QueryByMoreResponse<TDto>();
            result.TotalRowCount = (int)SQLHelper.ExecuteScalar(sqlEntity.dbname, sqlByQueryMoreCount, sqlparams);
            if (!request.canpaging)
            {
                result.Rows = SQLHelper.Gets<TDto>(SQLHelper.ExecuteIDataReader(sqlEntity.dbname, sqlByQueryMore, sqlparams));
            }
            else
            {
                if (!request.cancache)
                {
                    result.Rows = SQLHelper.Gets<TDto>(SQLHelper.ExecuteIDataReader(sqlEntity.dbname, sqlByQueryMore, sqlparams),
                                                request.pageindex,
                                                request.pagesize);
                }
                else
                {
                    result.Rows = SQLHelper.Gets<TDto>(SQLHelper.ExecuteIDataReader(sqlEntity.dbname, sqlByQueryMore, sqlparams),
                                                request.pageindex,
                                                request.pagesize,
                                                request.cahcepagecount);
                }
            }
            return result;
        }

        #endregion
    }
}
