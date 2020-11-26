using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webApi.Data;

namespace webApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //在构造之后，运行之前做一些事
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    //因为这里没有构造函数，以此获得服务
                    var Dbcontext = scope.ServiceProvider.GetService<RoutingDbcontext>();
                    //Dbcontext.Database.EnsureDeleted();//数据库存在则删除
                    Dbcontext.Database.Migrate();//不存在则做迁移
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                    logger.LogError(ex, message: "Database Migration Error");
                }
            }

                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
