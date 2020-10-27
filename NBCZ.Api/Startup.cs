using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace NBCZ.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //log4Net
            var repository = LogManager.CreateRepository(Common.LogFactory.repositoryName);
            // 指定配置文件
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));

        }



        public IConfiguration Configuration { get; }

        //依赖注入服务
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //1.全局异常 2.Json 日期格式化
            services
                .AddMvc(o=> {o.Filters.Add(typeof(WebApiExceptionAttribute)) ; })
                .AddJsonOptions(options =>{options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";});

            //参考 https://www.cnblogs.com/aishangyipiyema/p/9262642.html
            JWTConfig(services);


            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "NBCZ API", Version = "v1",
                    Description = "NBCZ基础框架API" ,License = new License
                    {
                        Name = "MIT",
                        Url = "https://github.com/chi8708/NBCZ_Admin_NetCore/blob/master/LICENSE"
                    }
                    });

                //swagger中控制请求的时候发是否需要在url中增加accesstoken
                c.OperationFilter<AuthTokenHeaderParameter>();

                // 为 Swagger JSON and UI设置xml文档注释路径
                //HttpContext.Current.Request.PhysicalApplicationPath
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "NBCZ.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            //services.AddIdentityServer(options => options.Authentication.CookieAuthenticationScheme = "Cookies")
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryIdentityResources(Config.GetIdentityResources())
            //    .AddInMemoryApiResources(Config.GetApis())
            //    .AddInMemoryClients(Config.GetClients())
            //    .AddTestUsers(Config.GetUsers());
            // configure identity server with in-memory stores, keys, clients and scopes

            //jwt
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        // IdentityServer4 地址
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;
            //        options.Audience = "api1";
            //    });
        }


        /// <summary>
        /// 使用 Microsoft.AspNetCore.Authentication.JwtBearer
        /// </summary>
        /// <param name="services"></param>
        private void JWTConfig(IServiceCollection services)
        {
            services.Configure<JwtSeetings>(Configuration.GetSection("JwtSeetings"));

            var jwtSeetings = new JwtSeetings();
            //绑定jwtSeetings
            Configuration.Bind("JwtSeetings", jwtSeetings);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSeetings.Issuer,
                    ValidAudience = jwtSeetings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSeetings.SecretKey))
                };
            });


            //core 2.2 跨域
            //services.AddCors(op =>
            //{
            //    op.AddPolicy("cors", set =>
            //    {
            //        set.SetIsOriginAllowed(origin => true)
            //           .AllowAnyHeader()
            //           .AllowAnyMethod()
            //           .AllowCredentials();
            //    });
            //});
        }

        //中间件
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //jwt认证 需要在app.UseMvc()前调用
            app.UseAuthentication();

            //跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.AllowCredentials();
                // builder.WithOrigins("http://localhost:8080");
            });

            //core 2.2 跨域
            //app.UseCors("cors");

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NBCZ API V1");

               // c.RoutePrefix = string.Empty;
            });


            //app.UseIdentityServer();
            app.UseMvc();
  
            app.UseStaticFiles();

           
          
        }
    }
}
