using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL.Interface
{

    public partial interface IPub_RoleBLL
    {
        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
       string GetCode();

        (bool, string) SaveFunctions(string code, List<Pub_RoleFunction> functions);
    }
}
