using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NBCZ.BLL;
using NBCZ.Common;

namespace NBCZ.Api.Controllers
{
    /// <summary>
    /// 认证
    /// </summary>
    [Route("api/[controller]")]
    //jwt1 身份认证
    public class AuthroizeController : ControllerBase
    {
        private readonly JwtSeetings _jwtSeetings;

        public AuthroizeController(IOptions<JwtSeetings> jwtSeetingsOptions)
        {
            _jwtSeetings = jwtSeetingsOptions.Value;
        }

        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="loginViewModel">登录实体信息</param>
        /// <returns></returns>
        [HttpPost,AllowAnonymous]
        public ActionResult Post([FromBody]LoginViewModel loginViewModel)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var users = new Pub_UserBLL().GetList($"StopFlag=0 AND UserName='{loginViewModel.Name}' AND UserPwd='{loginViewModel.Password}'", limits: 1);
            
            if (users.Count>0)
            {
                var user = users.First();
                var userFunctions = new  Pub_UserFunctionBLL().GetList($"UserCode='{user.UserCode}'").Select(p=>p.FunctionCode);
                var roleFunctions = new Pub_RoleFunctionBLL().GetList($" RoleCode IN(SELECT pur.RoleCode FROM Pub_UserRole AS pur WHERE pur.UserCode='{user.UserCode}' )").Select(p=>p.FunctionCode);
                var functions = userFunctions.Concat(roleFunctions).Distinct();
                var functionsStr = string.Join(',', functions);
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Sid,user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,user.UserCode),
                    new Claim(ClaimTypes.UserData,functionsStr),
                    new Claim(ClaimTypes.MobilePhone,user.Tel),
                    new Claim(ClaimTypes.GroupSid,user.DeptCode)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSeetings.SecretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expires = DateTime.Now.AddMinutes(30);
                var token = new JwtSecurityToken(
                    _jwtSeetings.Issuer,
                    _jwtSeetings.Audience,
                    claims,
                    DateTime.Now,
                   expires,
                    creds
                    );
                return Ok(new ResponseObj<dynamic>()
                {
                    Code = 1,
                    Message = "认证成功",
                    Data = new { Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expires = TypeUtil.ConvertDateTimeInt(expires) }
                });
            }

            return Ok(new ResponseObj<dynamic>()
            {
                Code = 0,
                Message = "用户名密码错误！"
            });
            //return BadRequest();
        }
    }
}