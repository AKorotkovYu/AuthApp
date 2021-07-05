using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneChat.BLL.Interfaces;
using System;

namespace AuthApp.Controllers
{
    public class AddUserToChatController : Controller
    {
        IStore store;

        public AddUserToChatController(IStore store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public IActionResult Index(int chatId)
        {
            return View(store.AllAnotherUsers(chatId));
        }

        [Authorize]
        public IActionResult AddUser(int userId, int chatId)
        {
            store.AddUserToChat(userId, chatId);
            store.SaveChanges();

            return RedirectToAction("Index", "AddUserToChat", new { chatId });
        }
    }
}
