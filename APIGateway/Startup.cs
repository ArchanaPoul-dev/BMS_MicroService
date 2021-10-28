using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway
{
    
    public class Startup
    {
        readonly string allowspecificOrigins = "AllowOrigin";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddOcelot();

            services.AddCors(o =>
            {
                o.AddPolicy(name: allowspecificOrigins, builder =>
                {
                    builder.WithOrigins("http://localhost:4200", "http://localhost:4300")
                       .AllowAnyHeader()
                       .AllowAnyMethod();

                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(
                    options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()
                         ); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           await app.UseOcelot();
        }
    }
}
