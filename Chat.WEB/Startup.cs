using OneChat.BLL.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneChat.DAL.EF;
using OneChat.DAL.Interfaces;
using OneChat.DAL.Repositories;
using OneChat.BLL.Services;
using OneChat.WEB.HostedServices;
using OneChat.WEB.Middlewares;
using OneChat.WEB.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OneChat.WEB
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        public void ConfigureServices(IServiceCollection services)
        {

            string connection = Configuration.GetConnectionString("DefaultConnection"); 
            
            services.AddDbContextFactory<OperatorContext>(options => options.UseSqlServer(connection));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddSingleton<SendFilter>();
            services.AddControllersWithViews();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IBotUnitOfWork, BotEFUnitOfWork>();
            services.AddTransient<ILogic, Logic>();
            services.AddTransient<IStore, Store>();
            services.AddTransient<IBotStore, BotStore>();

            services.AddHostedService<BotHostedService>();
            services.Configure<BotOptions>(Configuration.GetSection("BotsSettings"));
            
            
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapWhen(context =>
            {
                return context.Request.Path.Value.ToLower().Equals("/chat/send");
            }, HandleQuery);


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void HandleQuery(IApplicationBuilder app)
        {
            app.UseMiddleware<UserStatsMiddleware>();
        }
    }
}