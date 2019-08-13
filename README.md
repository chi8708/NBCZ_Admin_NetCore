一、框架概述 <a href="https://github.com/chi8708/NBCZ_Admin" style="font-size:13px;" >转至.net framework+layui项目</a>
-------------
1. NBCZ_Admin_NetCore是一个前后端分离通用权限系统， 用vs2017+sqlserver2012开发工具。
2. 后端标准三层结构：
   1. 基于NETStandard2.0标准类库。
   2. Repository（DAL仓储层）使用Dapper.Contrib+Dapper开发。
   3. api使用asp.net core webapi,jwt身份认证。
3. 前端
   1. 基于vue的iview框架。
--------  

二、配置使用
-------------------
1. 项目文件基本配置
    1. git clone项目，修改文件夹及sln、csproj、user文件名称为项目命名空间。

    2. 修改sln、csproj内容 将NBCZ修改为项目命名空间。

    3. 用vs打开项目，整个解决方案替换将NBCZ修改为项目命名空间。

2. 数据库配置
    1. 还原数据库db→NBCZ
    2. 配置 DBUtility项目下的DbEntity.ttinclude
    3. 配置api项目下的appsettings.json 数据库连接

3. 代码生成
    1. 按Model、DAL、BLL的顺序, 分别保存项目文件 T4.DapperExt→后缀为.tt文件。
	
4. 前端
    1. 进入NBCZ.Web.Admin目录，运行命令：npm install、npm run dev。
----------

三、主界面如下：
---------------
<img src="https://github.com/chi8708/NBCZ_Admin_NetCore/blob/master/main.png" width="900px;">

四、参考文档
--------------------------
- Dapper：https://github.com/StackExchange/Dapper
- Dapper.Contrib：https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib
* iview： https://www.iviewui.com/
* iview-admin：http://admin.iviewui.com/login

给Aigu赞赏
-----------------
<img src="https://github.com/chi8708/NBCZ/blob/master/zs.jpg" />



