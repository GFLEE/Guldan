using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;

namespace Guldan.Service.Entity
{
    public interface IEntityUpdate<TKey>
    {
        string ModifiedUserId { get; set; }
        string ModifiedUserName { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
    public interface IEntityAdd<TKey>
    {
        string CreatedUserId { get; set; }
        string CreatedUserName { get; set; }
        DateTime? CreatedTime { get; set; }
    }
    public interface IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        string Version { get; set; }
    }
    public interface IEntitySoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
    public interface IEntity<TKey>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        //TKey Id { get; set; }
    }

    public interface IEntity : IEntity<string>
    {
    }

    public class Entity<TKey> : IEntity<TKey>
    {
        ///// <summary>
        ///// 主键Id
        ///// </summary>
        //[Description("主键Id")]
        //[Column(Position = 1, IsIdentity = false, IsPrimary = true)]
        //public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<string>
    {
    }
}
