using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL.Interface
{
    public partial interface IPub_DepartmentBLL
    {

        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        string GetCode();
    }
}
