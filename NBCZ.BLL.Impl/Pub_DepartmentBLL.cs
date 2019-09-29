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

    public partial class Pub_DepartmentBLL:IPub_DepartmentBLL
    {
        /// <summary>
        /// 获取部门编号
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public string GetCode()
        {
            var code = "D000001";
            List<Pub_Department> depts = GetList("", " DeptCode Desc ", 1);
            if (depts.Count > 0)
            {
                var dept = depts.First();
                code="D"+(Convert.ToInt32(dept.DeptCode.Remove(0,1))+1).ToString().PadLeft(6,'0');
            }

            return code;
        }
    }
}
