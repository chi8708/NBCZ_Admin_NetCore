using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NBCZ.Common
{
    public static class InjectionFilterUtil
    {
        #region 字符串过滤 sql注入、跨站脚本注入等
        ///<summary>
        /// 过滤危险字符
        ///</summary>
        ///<param name="inputString">需要过滤字符串</param>
        ///<returns>过滤后的字符串</returns>
        public static string InjectionFilter(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
            {
                string xss = XSSFilter(inputString);
                string flash = FlashFilter(xss);
                string sql = SqlFilter(flash);
                return sql;
            }
            else
            {
                return string.Empty;
            }
        }

        ///<summary>
        /// 过滤字符串中的注入跨站脚本(先进行UrlDecode再过滤脚本关键字)
        /// 过滤脚本如:<script>window.alert("test");</script>输出window.alert("test");
        /// 如<a href = "javascript:onclick='fun1();'">输出<a href=" onXXX='fun1();'">
        /// 过滤掉javascript和 onXXX
        ///</summary>
        ///<param name="source">需要过滤的字符串</param>
        ///<returns>过滤后的字符串</returns>
        private static string XSSFilter(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            string result = HttpUtility.UrlDecode(source);

            string replaceEventStr = " onXXX =";
            string tmpStr = "";

            string patternGeneral = @"<[^<>]*>";                              //例如 <abcd>
            string patternEvent = @"([\s]|[:])+[o]{1}[n]{1}\w*\s*={1}";     // 空白字符或: on事件=
            string patternScriptBegin = @"\s*((javascript)|(vbscript))\s*[:]?";  // javascript或vbscript:
            string patternScriptEnd = @"<([\s/])*script.*>";                       // </script>
            string patternTag = @"<([\s/])*\S.+>";                       // 例如</textarea>

            Regex regexGeneral = new Regex(patternGeneral, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexEvent = new Regex(patternEvent, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexScriptEnd = new Regex(patternScriptEnd, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexScriptBegin = new Regex(patternScriptBegin, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexTag = new Regex(patternTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);


            Match matchGeneral, matchEvent, matchScriptEnd, matchScriptBegin, matchTag;

            //符合类似 <abcd> 条件的
            #region 符合类似 <abcd> 条件的
            //过滤字符串中的 </script>   
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptEnd = regexScriptEnd.Match(tmpStr);
                if (matchScriptEnd.Success)
                {
                    tmpStr = regexScriptEnd.Replace(tmpStr, "");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的脚本事件
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchEvent = regexEvent.Match(tmpStr);
                if (matchEvent.Success)
                {
                    tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的 javascript或vbscript:
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptBegin = regexScriptBegin.Match(tmpStr);
                if (matchScriptBegin.Success)
                {
                    tmpStr = regexScriptBegin.Replace(tmpStr, "");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            #endregion

            //过滤字符串中的事件 例如 onclick --> onXXX
            for (matchEvent = regexEvent.Match(result); matchEvent.Success; matchEvent = matchEvent.NextMatch())
            {
                tmpStr = matchEvent.Groups[0].Value;
                tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                result = result.Replace(matchEvent.Groups[0].Value, tmpStr);
            }

            //过滤掉html标签，类似</textarea>
            for (matchTag = regexTag.Match(result); matchTag.Success; matchTag = matchTag.NextMatch())
            {
                tmpStr = matchTag.Groups[0].Value;
                tmpStr = regexTag.Replace(tmpStr, "");
                result = result.Replace(matchTag.Groups[0].Value, tmpStr);
            }


            return result;
        }

        ///<summary>
        /// 过滤字符串中注入Flash代码
        ///</summary>
        ///<param name="htmlCode">输入字符串</param>
        ///<returns>过滤后的字符串</returns>
        private static string FlashFilter(string htmlCode)
        {
            if (string.IsNullOrEmpty(htmlCode)) return htmlCode;

            string pattern = @"\w*<OBJECT\s+.*(macromedia)[\s*|\S*]{1,1300}</OBJECT>";

            return Regex.Replace(htmlCode, pattern, "", RegexOptions.Multiline);
        }

        /// <summary>
        /// 过滤危险的字符（SQL注入）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string SqlFilter(this string str)
        {
            var ext = new[] { "and", "exec", "insert", "select", "delete", "update", "chr", "mid", "master", " or ", "truncate", "char", "declare", "join", "\r", "\n", "'" };

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
        #endregion
    }
}
