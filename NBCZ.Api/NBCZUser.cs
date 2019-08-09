using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace NBCZ
{
    public class NBCZUser
    {
        public NBCZUser(ClaimsPrincipal user)
        {
            this.User = user;
            this.Identity= (ClaimsIdentity)user.Identity;
        }

        public ClaimsPrincipal User { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string UserCode
        {
            get
            {
              
                var userCode = Identity.FindFirst(p => p.Type == ClaimTypes.NameIdentifier).Value;
                return userCode;               
            }
        }

        public  string UserName
        {
            get
            {
                var userName = Identity.FindFirst(p => p.Type == ClaimTypes.Name).Value;
                return userName;   
            }
        }

        public  string DeptCode
        {
            get
            {
                var deptCode = Identity.FindFirst(p => p.Type == ClaimTypes.GroupSid).Value;
                return deptCode; 
            }
        }


        public  string MobilePhone
        {
            get
            {
                var mobile = Identity.FindFirst(p => p.Type == ClaimTypes.MobilePhone).Value;
                return mobile;
            }
        }

        public  string Access
        {
            get
            {
                var access = Identity.FindFirst(p => p.Type == ClaimTypes.UserData).Value;
                return access;
            }
        }

        private class LoginAdmin
        {
            public string UserCode { get; set; }
            public string  UserName { get; set; }

            public string DeptCode { get; set; }

            public string MobilePhone { get; set; }

            public string Access { get; set; }

            // public List<Pub_Function> UserFunctions { get; set; }
        }
    }

    public static class NBCZFactory
    {
        public static NBCZUser GetNBCZUser(ClaimsPrincipal user)
        {
            return new NBCZUser(user);
        }
    }
}
