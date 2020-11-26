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
            //�ڹ���֮������֮ǰ��һЩ��
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    //��Ϊ����û�й��캯�����Դ˻�÷���
                    var Dbcontext = scope.ServiceProvider.GetService<RoutingDbcontext>();
                    //Dbcontext.Database.EnsureDeleted();//���ݿ������ɾ��
                    Dbcontext.Database.Migrate();//����������Ǩ��
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
