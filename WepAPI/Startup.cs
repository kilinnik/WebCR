using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using WepAPI.Models;
using WepAPI.Repository;
using WepAPI.Services;

namespace WebAPI
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
            services.AddScoped<IDataService<DataLogin>, GenericDataService<DataLogin>>();
            services.AddScoped<IDataService<Patient>, GenericDataService<Patient>>();
            services.AddScoped<IDataService<Area>, GenericDataService<Area>>();
            services.AddScoped<IDataService<Complaints>, GenericDataService<Complaints>>();
            services.AddScoped<IDataService<Diagnosis>, GenericDataService<Diagnosis>>();
            services.AddScoped<IDataService<Doctor>, GenericDataService<Doctor>>();
            services.AddScoped<IDataService<Preparation>, GenericDataService<Preparation>>();
            services.AddScoped<IDataService<SickLeave>, GenericDataService<SickLeave>>();
            services.AddScoped<IDataService<Survey>, GenericDataService<Survey>>();
            services.AddScoped<IDataService<Timetable>, GenericDataService<Timetable>>();
            services.AddScoped<IDataService<Visit>, GenericDataService<Visit>>();

            services.AddDbContext<СlinicReceptionDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "WEB API",
                        Version = "v1"
                    });
            });
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });
            //JSON Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WEB API");
                c.DocumentTitle = "WEB API";
                c.DocExpansion(DocExpansion.List);
            });
        }
    }
}

