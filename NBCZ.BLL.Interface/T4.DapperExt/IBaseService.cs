using System;
using System.Collections.Generic;
using NBCZ.Model;

namespace NBCZ.BLL.Interface
{
    public partial interface IBaseService<T> where T : class, new()
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
        ///根据实体删除 id必须是int 或 guid
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
        /// 根据实体删除 id必须是int 或 guid
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        bool DeleteBatch(List<T> models);

        /// <summary>
        /// 获取一个实体对象
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
        /// <param name="where">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="limits">前几条</param>
        /// <returns></returns>
        List<T> GetList(string where, string sort = null, int limits = -1, string fields = " * ", string orderby = "");

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <returns></returns>
        PageDateRes<T> GetPage(string where, string sort, int page, int resultsPerPage, string fields = "*");

        bool ChangeSotpStatus(string where);
 
    }
}