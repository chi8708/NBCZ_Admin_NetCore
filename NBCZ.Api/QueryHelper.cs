using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NBCZ.Api
{

    /// <summary>
    /// 搜索条件
    /// </summary>
    public static class QueryHelper
    {

        /// <summary>
        /// 获取搜索query
        /// </summary>
        public static string GetWhereStr(this HttpContext context)
        {
            var request = context.Request;
            var body = request.Body;
            var postData = "";
            var query = new Dictionary<string, object>();
            var r= GetQuery();
            if (!r)
            {
                return "-1";
            }
            bool GetQuery()
            {
                try
                {

                    if (request.ContentType.IndexOf("application/json") >= 0)
                    {
                        body.Position = 0;//body.Position = 0;才能读到数据
                        var bytes = new byte[body.Length];
                        body.Read(bytes, 0, bytes.Length);
                        postData = System.Text.Encoding.UTF8.GetString(bytes);
                    }
                    if (string.IsNullOrEmpty(postData))
                    {
                        return false;
                    }

                    var postDataJson = JsonConvert.DeserializeObject<dynamic>(postData);
                    query =JsonConvert.DeserializeObject<Dictionary<string, object>>(postDataJson.query.ToString());
                    if (query == null || query.Keys.Count <= 0)
                    {
                        return true;
                    }
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

            return context.GetWhereStr(query);
        }

        public static string GetWhereStr(this HttpContext context, Dictionary<string, object> query)
        {
            string strWhere = " 1=1 ";
            if (query == null)
            {
                return strWhere;
            }
            var keys = query.Select(p => p.Key);
            var parms = keys.Where(p => (p.Contains("SL_")
                || p.Contains("SEB_")) || p.Contains("SES_") || p.Contains("SEGT_") || p.Contains("SELT_") || p.Contains("SEI_") || p.Contains("SENE_") || p.Contains("SLL_") || p.Contains("SLR_"));
            foreach (var parm in parms)
            {
                var name = parm.Split('_');
                var keyPosition = name[0].Length + 1;
                var fieldName = parm.Substring(keyPosition, parm.Length - keyPosition);

                var value = query[parm].ToString().Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                value = SqlFilter(value);

                switch (name[0])
                {
                    case "SL": strWhere += string.Format(" And {0} like '%{1}%' ", fieldName, value); break;
                    case "SLL": strWhere += string.Format(" And {0} like '%{1}' ", fieldName, value); break;
                    case "SLR": strWhere += string.Format(" And {0} like '{1}%' ", fieldName, value); break;
                    case "SEB": strWhere += string.Format(" And {0}={1} ", fieldName, value); break;
                    case "SEI": strWhere += string.Format(" And {0} in({1})", fieldName, value); break;
                    case "SES": strWhere += string.Format(" And {0}='{1}'", fieldName, value); break;
                    case "SEGT": strWhere += string.Format(" And {0}>='{1}' ", fieldName, value); break;
                    case "SELT": strWhere += string.Format(" And {0}<='{1}' ", fieldName, value); break;
                    case "SENE": strWhere += string.Format(" And {0}<>'{1}' ", fieldName, value); break;
                    default:
                        break;
                }
            }

            return strWhere;
        }
        /// <summary>
        /// 过滤危险的字符（SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SqlFilter(this string str)
        {
            var ext = new[] { "and", "exec", "insert", "select", "delete", "update", "chr", "mid", "master", "or", "truncate", "char", "declare", "join", "\r", "\n", "'" };

            if (str.Contains("'"))
            {
                str = str.Replace("'", "''");
            }
            else
            {
                if (!string.IsNullOrEmpty(str) && str.Length >= 3)
                {
                    foreach (var e in ext.Where(e => str.ToLower().IndexOf(e, StringComparison.Ordinal) != -1))
                    {
                        str = Regex.Replace(str, e, "", RegexOptions.IgnoreCase);
                    }
                }

                if (str.Length >= 128)
                    str = "";
            }


            return str;
        }



        /// <summary>
        /// 生成自增编号的SQL执行字符
        /// 如审批流自增编号AP00000001
        /// 最后一个编号是  AP00000009
        /// 本方法生成的SQL执行字符执行后反回AP00000010
        /// 本方法生成的SQL字符，只做计算，不做存储。计算返回自增后的编号
        /// </summary>
        /// <param name="increment">增量, 最后一个编号需要相加的数字, 如AP00000010 + 1 ＝ AP00000011，1就是增量</param>
        /// <param name="field">需要增量的字段名称，如审批流表M3_ApprovalSetting的ApprovalCode</param>
        /// <param name="table">表名称</param>
        /// <param name="prefix">前缀, 如AP00000011，前缀AP</param>
        /// <param name="digitBit">位数,如AP00000011,数字位8位</param>
        /// <returns></returns>
        public static string GenerateIncrementCodeSQLStr(int increment, string field, string table, string prefix, int digitBit)
        {
            string digitBitStr = ("").PadLeft(digitBit, '0');
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("DECLARE @CodeNum VARCHAR(50) ");
            sql.AppendLine("DECLARE @resCode VARCHAR(50) ");
            sql.AppendLine("DECLARE @addNum INT ");
            sql.AppendFormat("SET @addNum={0} ", increment.ToString());
            sql.AppendFormat("SELECT @CodeNum = REPLACE((SELECT MAX({0}) AS {0} FROM [dbo].[{1}]), '{2}', '') ", field, table, prefix);
            sql.AppendFormat("SET @CodeNum = ISNULL(@CodeNum, '{0}')", digitBitStr);
            sql.AppendFormat("SELECT @resCode = ('{0}' + REPLICATE('0',(Len(@CodeNum)-LEN(@CodeNum+@addNum))) + CAST((@CodeNum+@addNum) AS VARCHAR)) ", prefix);
            sql.AppendLine("SELECT @resCode");
            return sql.ToString();
        }

     
    }
}
