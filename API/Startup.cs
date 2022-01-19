using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Di container 
        public void ConfigureServices(IServiceCollection services)
        {
         
            
            // Registering automapper as service.
            services.AddAutoMapper(typeof(MappingProfiles)); // mapping profiles is class where we have provided mappings.
            services.AddControllers();

            // add dbcontext as service
            // ordering dosent matter here. ordering matter in middleware section

             services.AddDbContext<StoreContext>(x => 
            x.UseSqlServer(_config.GetConnectionString("StoreSqlServerConnection")));

            // add identiy db context as service
            services.AddDbContext<AppIdentityDbContext>(x => 
            x.UseSqlServer(_config.GetConnectionString("IdentitySqlServerConnection")));

            // Adding redis connnection settiong
 
             services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(
                    _config.GetConnectionString("Redis"), true);
                     configuration.Password =  _config.GetConnectionString("RedisPassword");
                return ConnectionMultiplexer.Connect(configuration);
            });

           services.AddApplicationServices(); // coming from extension method.
           services.AddIdentityServices(_config); // coming from extension
           services.AddSwaggerDocumentation(); // coming from swagger extension


           // adding CORS
           services.AddCors(opt => {
               opt.AddPolicy("CorsPolicy", policy => {
                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
               });
           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Places to add middleware..
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // Using custom middleware.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // When no endpoint match it will hit this middleware which will redirect to route /errors.

            app.UseHttpsRedirection(); // this redirect http request to https.

            app.UseRouting(); // Enable to use us routing.

            app.UseStaticFiles(); // Enabling server to serve ststic file. it serve file from wwwroot folder

            // following is to serve image from content folder
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ),
                RequestPath = "/content"
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication(); // must before UseAuthorization

            app.UseAuthorization();

            app.UseSwaggerDocumentation(); // from extension method.


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // following setting for angular as we are serving angular page from api
                 endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
