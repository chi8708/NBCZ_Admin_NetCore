using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using NBCZ.BLL;
using NBCZ.Common;
using NBCZ.Model;

namespace NBCZ.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Pub_User> Get()
        {
            //var log= LogFactory.GetLogger(Request.Path);
            //log.Info("info");
            //log.Warning("ok");
            //log.Error("error");
           
            return new Pub_UserBLL().GetList("");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Pub_User value)
        {
            //  Request.EnableRewind();
            if (Request.ContentLength>0)
            {
                Request.Body.Position = 0;
                var bytes = new byte[Request.Body.Length];
                Request.Body.Read(bytes, 0, bytes.Length);
                var data = System.Text.Encoding.UTF8.GetString(bytes);
            }
          
            var a = 0;
            var b = 1 / a;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
