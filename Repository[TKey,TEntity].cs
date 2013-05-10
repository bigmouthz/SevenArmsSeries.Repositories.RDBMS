using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SevenArmsSeries.Repositories;
using SevenArmsSeries.Repositories.RDBMS.Core;

namespace SevenArmsSeries.Repositories.RDBMS
{   
    public class Repository<TKey, TEntity> :  IRepository< TKey,TEntity> 
        where TKey :IKey
        where TEntity :IEntity
    {
        #region IRepository<TKey,TEntity> Members

        public RepositoryResponse Save(RepositoryRequest<TKey, TEntity> request)
        {
            if (string.IsNullOrWhiteSpace(request.Guid) || !SQLEntityFactory.Items.ContainsKey(request.Guid))
                throw new Exception("Error: requestGuid is null or is not exists! ");
            SQLEntityFactory.SQLEntity sqlEntity = SQLEntityFactory.Items[request.Guid];
            return Save(request, sqlEntity);
        }

        public RepositoryResponse Save(RepositoryRequest<TKey, TEntity> request, SQLEntityFactory.SQLEntity sqlEntity)
        {
            string sqlCreate = sqlEntity.sqlByRepositoryCreate;
            string sqlUpdate = sqlEntity.sqlByRepositoryUpdate;
            string sqlRemove = sqlEntity.sqlByRepositoryRemove;

            RepositoryResponse result = new RepositoryResponse();

            int cnt = 0;
            int failcnt = 0;
            foreach (var e in request.CreateEntities)
            {     
               try
                {
                    cnt += SQLHelper.ExecuteNonQuery(sqlEntity.dbname, sqlEntity.sqlByRepositoryCreate,e.GetSQLParams());
                }
                catch (Exception ex)
                {
                    failcnt++;
                    result.CreateMessage.Add(e.GetKey().ToString(), ex.Message);
                }
            }

            foreach (var e in request.UpdateEntities)
            {
                try
                {
                    cnt += SQLHelper.ExecuteNonQuery(sqlEntity.dbname, sqlEntity.sqlByRepositoryUpdate, e.GetSQLParams());
                }
                catch (Exception ex)
                {
                    failcnt++;
                    result.UpdateMessage.Add(e.GetKey().ToString(), ex.Message);
                }
            }

            foreach (var e in request.RemoveEntities)
            {
                try
                {
                    cnt += SQLHelper.ExecuteNonQuery(sqlEntity.dbname, sqlEntity.sqlByRepositoryRemove, e.GetSQLParams());
                }
                catch (Exception ex)
                {
                    failcnt++;
                    result.RemoveMessage.Add(e.ToString(), ex.Message);
                }
            }

            result.Verdict = failcnt == 0;
            return result;
        }

        #endregion
    }
}
