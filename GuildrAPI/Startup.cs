﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using GuildrAPI.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace GuildrAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime and adds services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<GuildrAPIContext>(options =>
                                options.UseSqlite(Configuration.GetConnectionString("GuildrAPIContext")));

            //Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GuildrAPI", Version = "v1" });
            });
        }

        //This method gets called by the runtime and configures the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            //Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
            //Specifies the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                //Launch swagger from root
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; 
            });
        }
    }
}
