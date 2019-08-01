using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCZ.BLL;
using NBCZ.Model;

namespace NBCZ.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/PubDept")]
    public class PubDeptController : Controller
    {
        Pub_DepartmentBLL bll = new Pub_DepartmentBLL();

        [Route("GetList")]
        [HttpPost]
        public DataRes<List<Pub_Department>> GetList()
        {
            var depts = bll.GetList("StopFlag=0");

            return new DataRes<List<Pub_Department>>() { data = depts };
        }
    }
}