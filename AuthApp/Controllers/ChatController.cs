using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using AuthApp.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using AuthApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IServiceProvider serv;
        private readonly IStore store;

        public ChatController(OperatorContext context, IServiceProvider serv, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.serv = serv ?? throw new ArgumentNullException(nameof(serv));
            store.SetDb(context);
        }

        [Authorize]
        public IActionResult Index(int chatId)
        {
            List<ChatMessage> chatMessage = new();

            foreach (ChatMessage message in store.FindMessages(chatId))
            {
                    if (DateTime.Now >= message.TimeOfPosting.AddDays(+1) )
                        message.isOld = true;
                    else
                        message.isOld = false;
                chatMessage.Add(message);
            }
            return View(chatMessage);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(ChatMessageModel model)
        {
            ChatMessage message = new() { 
                ChatId = model.ChatId,
                ChatName = model.ChatName,
                Message = model.Message,
                Nickname = HttpContext.User.Identity.Name,
                TimeOfPosting = System.DateTime.Now 
            };
             store.SaveMessage(message);

            var bots = serv.GetServices<IBot>();
            foreach (var bot in bots)
            {
                string answer = bot.Execute(model.Message);

                if (answer != null)
                {
                    ChatMessage botMessage = new()
                    {
                        ChatId = model.ChatId,
                        ChatName = model.ChatName,
                        Message = bot.Execute(model.Message),
                        Nickname = bot.Name,
                        TimeOfPosting = System.DateTime.Now
                    };

                    store.SaveMessage(botMessage);
                }
            }
            await store.SaveChanges();
            return RedirectToAction("Index", "Chat", new { chatId = model.ChatId});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DelMes(int chatId, int mesId)
        { 
            store.Remove(store.FindMessage(mesId));
            await store.SaveChanges();
            return RedirectToAction("Index", "Chat", new { chatId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Exit(string nickname, int chatId)
        {
            await store.DelUserFromChat(store.FindUser(nickname).Id, chatId);
            await store.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


    }
}


