using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
//using DapperExtensions;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;
using System.Text;
using NBCZ.Model;
using System.Data;

namespace NBCZ.DAL.Interface
{
    public partial interface IBaseData<T> where T : class ,new()
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long Insert(T model);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        bool InsertBatch(List<T> models);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Update(T model);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        bool UpdateBatch(List<T> models);


        /// <summary>
        ///根据实体删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        dynamic Delete(T model);


        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        dynamic Delete(object predicate);

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteByWhere(string where, object param = null);

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        bool DeleteBatch(List<T> models);

        /// <summary>
        /// 根据一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
       T Get(object id);

        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        T Get(object id, string keyName);

        /// <summary>
        /// 根据条件查询实体列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        List<T> GetList(string where, string sort = null, int limits = -1, string fileds = " * ", string orderby = "");

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="sort">分类</param>
        /// <param name="page">页索引</param>
        /// <param name="resultsPerPage">页大小</param>
        /// <param name="fields">查询字段</param>
        /// <returns></returns>
        PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*", Type result = null);

        /// <summary>
        /// 修改删除状态
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>

        bool ChangeSotpStatus(string where);
     
    }
}