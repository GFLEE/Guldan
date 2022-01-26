using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guldan.Common.Enum;

namespace Guldan.Service.Dapper
{
    public class DapperDao
    {
        private static EuDialect _dialect = EuDialect.Oracle;
        private static string _encapsulation;
        private static string _getIdentitySql;
        private static string _getPagedListSql;

        public DapperDao()
        {

            SetDialect(_dialect);

        }




        public static void SetDialect(EuDialect EuDialect)
        {
            switch (EuDialect)
            {
                case EuDialect.PostgreSQL:
                    _dialect = EuDialect.PostgreSQL;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format("SELECT LASTVAL() AS id");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                    break;
                case EuDialect.SQLite:
                    _dialect = EuDialect.SQLite;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format("SELECT LAST_INSERT_ROWID() AS id");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {RowsPerPage} OFFSET (({PageNumber}-1) * {RowsPerPage})";
                    break;
                case EuDialect.MySQL:
                    _dialect = EuDialect.MySQL;
                    _encapsulation = "`{0}`";
                    _getIdentitySql = string.Format("SELECT LAST_INSERT_ID() AS id");
                    _getPagedListSql = "Select {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy} LIMIT {Offset},{RowsPerPage}";
                    break;
                case EuDialect.Oracle:
                    _dialect = EuDialect.Oracle;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = "";
                    _getPagedListSql = "SELECT * FROM (SELECT ROWNUM PagedNUMBER, u.* FROM(SELECT {SelectColumns} from {TableName} {WhereClause} Order By {OrderBy}) u) WHERE PagedNUMBER BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
                case EuDialect.DB2:
                    _dialect = EuDialect.DB2;
                    _encapsulation = "\"{0}\"";
                    _getIdentitySql = string.Format("SELECT CAST(IDENTITY_VAL_LOCAL() AS DEC(31,0)) AS \"id\" FROM SYSIBM.SYSDUMMY1");
                    _getPagedListSql = "Select * from (Select {SelectColumns}, row_number() over(order by {OrderBy}) as PagedNumber from {TableName} {WhereClause} Order By {OrderBy}) as t where t.PagedNumber between (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
                default:
                    _dialect = EuDialect.SQLServer;
                    _encapsulation = "[{0}]";
                    _getIdentitySql = string.Format("SELECT CAST(SCOPE_IDENTITY()  AS BIGINT) AS [id]");
                    _getPagedListSql = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNumber BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
                    break;
            }
        }

        public static string GetDialect()
        {
            return _dialect.ToString();
        }


    }
}
