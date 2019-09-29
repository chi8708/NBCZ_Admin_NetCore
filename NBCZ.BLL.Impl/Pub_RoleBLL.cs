using NBCZ.DAL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBCZ.BLL.Interface;

namespace NBCZ.BLL.Impl
{

    public partial class Pub_RoleBLL:IPub_RoleBLL
    {

        IPub_RoleFunctionBLL roleFunctionBLL = null;
        public Pub_RoleBLL(IPub_RoleFunctionBLL roleFunctionBLL)
        {
            this.roleFunctionBLL = roleFunctionBLL;
        }
        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "RC000001";
            List<Pub_Role> roles = GetList("", " Id Desc ", 1);
            if (roles.Count > 0)
            {
                var model = roles.First();
                code ="RC"+ (Convert.ToInt32(model.RoleCode.Remove(0, 2)) + 1).ToString().PadLeft(6, '0');
            }

            return code;
        }

        public (bool, string) SaveFunctions( string code, List<Pub_RoleFunction> functions)
        {
            var r= roleFunctionBLL.DeleteByWhere($"RoleCode='{code}'");
                r= roleFunctionBLL.InsertBatch(functions);

            return (r, r?"保存成功":"保存失败");

        }
    }
}
