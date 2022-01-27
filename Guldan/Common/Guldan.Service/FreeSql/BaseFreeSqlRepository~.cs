using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Service.RemotingService.Repository;
using Guldan.Service.RemotingService;
using Guldan.Common.Service;

namespace Guldan.Service.FreeSql
{
    public class BaseFreeSqlRepository<T, TRepository>
        : RepositoryAbs<T, TRepository>, IRepository<T, TRepository>, IRDefaultService<T>
    where T : class, new()
    {
        public T Add(T entity)
        {
            //T data = null;
            //OnBeforeAdd(IFreeSql, entity);
            //data = _dapperDao.Insert<T, T>(entity);
            //OnAfterAdd(IFreeSql, entity);
            return entity;
        }

        public List<T> Adds(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteByID(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteByIDList(List<string> idList)
        {
            throw new NotImplementedException();
        }

        public void Deletes(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public T GetByID(string id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Dictionary<string, string> condition)
        {
            throw new NotImplementedException();
        }

        public List<T> GetList(Dictionary<string, string> condition)
        {
            throw new NotImplementedException();
        }

        public T GetNew()
        {
            throw new NotImplementedException();
        }

        public PageInfo<T> GetRecords(Dictionary<string, string> condition)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public List<T> Updates(List<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
