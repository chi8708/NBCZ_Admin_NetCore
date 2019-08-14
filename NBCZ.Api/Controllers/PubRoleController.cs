using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/PubRole")]
    [Authorize]
    public class PubRoleController : Controller
    {
        Pub_RoleBLL bll = new Pub_RoleBLL();
        Pub_RoleFunctionBLL roleFunctionBLL = new Pub_RoleFunctionBLL();

        [Route("GetList")]
        [HttpPost]
        public DataRes<List<Pub_Role>> GetList()
        {
            var roles = bll.GetList("StopFlag=0");

            return new DataRes<List<Pub_Role>> { data= roles} ;
        }

        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <returns></returns>
        [Route("GetPage")]
        [HttpPost]
        public PageDateRes<Pub_Role> GetPage([FromBody]PageDataReq pageReq)
        {
            var whereStr = GetWhereStr();
            if (whereStr == "-1")
            {
                return new PageDateRes<Pub_Role>() { code = ResCode.Error, msg = "查询参数有误！", data = null };
            }
            var list = bll.GetPage(whereStr, (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return list;
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");
            var query = this.HttpContext.GetWhereStr();
            if (query == "-1")
            {
                return query;
            }
            sb.AppendFormat(" and {0} ", query);

            return sb.ToString();
        }


        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]Pub_Role model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.RoleCode = bll.GetCode();
            model.Lmdt = model.Lmdt = DateTime.Now;
            var user = NBCZFactory.GetNBCZUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
            model.StopFlag = false;
            var r = bll.Insert(model)>0;

            if (!r)
            {
                res.code = ResCode.Error;
                res.msg = "保存失败！";
            }

            return res;
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody]Pub_Role model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.Lmdt = DateTime.Now;
            var user = new NBCZUser(User);
            model.Lmid = $"{user.UserCode}-{user.UserName}";
            var r = bll.Update(model);
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
            }

            return res;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(long id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.ChangeSotpStatus($"id={id}");
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <returns></returns>
        [Route("GetFunctions/{code}")]
        [HttpPost]
        public DataRes<IEnumerable<string>> GetFunctions(string code)
        {
            DataRes<IEnumerable<string>> res = new DataRes<IEnumerable<string>>() { code = ResCode.Success };

            var list = roleFunctionBLL.GetList($"roleCode='{code}'");
            res.data = list.Select(p => p.FunctionCode);

            return res;
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <returns></returns>
        [Route("SaveFunctions/{code}")]
        [HttpPost]
        public DataRes<bool> SaveFunctions(string code,[FromBody]List<string> functions)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            List<Pub_RoleFunction> list = new List<Pub_RoleFunction>();
            functions.ForEach(p =>{ list.Add(new Pub_RoleFunction() {FunctionCode=p,RoleCode=code }); });
            var r = bll.SaveFunctions(code, list);
            if (!r.Item1)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "保存失败";
            }

            return res;
        }


    }

}