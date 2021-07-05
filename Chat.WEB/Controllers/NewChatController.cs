using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using OneChat.BLL.DTO;
using OneChat.BLL.Interfaces;
using OneChat.WEB.Models;
using OneChat.WEB.Controllers;

namespace OneChat.WEB.Controllers
{

    [Authorize]
    public class NewChatController : Controller
    {
        IStore store;

        public NewChatController(IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
            this.store=store;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateChat(ChatModel chatModel)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = store.GetUser(User.Identity.Name);//TODO: USERID

                ChatDTO newChat = new()
                {
                    ChatName = chatModel.ChatName,
                    AdminId = userDTO.Id,
                };
                
                newChat=store.AddChat(newChat);

                store.SaveMessage(new()
                {
                    ChatId = newChat.Id,
                    ChatName = newChat.ChatName,
                    Message = "чат создан",
                    Nickname = "system",
                    TimeOfPosting = System.DateTime.Now,
                    isOld = true
                });

                store.AddUserToChat(userDTO.Id, newChat.Id);
                store.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }

}
