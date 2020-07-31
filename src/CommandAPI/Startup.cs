using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CommandAPI.Data;
using CommandAPI.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace CommandAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
           
             var builder = new NpgsqlConnectionStringBuilder();
            builder.ConnectionString =Configuration.GetConnectionString("CommanderConnection");
            builder.Username= Configuration["UserID"];
            builder.Password = Configuration["Password"];

            services.AddDbContext<CommandAPIContext>(opt => opt.UseNpgsql
                (builder.ConnectionString)
            );

             services.AddControllers();
            // services.AddScoped<ICommandAPIRepo, MockCommandRepo>();
            // services.AddScoped<ICommandAPIRepo,DbCommandAPIRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
        CommandAPIContext context)
        {
            // context.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//for our endpoints specified 
                //within configure services method
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Hello World!");
                // });
            });
        }
    }
}
