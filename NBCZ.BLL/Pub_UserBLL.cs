using NBCZ.DAL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBCZ.BLL
{
    public partial class  Pub_UserBLL
    {
        Pub_UserDAL dal = new Pub_UserDAL();
        Pub_UserRoleBLL userRoleBLL = new Pub_UserRoleBLL();
        Pub_UserFunctionBLL userFunctionBLL = new Pub_UserFunctionBLL();


        public (bool, string) Add(V_PubUser_Dept model)
        {
            Pub_User user = model.MapTo<V_PubUser_Dept, Pub_User>();

            user.UserCode = GetCode();
            var r = Insert(user);
            if (r <= 0)
            {
                return (false, "保存用户失败！");
            }

            SaveUserRole(user.UserCode, model.RoleCodes);
            return (true, "保存成功");
        }


        public (bool, string) Edit(V_PubUser_Dept model)
        {
            Pub_User user = model.MapTo<V_PubUser_Dept, Pub_User>();
            var r = Update(user);
            if (!r)
            {
                return (false, "保存用户失败！");
            }

            SaveUserRole(user.UserCode, model.RoleCodes);
            return (true, "保存成功");
        }


        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="roleCodes"></param>
        /// <returns></returns>
        public bool SaveUserRole(string userCode, IEnumerable<string> roleCodes)
        {
            userRoleBLL.DeleteByWhere($"userCode={userCode}");
            if (roleCodes==null)
            {
                return true;
            }
            List<Pub_UserRole> userRoles = roleCodes.Select<string, Pub_UserRole>(p =>
            {
                return new Pub_UserRole()
                {
                    UserCode = userCode,
                    RoleCode = p
                };
            }).ToList();

            userRoleBLL.InsertBatch(userRoles);

            return true;
        }
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "00000001";
            List<Pub_User> users = GetList("", " Id Desc ", 1);
            if (users.Count > 0)
            {
                var model = users.First();
                code = (Convert.ToInt32(model.UserCode.Remove(0, 1)) + 1).ToString().PadLeft(8, '0');
            }

            return code;
        }

        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool ChangeSotpStatus(string where, object pms)
        {

            return dal.ChangeSotpStatus(where, pms);
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Pub_User GetUserByUserName(string userName) 
        {
            var dbUser = GetList(string.Format(" StopFlag=0 AND UserName='{0}' ",
                  userName)).FirstOrDefault();

            return dbUser;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool EditPassWord(string userCode,string pwd)
        {
           return dal.EditPassWord(userCode,pwd);
         }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="functions"></param>
        /// <returns></returns>
        public (bool, string) SaveFunctions(string code, List<Pub_UserFunction> functions)
        {
            var r = userFunctionBLL.DeleteByWhere($"UserCode='{code}'");
            r = userFunctionBLL.InsertBatch(functions);

            return (r, r ? "保存成功" : "保存失败");

        }
    }
}
