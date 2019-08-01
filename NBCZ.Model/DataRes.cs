using System;
using System.Collections.Generic;
using System.Text;

namespace NBCZ.Model
{
    public class DataRes<T>
    {

        public ResCode code { get; set; } = ResCode.Success;

        public string msg { get; set; } = "成功";

        public T data { get; set; }
    }

    public enum ResCode
    {
        /// <summary>
        /// 错误
        /// </summary>
       Error=-1,
       /// <summary>
       /// 验证未通过
       /// </summary>
       NoValidate=0,
       /// <summary>
       /// 成功
       /// </summary>
       Success = 1
    }
}
