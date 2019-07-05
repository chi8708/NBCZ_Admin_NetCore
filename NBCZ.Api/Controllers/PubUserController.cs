using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCZ.Model;

namespace NBCZ.Api.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/PubUser")]
    public class PubUserController : Controller
    {
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
        var claimIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimIdentity.FindFirst(p => p.Type == ClaimTypes.Name).Value;
            var userCode = claimIdentity.FindFirst(p => p.Type == ClaimTypes.NameIdentifier).Value;
            var access= claimIdentity.FindFirst(p => p.Type == ClaimTypes.UserData).Value;
            return new {
                access=access,
                avatar = "https://file.iviewui.com/dist/a0e88e83800f138b94d2414621bd9704.png",
                name = userName,
                user_id=userCode
            };
        }
    }
}