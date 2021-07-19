using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using OneChat.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;


namespace OneChat.WEB.Filters
{
    public class SendFilter : IActionFilter
    {
        private readonly IStore store;
        private readonly ILogic logic;

        public SendFilter(IStore store, ILogic logic)
        {
            this.store = store;
            this.logic = logic;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
                
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (Int32.TryParse(context.HttpContext.User.Identity.Name, out int id))
                {
                    var user = store.GetUser(id);
                    var dayFromRegistration = user.DateOfRegistration.Date - DateTime.Now.Date;
                    user.AverageMessageCountInDay = user.MessageActivity / (dayFromRegistration.Days + 1);
                    logic.UpdateUser(user);
                }
        }
    }
}
