using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopApp.Infrastructure;

using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
using ShopApp.Models;
using Microsoft.AspNetCore.Identity;
using ShopApp.Modules.ExternalModules;
using ShopApp.Hubs;
using ShopApp.Modules.InnerModules;

namespace ShopApp
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
            services.AddLogging();
            services.AddDbContext<ShopAppDbContext>(d => d.UseSqlServer(Configuration.GetConnectionString("ShopAppDb")));
            services.AddIdentity<User, IdentityRole>(o=> {
                o.Password.RequireDigit = false;
                o.Password.RequiredLength = 1;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;

                o.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ShopAppDbContext>();

            services.AddScoped<IRepository, EFShopRepository>();

            services.AddTransient<DataGenerator>();
            services.AddTransient<IVerificationUserAccess, VerificationUserAccess>();

            services.AddMemoryCache();

            services.AddSignalR();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();
            services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

            services.AddControllersWithViews(options => options.CacheProfiles.Add("Caching", new CacheProfile()
            {
                Duration = 60,
                Location = ResponseCacheLocation.Client
            }));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseReact(config => { });
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

/*            app.Use(async (context, next) =>
            {
                var db = context.RequestServices.GetService<ShopAppDbContext>();
                var generator = context.RequestServices.GetService<DataGenerator>();



                await next.Invoke();
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotifyHub>("/notify");
                endpoints.MapControllers();
            });
        }
    }
}
