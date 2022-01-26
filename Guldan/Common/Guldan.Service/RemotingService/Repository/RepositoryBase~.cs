using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common.Service;
using Guldan.Service.Dapper.Dao;
using Guldan.Service.Dapper.DbContext;
using Guldan.Service.Factory;

namespace Guldan.Service.RemotingService.Repository
{
    /// <summary>
    /// 抽象业务逻辑（输入输出控制，实体检查虚方法等）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRepository"></typeparam>
    public abstract class RepositoryBase<T, TRepository>
        : RepositoryAbs<T, TRepository>, IRepository<T, TRepository>, IRDefaultService<T>
         where T : class, new()
    {
        protected IDbContext context;
        protected IDbConnection conn;
        protected IDapperDao _dapperDao;
        public RepositoryBase(IDapperContext context, IDapperDao dapperDao)
        {
            this.context = context;
            conn = ((IDapperContext)this.context).Conn;
            _dapperDao = dapperDao;
            _dapperDao.InitConn(conn);
        }


        public T Add(T entity)
        {
            T data = null;
            context.UseTransaction(() =>
            {
                OnBeforeAdd(context, entity);
                data = _dapperDao.Insert<T, T>(entity);
                OnAfterAdd(context, entity);
            });
            return data;
        }
        public T Update(T entity)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                T data = null;
                conext.UseTransaction(() =>
                {
                    OnBeforeUpate(conext, entity);
                    // conext.Update(entity);
                    OnAfterUpate(conext, entity);
                });
                return data;
            }
        }

        public void Delete(T entity)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                conext.UseTransaction(() =>
                {
                    OnBeforeDelete(conext, entity);
                    // conext.Delete(entity);
                    OnAfterDelete(conext, entity);
                });
            }
        }
        public List<T> Adds(List<T> entities)
        {

            using (var conext = BizContextFactory.GetBizContext())
            {
                conext.UseTransaction(() =>
                {
                    foreach (T entity in entities)
                    {
                        OnBeforeAdd(conext, entity);
                    }
                    // conext.InsertRange(entities);
                    foreach (T entity in entities)
                    {
                        OnAfterAdd(conext, entity);
                    }
                });
            }

            return entities;
        }
        public List<T> Updates(List<T> entities)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {

                conext.UseTransaction(() =>
                {
                    foreach (T entity in entities)
                    {
                        OnBeforeUpate(conext, entity);
                    }
                    //conext.Update(entities);
                    foreach (T entity in entities)
                    {
                        OnAfterUpate(conext, entity);
                    }
                });
            }

            return entities;
        }
        public void Deletes(List<T> entities)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                conext.UseTransaction(() =>
                {
                    foreach (T entity in entities)
                    {
                        OnBeforeDelete(conext, entity);
                    }
                    // conext.Delete(entities);
                    foreach (T entity in entities)
                    {
                        OnAfterDelete(conext, entity);
                    }
                });
            }

        }
        public void DeleteByIDList(List<string> idList)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                conext.UseTransaction(() =>
                {
                    foreach (var id in idList)
                    {
                        // conext.DeleteByKey<T>((object)id);
                    }
                });

            }

        }
        public void DeleteByID(string id)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                conext.UseTransaction(() =>
                {
                    // conext.DeleteByKey<T>((object)id);
                });
            }
        }
        public T GetByID(string id)
        {
            using (var conext = BizContextFactory.GetBizContext())
            {
                // var data = conext.QueryByKey<T>((object)id);
                return default(T);
            }
        }

        public int GetCount(Dictionary<string, string> condition)
        {
            return 0;
        }

        public List<T> GetList(Dictionary<string, string> condition)
        {
            var entities = new List<T>();
            using (var conext = BizContextFactory.GetBizContext())
            {
                OnBeforeGetList(conext, entities);
                //Pure Query

            }
            return entities;
        }

        public T GetNew()
        {
            return new T();
        }



        PageInfo<T> IRQueryService<T>.GetRecords(Dictionary<string, string> condition)
        {
            throw new NotImplementedException();
        }

    }
}
