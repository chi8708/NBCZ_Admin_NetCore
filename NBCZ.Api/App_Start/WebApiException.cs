using Microsoft.AspNetCore.Mvc.Filters;
using NBCZ.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace NBCZ.Api
{
    /// <summary>
    /// 异常捕获
    /// </summary>
    public class WebApiExceptionAttribute : ExceptionFilterAttribute
    {
       
        public override void OnException(ExceptionContext context)
        {
            Task.Run(()=>WriteErrorLog(context));
            base.OnException(context);
        }

        private void WriteErrorLog(ExceptionContext context)
        {

            var request = context.HttpContext.Request;
            var body = request.Body;
            var path= request.Path+request.QueryString;
            var postData = "";
            if (request.Method.ToUpper()=="POST"&& request.ContentLength > 0)
            {
                //application / json; charset = utf - 8
                if (request.ContentType.IndexOf("application/json")>=0)
                {
                    //Microsoft.AspNetCore.Http.Internal 
                    //  Request.EnableRewind();
                    body.Position = 0;//body.Position = 0;才能读到数据
                    var bytes = new byte[body.Length];
                    body.Read(bytes, 0, bytes.Length);
                    postData = System.Text.Encoding.UTF8.GetString(bytes);
                }
               
            }

            var log = LogFactory.GetLogger(path);
            log.Error($"path:{path},method:{request.Method},data:{postData}\r\n error:{context.Exception.Message}\r\n stack:{context.Exception.StackTrace}");
        }
    }
}