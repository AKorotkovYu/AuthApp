using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Threading;
using OneChat.BLL.Interfaces;
using OneChat.DAL.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;
using OneChat.BLL.Services;
using System.Collections.Generic;

namespace OneChat.WEB.HostedServices
{
    public class CheckFIFOHostedService : BackgroundService
    {

        private readonly IStore store;

        public CheckFIFOHostedService(IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }



        protected override Task ExecuteAsync(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    //await DistributeFIFOAsync();
                    await Task.Delay(1000, token);
                }

            }, token);
        }
    }
}
