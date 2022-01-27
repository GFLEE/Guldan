using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Guldan.Service.Entity;
using FreeSql.DataAnnotations;

namespace Guldan.Data {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class SYS_USER : EntityBase
	{

		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)", IsPrimary = true, IsNullable = false)]
		public string ID { get; set; }

		/// <summary>
		/// 地址
		/// </summary>
		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)")]
		public string ADDRESS { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty, Column(DbType = "DATE(7)")]
		public DateTime? CREATE_DATE { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)")]
		public string NICK_NAME { get; set; }

		/// <summary>
		/// 电话号码
		/// </summary>
		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)")]
		public string PHONE { get; set; }

		/// <summary>
		/// 用户编码
		/// </summary>
		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)")]
		public string USER_CODE { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[JsonProperty, Column(DbType = "VARCHAR2(64 BYTE)")]
		public string USER_NAME { get; set; }

	}

}
