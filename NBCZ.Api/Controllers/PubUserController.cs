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
    public class PubUserController : Controller
    {
        private V_PubUser_DeptBLL userDeptBLL = new V_PubUser_DeptBLL();
        private Pub_UserRoleBLL userRoleBLL = new Pub_UserRoleBLL();

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
            return new {
                access=access,
                avatar = "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
                name = userName,
                user_id=userCode
            };
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <returns></returns>
        [Route("GetPage")]
        [HttpPost]
        public JsonResult GetPage([FromBody]PageDataReq pageReq )
        {
            var users = userDeptBLL.GetPage(GetWhereStr(), (pageReq.field + " " + pageReq.order), pageReq.pageNum, pageReq.pageSize);

            return Json(users);
        }

        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");

         //   sb.AppendFormat(" and {0} ", this.HttpContext.GetWhereStr());

            return sb.ToString();
        }
    }
}