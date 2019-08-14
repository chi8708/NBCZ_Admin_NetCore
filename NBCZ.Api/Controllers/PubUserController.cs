using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCZ.Api.Model.Request;
using NBCZ.BLL;
using NBCZ.Model;

namespace NBCZ.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/PubUser")]
    public class PubUserController : BaseController
    {
        private Pub_UserBLL bll = new Pub_UserBLL();
        private V_PubUser_DeptBLL userDeptBLL = new V_PubUser_DeptBLL();
        private Pub_UserRoleBLL userRoleBLL = new Pub_UserRoleBLL();
        Pub_UserFunctionBLL userFunctionBLL = new Pub_UserFunctionBLL();

        [Route("GetAccess")]
        public dynamic GetAccess()
        {
            // var userCode = User.Identity.Name;

            //var c = (a:1,b:2);
            //return (
            //    access: new List<string>() { "super_admin", "admin" },
            //    avatar: "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
            //    name: "super_admin",
            //    user_id:"1"
            //    );
            var user = new NBCZUser(User);
            var userName = user.UserName;
            var userCode = user.UserCode;
            var access = user.Access;
            return new
            {
                access = access,
                avatar = "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
                name = userName,
                user_id = userCode
            };
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <returns></returns>
        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<V_PubUser_Dept> GetPage([FromBody]PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr=="-1")
            {
                return new PageDateRes<V_PubUser_Dept>() {code=ResCode.Error,msg="查询参数有误！",data=null };
            }
            var users = userDeptBLL.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            //  PageDateRes<V_PubUser_DeptExt> users = usersPage.MapTo<PageDateRes <V_PubUser_Dept>,PageDateRes <V_PubUser_DeptExt>>();
            var userCodes = string.Join("','", users.data.Select(p => p.UserCode));
            List<Pub_UserRole> userRoles = userRoleBLL.GetList($"userCode in ('{userCodes}')");
            users.data.ForEach(p =>
            {
                p.RoleCodes = userRoles.Where(c => c.UserCode == p.UserCode).Select(d => d.RoleCode);
            });

            return users;
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");
            var query = this.HttpContext.GetWhereStr();
            if (query=="-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            return sb.ToString();
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]V_PubUser_Dept model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.Crdt = model.Lmdt = DateTime.Now;
            var user = new NBCZUser(User);
            model.Crid = model.Lmid = $"{user.UserCode}-{user.UserName}";
            var r = bll.Add(model);
            if (!r.Item1)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = r.Item2;
            }

            return res;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody]V_PubUser_Dept model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.Lmdt = DateTime.Now;
            var user = new NBCZUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
            var r = bll.Edit(model);
            if (!r.Item1)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = r.Item2;
            }

            return res;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(long id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.ChangeSotpStatus($"id={id}", null);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }


        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <returns></returns>
        [Route("GetFunctions/{code}")]
        [HttpPost]
        public DataRes<IEnumerable<string>> GetFunctions(string code)
        {
            DataRes<IEnumerable<string>> res = new DataRes<IEnumerable<string>>() { code = ResCode.Success };

            var list = userFunctionBLL.GetList($"userCode='{code}'");
            res.data = list.Select(p=>p.FunctionCode);

            return res;
        }

    }
}