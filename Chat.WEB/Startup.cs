﻿
using OneChat.BLL.BusinessModel;
using OneChat.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneChat.DAL.EF;
using OneChat.DAL.Interfaces;
using OneChat.DAL.Repositories;

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
            services.AddDbContext<OperatorContext>(options => options.UseSqlServer(connection));

            // óñòàíîâêà êîíôèãóðàöèè ïîäêëþ÷åíèÿ
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            
            services.AddControllersWithViews();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddTransient<IBot, TimeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, DownloadBot>();

            services.AddTransient<IStore, Store>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}