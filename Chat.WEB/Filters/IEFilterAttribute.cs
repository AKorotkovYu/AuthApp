using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace OneChat.WEB.Filters
{

    public class IEFilterAttribute: Attribute, IAsyncResourceFilter
    {

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            if (Regex.IsMatch(userAgent, "MSIE|Trident"))
            {
                context.Result = new ContentResult { Content = "Ваш браузер устарел" };
            }
            else
                await next();
        }


    }
}
