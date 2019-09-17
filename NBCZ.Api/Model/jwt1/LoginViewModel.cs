using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NBCZ.Api
{

    public class LoginViewModel
    {
        //[Required]
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        //[Required]
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
