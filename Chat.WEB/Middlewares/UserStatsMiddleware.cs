using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneChat.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace OneChat.WEB.Middlewares
{

    public class UserStatsMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatsMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        [Authorize]
        public async Task InvokeAsync(HttpContext context, IStore store, ILogic logic)
        {
            if (context.Request.RouteValues.ContainsKey("action"))
            if (context.Request.RouteValues["action"].ToString().ToLower().Equals("send"))
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
