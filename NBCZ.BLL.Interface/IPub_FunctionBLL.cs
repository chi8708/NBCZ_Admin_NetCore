using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBCZ.BLL.Interface
{
   public partial interface IPub_FunctionBLL
    {
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
    string GetCode(string parentCode);
    }
}
