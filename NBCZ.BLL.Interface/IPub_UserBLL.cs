using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL.Interface
{
    public partial interface IPub_UserBLL
    {


        (bool, string) Add(V_PubUser_Dept model);


        (bool, string) Edit(V_PubUser_Dept model);

        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="roleCodes"></param>
        /// <returns></returns>
        bool SaveUserRole(string userCode, IEnumerable<string> roleCodes);

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        string GetCode();

        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool ChangeSotpStatus(string where, object pms);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Pub_User GetUserByUserName(string userName);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool EditPassWord(string userCode, string pwd);

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="functions"></param>
        /// <returns></returns>
        (bool, string) SaveFunctions(string code, List<Pub_UserFunction> functions);
    }
}
