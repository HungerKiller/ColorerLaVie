using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PhotoMasterBackend.Mappings;
using PhotoMasterBackend.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoMasterBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PhotoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PhotoDatabase")));

            // Repositories
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();

            // Auto Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Cors
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhotoMasterBackend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhotoMasterBackend v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // The local/network folder containing static files we are mapping to
            var staticFilesFolder = Configuration.GetSection("StaticFilesFolder").Value;
            // Create directory if not exists
            if (!Directory.Exists(staticFilesFolder))
                Directory.CreateDirectory(staticFilesFolder);

            if (!string.IsNullOrWhiteSpace(staticFilesFolder))
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(staticFilesFolder),
                    RequestPath = Configuration.GetSection("StaticFilesUrlPath").Value // The path in the url we will manage
                });
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
