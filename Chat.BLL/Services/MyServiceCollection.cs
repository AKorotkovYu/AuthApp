using OneChat.BLL.Interfaces;
using OneChat.BLL.BusinessModel;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MyServiceCollection
    {
        public static void AddConfig( this IServiceCollection services)
        {
            services.AddSingleton<IBot, TimeBot>();
            services.AddSingleton<IBot, JokeBot>();
            services.AddSingleton<IBot, DownloadBot>();
        }
    }
}