using Microsoft.AspNetCore.Mvc;
using OneChat.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using OneChat.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using OneChat.BLL.DTO;

namespace OneChat.WEB.Controllers
{
    public class ChatController : Controller
    {
        private readonly IServiceProvider serv;
        private readonly IStore store;
        UserDTO user;

        public ChatController(IServiceProvider serv, IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.serv = serv ?? throw new ArgumentNullException(nameof(serv));
        }

        [Authorize]
        public IActionResult Index(int chatId)
        {
            return View(store.GetMessages(chatId));
        }


        [Authorize]
        [HttpPost]
        public IActionResult Send(ChatMessageModel model)
        {
            store.SaveMessage(new()
            {
                ChatId = model.ChatId,
                ChatName = model.ChatName,
                Message = model.Message,
                Nickname = User.Claims.Where(c => c.Type == "Nickname").Select(c => c.Value).SingleOrDefault(),
                TimeOfPosting = System.DateTime.Now
            });

            var bots = serv.GetServices<IBot>();

            foreach (var bot in bots)
            {
                string answer = bot.Execute(model.Message);
                if (answer != null)
                {
                    store.SaveMessage(new()
                    {
                        ChatId = model.ChatId,
                        ChatName = model.ChatName,
                        Message = answer,
                        Nickname = bot.Name,
                        TimeOfPosting = System.DateTime.Now
                    });
                }
            }
            store.SaveChanges();
            return RedirectToAction("Index", "Chat", new { chatId = model.ChatId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult DelMes(int chatId, int mesId)
        {
            if(store.GetMessage(mesId).TimeOfPosting>DateTime.Now.AddDays(-1))
                store.RemoveMessage(mesId);
            
            store.SaveChanges();
            return RedirectToAction("Index", "Chat", new { chatId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Exit(int userId, int chatId)
        {
            store.DelUserFromChat(store.GetUser(Int32.Parse(User.Identity.Name)).Id, chatId);
            store.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


    }
}


