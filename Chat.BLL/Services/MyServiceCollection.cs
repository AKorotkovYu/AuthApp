using OneChat.BLL.Interfaces;
using OneChat.BLL.BusinessModel;
using OneChat.BLL.Services;
using OneChat.DAL.Repositories;
using OneChat.DAL.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OneChat.DAL.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace OneChat.BLL.Services
{
    public class MyServiceCollection
    {

        public MyServiceCollection(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IEnumerable<IBot> AddConfig()
        {
            var services = new ServiceCollection();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OperatorContext>(options => options.UseSqlServer(connection));
            services.AddTransient<IBot, TimeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, JokeBot>();
            services.AddTransient<IBot, DownloadBot>();
            services.AddTransient<IBotUnitOfWork, BotEFUnitOfWork>();
            services.AddTransient<IBotStore, BotStore>();

            var servicesProvider = services.BuildServiceProvider();

            return servicesProvider.GetServices<IBot>();
        }
    }
}