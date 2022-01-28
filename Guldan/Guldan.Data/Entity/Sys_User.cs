using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Guldan.Common;
using FreeSql.DataAnnotations;

namespace Guldan.Data {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class SYS_USER
	{

		[JsonProperty, Column(StringLength = 64, IsPrimary = true, IsNullable = false)]
		public string ID { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string ADDRESS { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string CREATE_BY { get; set; }

		[JsonProperty, Column(DbType = "DATE(7)")]
		public DateTime? CREATE_TIME { get; set; }

		[JsonProperty, Column(DbType = "NUMBER(22)")]
		public decimal? IS_DELETED { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string MODIFY_BY { get; set; }

		[JsonProperty, Column(DbType = "DATE(7)")]
		public DateTime? MODIFY_TIME { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string NICK_NAME { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string PHONE { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string USER_CODE { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string USER_NAME { get; set; }

		[JsonProperty, Column(StringLength = 64)]
		public string VERSION { get; set; }

	}

}
