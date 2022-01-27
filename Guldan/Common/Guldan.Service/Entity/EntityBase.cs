using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Tls;
using FreeSql.DataAnnotations;

namespace Guldan.Service.Entity
{

    public class EntityBase<TKey> : Entity<TKey>, IEntityVersion, IEntitySoftDelete, IEntityAdd<TKey>, IEntityUpdate<TKey>  
    {
        /// <summary> 
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -9, IsVersion = true)]
        public string Version { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -8)]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 创建者Id
        /// </summary>
        [Description("创建者Id")]
        [Column(Position = -7, CanUpdate = false)]
        public string CreatedUserId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [Column(Position = -6, CanUpdate = false), MaxLength(50)]
        public string CreatedUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [Description("修改者Id")]
        [Column(Position = -4, CanInsert = false)]
        public string ModifiedUserId { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [Column(Position = -2, CanInsert = false), MaxLength(50)]
        public string ModifiedUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? ModifiedTime { get; set; }
    }

    /// <summary>
    /// 实体完整类
    /// </summary>
    public class EntityBase : EntityBase<string>
    {
    }

}
