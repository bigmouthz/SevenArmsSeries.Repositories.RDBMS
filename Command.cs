using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SevenArmsSeries.Repositories;
using SevenArmsSeries.Repositories.RDBMS.Core;

namespace SevenArmsSeries.Repositories.RDBMS
{
    public class Command : ICommand
    {
        public CommandResponse Execute(CommandRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Guid) || !SQLEntityFactory.Items.ContainsKey(request.Guid))
                throw new Exception("Error: requestGuid is null or is not exists! ");
            SQLEntityFactory.SQLEntity sqlEntity = SQLEntityFactory.Items[request.Guid];
            return Execute(request, sqlEntity);
        }


        public CommandResponse Execute(CommandRequest request, SQLEntityFactory.SQLEntity sqlEntity)
        {
            string sql = sqlEntity.Items[request.Command];

            IList<SqlParams> sqlparams = new List<SqlParams>();
            foreach (var q in request.Params)
            {
                sqlparams.Add(new SqlParams()
                                  {
                                      ColumnName = q.Name,
                                      ColumnType = string.IsNullOrWhiteSpace( q.Type)
                                                       ? SQLHelper.GetDbType(q.Type)
                                                       : DbType.String,
                                      Value = q.Value
                                  });
            }
            CommandResponse response = new CommandResponse();
            if (request.TrueScalar_FalseIntByResult)
            {
                response.ResultValue = SQLHelper.ExecuteScalar(sqlEntity.dbname, sql, sqlparams);
            }
            else
            {
                response.ResultValue = SQLHelper.ExecuteNonQuery(sqlEntity.dbname, sql, sqlparams);
            }
            return response;
        }
    }
}
