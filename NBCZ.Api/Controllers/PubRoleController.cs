using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [Route("GetList")]
        [HttpPost]
        public DataRes<List<Pub_Role>> GetList()
        {
            var roles = bll.GetList("StopFlag=0");

            return new DataRes<List<Pub_Role>> { data= roles} ;
        }
    }

}