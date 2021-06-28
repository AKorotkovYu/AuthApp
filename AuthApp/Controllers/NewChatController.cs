using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.ViewModels;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AuthApp.Models;

namespace AuthApp.Controllers
{

    [Authorize]
    public class NewChatController : Controller
    {
        private OperatorContext db;
        
        public NewChatController( OperatorContext context)
        {
            db = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost] 
        public async Task<IActionResult> CreateChat(ChatModel model)
        {
            if (ModelState.IsValid)
            {
                Chat chat = await db.Chats.FirstOrDefaultAsync(u => u.ChatName == model.ChatName);
                if (chat == null)
                {
                    db.Chats.Add(new Chat { ChatName = model.ChatName});
                    db.ChatMessages.Add(new ChatMessage { ChatName = model.ChatName, Nickname = "System", TimeOfPosting = DateTime.Now, Message = "Чат создан" });

                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return RedirectToAction("Index", "Home"); ;
        }
    }


    
}
