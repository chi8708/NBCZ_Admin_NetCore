using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCZ.BLL.Interface;
using NBCZ.Model;

namespace NBCZ.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/PubFunction")]
    public class PubFunctionController : Controller
    {

        IPub_FunctionBLL bll = null;
        IV_PubFunction_ParentBLL functionParentBLL =null;

        public PubFunctionController(IPub_FunctionBLL bll, IV_PubFunction_ParentBLL functionParentBLL)
        {
            this.bll = bll;
            this.functionParentBLL = functionParentBLL;
        }

        [HttpPost]
        [Route("GetList")]
        public DataRes<List<Pub_Function>> GetList()
        {
            var depts = bll.GetList("StopFlag=0"," sortidx asc");

            return new DataRes<List<Pub_Function>>() { data = depts };
        }

        /// <summary>
        /// 获取子权限列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetChildList")]
        [Route("GetChildList/{code}")]
        public DataRes<List<V_PubFunction_Parent>> GetChildList(string code= "FC001")
        {

            var depts = functionParentBLL.GetList(string.Format(" StopFlag=0 And FunctionCode IN (Select FunctionCode From f_SearchChildFunction('{0}'))", code), " FunctionCode ");

            return new DataRes<List<V_PubFunction_Parent>>() { data = depts };
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [Route("Add")]
        [HttpPost]
        public DataRes<bool> Add([FromBody]Pub_Function model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.FunctionCode =bll.GetCode(model.ParentCode);
            model.editdate = DateTime.Now;
            var user = NBCZFactory.GetNBCZUser(User);
            model.editor = $"{user.UserCode}-{user.UserName}";
            model.StopFlag = false;
            var r = bll.Insert(model);
            return res;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        [Route("Edit")]
        [HttpPost]
        public DataRes<bool> Edit([FromBody]Pub_Function model)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            model.editdate= DateTime.Now;
            var user = NBCZFactory.GetNBCZUser(User);
            model.editor = $"{user.UserCode}-{user.UserName}";
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Route("Delete/{id}")]
        [HttpPost]
        public DataRes<bool> Delete(string id)
        {
            DataRes<bool> res = new DataRes<bool>() { code = ResCode.Success, data = true };

            var r = bll.ChangeSotpStatus($"FunctionCode='{id}'");
            if (!r)
            {
                res.code = ResCode.Error;
                res.data = false;
                res.msg = "删除失败";
            }

            return res;
        }
    }
}