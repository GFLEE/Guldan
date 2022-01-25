using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guldan.Service.RemotingService
{
    /// <summary>
    /// 带有固定接口的服务（增删改）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRDefaultService<T> : IRService, IRQueryService<T>
    {
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// 插入对象集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<T> Adds(List<T> entities);
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);
        /// <summary>
        /// 更新对象集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        List<T> Updates(List<T> entities);
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        void DeleteByID(long id);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="entities"></param>
        void Deletes(List<T> entities);
        /// <summary>
        /// 根据id集合删除对象
        /// </summary>
        /// <param name="idList"></param>
        void DeleteByIDList(List<long> idList);
        /// <summary>
        /// 获取新对象
        /// </summary>
        /// <returns></returns>
        T GetNew();
    }
}
