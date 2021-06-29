using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using AuthApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace AuthApp.Controllers
{
    public class ChatController : Controller
    {

        private OperatorContext db;

        public ChatController(OperatorContext context)
        {
            db = context;
        }

        [Authorize]
        public IActionResult Index(string chat)
        {
            return View(db.ChatMessages.Where(message => message.ChatName == chat).ToList());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(ChatMessageModel model)
        {
            db.ChatMessages.Add(new ChatMessage { ChatName = model.ChatName, Nickname = HttpContext.User.Identity.Name, TimeOfPosting = DateTime.Now, Message = model.Message });
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Chat", new { chat = model.ChatName});
        }
    }
}


