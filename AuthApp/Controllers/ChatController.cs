using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using AuthApp.ViewModels;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

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
            
            List<string> users = new();
            List<DateTime> dates = new();
            List<string> texts = new();

            if (db.ChatMessages != null)
            { 
                foreach (ChatMessage message in db.ChatMessages)
                {
                    if (message.ChatName == chat)
                    {
                        users.Add(message.Nickname);
                        dates.Add(message.TimeOfPosting);
                        texts.Add(message.Message);
                    }
                }

                ViewBag.Chat = chat;
                ViewBag.ListOfUsers = users;
                ViewBag.ListOfDates = dates;
                ViewBag.ListOfTexts = texts;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(ChatMessageModel model)
        {
            db.ChatMessages.Add(new ChatMessage { ChatName = model.ChatName, Nickname = model.Nickname, TimeOfPosting = DateTime.Now, Message = model.Message });
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Chat", new { chat = model.ChatName});
        }
    }
}


