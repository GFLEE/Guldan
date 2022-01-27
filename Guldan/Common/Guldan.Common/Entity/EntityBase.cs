using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Tls;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;

namespace Guldan.Common
{

    public class EntityBase<TKey> : Entity<TKey>, IEntityVersion, IEntitySoftDelete, IEntityAdd<TKey>, IEntityUpdate<TKey>
    {
        /// <summary>
        /// 创建者
        /// </summary>
        [Description("创建者")]
        [JsonProperty, Column(Position = -7, CanUpdate = false, DbType = "VARCHAR2(64 BYTE)")]
        public string Create_By { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [JsonProperty, Column(Position = -5, CanUpdate = false, ServerTime = DateTimeKind.Local, DbType = "DATE(7)")]
        public DateTime? Create_Time { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Description("修改者")]
        [JsonProperty, Column(Position = -2, CanInsert = false, DbType = "VARCHAR2(64 BYTE)"), MaxLength(50)]
        public string Modify_By { get; set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [JsonProperty, Column(Position = -1, CanInsert = false, DbType = "DATE(7)", ServerTime = DateTimeKind.Local)]
        public DateTime? Modify_Time { get; set; }


        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [JsonProperty, Column(Position = -8, DbType = "VARCHAR2(64 BYTE)")]
        public bool Is_Deleted { get; set; } = false;

        /// <summary> 
        /// 版本
        /// </summary>
        [Description("版本")]
        [JsonProperty, Column(Position = -9, IsVersion = true)]
        public string Version { get; set; }



    }


    public class EntityBase : EntityBase<string>
    {


    }

}
