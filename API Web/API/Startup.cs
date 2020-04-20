using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DAL;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
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
            services.AddSingleton<IDbService>();
            services.AddTransient<IStudentDbService, SQLserverDbService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService dbservice)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();
            
            app.UseWhen(context => context.Request.Path.ToString().Contains("secret"), app =>
            {
                app.Use(async (context, next) =>
                {

                    if (!context.Request.Headers.ContainsKey("Index"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("W naglowku potrzebny indeks studenta");
                        return;
                    }

                    var index = context.Request.Headers["Index"].ToString();

                    var student = dbservice.getStudent(index);

                    if (student == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status204NoContent;
                        context.Response.WriteAsync("Brak takiego studenta");
                    }

                    await next();
                });
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}