using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.DAL.Interface
{
    public partial interface IPub_UserDAL
    {
        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool ChangeSotpStatus(string where, object pms);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool EditPassWord(string userCode, string pwd);
    }
}
