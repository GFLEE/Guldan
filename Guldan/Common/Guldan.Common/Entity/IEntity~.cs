using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;

namespace Guldan.Common
{
    public interface IEntityUpdate<TKey>
    {
        string Modify_By { get; set; }
        DateTime? Modify_Time { get; set; }
    }
    public interface IEntityAdd<TKey>
    {
        string Create_By { get; set; }
        DateTime? Create_Time { get; set; }
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
        bool Is_Deleted { get; set; }
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
