﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using webApi.Data;
using webApi.Services;

namespace webApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                configure: setup =>
                {
                    setup.ReturnHttpNotAcceptable = true;
                    // setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                }
                )
                .AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson(setup=>
                {
                    setup.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .ConfigureApiBehaviorOptions(
                setup => setup.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Type = "www.baidu.com",
                            Title = "error",
                            Status = StatusCodes.Status422UnprocessableEntity,
                            Detail = "请看详细信息",
                            Instance = context.HttpContext.Request.Path
                        };
                        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                        return new UnprocessableEntityObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }//rfc7807规定
                        };
                    }
                );
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddDbContext<RoutingDbcontext>(
                optionsAction:options=>options.UseSqlite(connectionString:"Data Source = routine.db")
                );
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(
                    appbuilder => appbuilder.Run(
                        async context => {
                            context.Response.StatusCode = 500;
                            await context.Response.WriteAsync(text: "Ubexpected Error");
                            }
                        )
                    );
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
