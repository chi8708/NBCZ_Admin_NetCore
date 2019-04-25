using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace NBCZ.Common
{
    public class AppConfigurtaionServices
    {
        public static IConfiguration Configuration { get; set; }
        static AppConfigurtaionServices()
        {

            var currentDir = Directory.GetCurrentDirectory();
            // var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //ReloadOnChange = true 当appsettings.json被修改时重新加载        
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"{currentDir}\\appsettings.json")
              // .Add(new JsonConfigurationSource { Path = "appsettings.json",Optional=false, ReloadOnChange = true })
              .Build();

          
        }
    }
}
