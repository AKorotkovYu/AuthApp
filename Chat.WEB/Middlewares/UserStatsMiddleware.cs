using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneChat.BLL.Interfaces;
using OneChat.BLL.DTO;

namespace OneChat.WEB.Middlewares
{

    public class UserStatsMiddleware : Attribute
    {
        private readonly RequestDelegate _next;

        public UserStatsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStore store, ILogic logic)
        {
                if (Int32.TryParse(context.User.Identity.Name, out int id))
                {
                    var user = await store.GetUserAsync(id);
                    var dayFromRegistration = user.DateOfRegistration.Date - DateTime.Now.Date;
                    user.AverageMessageCountInDay = user.MessageActivity/(dayFromRegistration.Days+1);
                    logic.UpdateUser(user);
                }

          await _next.Invoke(context);
        }
    }
}
