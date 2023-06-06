一、框架概述 
-------------
1. NBCZ_Admin_NetCore是一个前后端分离通用权限系统， 用vs2017+sqlserver2012开发工具。
2. 后端标准三层结构：
   1. 基于NETStandard2.0标准类库。
   2. Repository（DAL仓储层）使用Dapper.Contrib+Dapper开发。
   3. api使用asp.net core webapi,jwt身份认证。
3. 前端
   1. 基于vue的iview框架。
4. 在线预览http://106.14.91.48:8918/ 登录名：admin 密码：123123
5. ![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+)
   *注：此版本不再维护，已升级至<a href="https://github.com/chi8708/CNet_Admin" >.net5前后端分离项目</a>* </sub> 
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
<img src="https://github.com/chi8708/CNet_Admin/blob/main/main.png" width="900px;">

四、版本
---------------
<table>
<tr><th>语言/框架</th><th>地址</th><th>协议</th><th>备注</th></tr>
<tr>
	<td>.Net Framework</td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin" >.net framework+layui+dapper-extensions</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td></td>	
</tr>
<tr>
	<td>.Net Framework + Vue </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_Vue" >.net framework+iview+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_Vue/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>	
</tr>
<tr>
	<td>.Net Core</td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_NetCore" >.net core+iview+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_NetCore/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>
</tr>
<tr>
	<td>.Net5</td>
	<td><a href="https://github.com/chi8708/CNet_Admin" >.net5+iview+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/CNet_Admin/blob/main/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>
</tr>
</table>

五、依赖/中间件
--------------------------
- Dapper：https://github.com/StackExchange/Dapper
- Dapper.Contrib：https://github.com/StackExchange/Dapper/tree/master/Dapper.Contrib
- Swagger：https://swagger.io/
- log4net：http://logging.apache.org/log4net/download_log4net.cgi

* iview： https://www.iviewui.com/
* iview-admin：http://admin.iviewui.com/login

交流提升
-------------
QQ群:点击加群  <a href='https://jq.qq.com/?_wv=1027&k=4je1frWy' target="_blank" >851743573  </a>

给Aigu赞赏
-----------------
<img src="https://github.com/chi8708/NBCZ/blob/master/zs.jpg" />


Aigu开发 github热门排行，微信小程序看源码
-----------------
<img src="tg.jpg" style="width:260px;" />

---------------

